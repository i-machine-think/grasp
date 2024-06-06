import argparse
import sys

import clip
import torch
import yaml
from torchvision.transforms import CenterCrop, Compose, Normalize, Resize

from grasp.models.wrapper import ModelWrapper
from grasp.utils import blue, green

try:
    from torchvision.transforms import InterpolationMode
    BICUBIC = InterpolationMode.BICUBIC
except ImportError:
    from PIL import Image
    BICUBIC = Image.BICUBIC


def load_video(video_path, clip_model):
    from vtimellm.mm_utils import VideoExtractor

    video_loader = VideoExtractor(N=100)
    _, images = video_loader.extract({'id': None, 'video': video_path})

    transform = Compose([
        Resize(224, interpolation=BICUBIC),
        CenterCrop(224),
        Normalize((0.48145466, 0.4578275, 0.40821073),
                  (0.26862954, 0.26130258, 0.27577711)),
    ])

    images = transform(images / 255.0)
    images = images.to(torch.float16)
    with torch.no_grad():
        features = clip_model.encode_image(images.to('cuda'))
    return features


class VTimeLLM(ModelWrapper):
    """Wrapper for PandaGPT: https://github.com/huangb23/VTimeLLM."""

    def __init__(self, config_file: str):
        """
        Args:
            config_file (str): Path to the configuration file.
        """
        super(VTimeLLM, self).__init__(config_file)
        print('Loading VTimeLLM ...')

        sys.path.insert(0, self.config.repository_path)
        from vtimellm.model.builder import load_pretrained_model
        from vtimellm.utils import disable_torch_init

        disable_torch_init()
        self.tokenizer, self.model, self.context_len = load_pretrained_model(
            self.config, self.config.stage2, self.config.stage3)
        self.model = self.model.cuda()
        self.model = self.model.to(torch.float16)
        self.clip_model, _ = clip.load(self.config.clip_path)
        self.clip_model.eval()
        self.clip_model = self.clip_model.cuda()
        self.reset()

    def load_config(self, config_file: str):
        """Loads parameters from configuration file.

        Args:
            config_file (str): Path to the configuration file.
        """
        with open(config_file, 'r') as file:
            config = yaml.load(file, Loader=yaml.FullLoader)

        config = argparse.Namespace(**config)

        return config

    def reset(self):
        """Reset the history of the conversation with the model."""
        from vtimellm.conversation import Conversation, SeparatorStyle

        self.conv = Conversation(
            system=("A chat between a curious user and an artificial "
                    "intelligence assistant. The assistant gives helpful and "
                    "polite answers to the user's questions."),
            roles=('USER', 'ASSISTANT'),
            version='v1',
            messages=[],
            offset=0,
            sep_style=SeparatorStyle.TWO,
            sep=' ',
            sep2='</s>',
        )
        self.roles = self.conv.roles
        self.images = []

    def submit(self, prompt: str, video: str = None, verbose: bool = True):
        """Submit a prompt with a video to the model.

        Args:
            prompt (str): Prompt string.
            video (str, optional): Path to the video file. Defaults to None.
            verbose (bool, optional): Whether to print to the prompt and
                answers. Defaults to True.
        """
        from vtimellm.constants import DEFAULT_IMAGE_TOKEN, IMAGE_TOKEN_INDEX
        from vtimellm.conversation import SeparatorStyle
        from vtimellm.mm_utils import (KeywordsStoppingCriteria,
                                       tokenizer_image_token)

        if video is not None:
            self.images.append(load_video(video, self.clip_model))
            if verbose:
                print(green(f'Prompt: ({video})'), prompt)
            prompt = DEFAULT_IMAGE_TOKEN + '\n' + prompt
        else:
            if verbose:
                print(green('Prompt:'), prompt)

        self.conv.append_message(self.conv.roles[0], prompt)
        self.conv.append_message(self.conv.roles[1], None)

        input_ids = tokenizer_image_token(
            self.conv.get_prompt(),
            self.tokenizer,
            IMAGE_TOKEN_INDEX,
            return_tensors='pt').unsqueeze(0).cuda()

        stop_str = None
        if self.conv.sep_style != SeparatorStyle.TWO:
            stop_str = self.conv.sep
        else:
            stop_str = self.conv.sep2

        keywords = [stop_str]
        stopping_criteria = KeywordsStoppingCriteria(keywords, self.tokenizer,
                                                     input_ids)

        with torch.inference_mode():
            output_ids = self.model.generate(
                input_ids,
                images=torch.stack(self.images).cuda(),
                do_sample=True,
                temperature=self.config.temperature,
                max_new_tokens=1024,
                streamer=None,
                use_cache=True,
                stopping_criteria=[stopping_criteria])
        outputs = self.tokenizer.decode(
            output_ids[0, input_ids.shape[1]:]).strip()

        self.conv.messages[-1][-1] = outputs
        response = outputs.rstrip('</s>')
        if verbose:
            print(blue('VTimeLLM:'), response)

        return response
