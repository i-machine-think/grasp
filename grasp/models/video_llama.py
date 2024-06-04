import argparse
import os
import sys

import yaml

from grasp.models.wrapper import ModelWrapper
from grasp.utils import blue, green, red


class VideoLLaMA(ModelWrapper):
    """Wrapper for VideoLLaMA: https://github.com/DAMO-NLP-SG/Video-LLaMA."""

    def __init__(self, config_file: str):
        """
        Args:
            config_file (str): Path to the configuration file.
        """
        super(VideoLLaMA, self).__init__(config_file)
        print('Loading Video-LLaMA ...')

        from video_llama.common.registry import registry  # type: ignore
        from video_llama.conversation.conversation_video import Chat

        gpu_id = self.config.gpu_id
        model_config = self.config.model_cfg
        model_config.device_8bit = gpu_id
        model_cls = registry.get_model_class(model_config.arch)
        model = model_cls.from_config(model_config).to(f'cuda:{gpu_id}')
        vis_processor_cfg = self.config.datasets_cfg.webvid.vis_processor.train
        vis_processor = registry.get_processor_class(
            vis_processor_cfg.name).from_config(vis_processor_cfg)
        self.chat = Chat(model, vis_processor, device=f'cuda:{gpu_id}')
        self.reset()

    def load_config(self, config_file: str):
        """
        Args:
            config_file (str): Path to the configuration file.
        """
        with open(config_file, "r") as file:
            config = yaml.load(file, Loader=yaml.FullLoader)

        sys.path.insert(0, os.path.join(config['repository_path']))
        from video_llama.common.config import Config  # type: ignore

        args = argparse.Namespace(**dict(options=[], cfg_path=config_file))
        cfg = Config(args)
        cfg.gpu_id = config['gpu_id']
        return cfg

    def reset(self):
        """Reset the history of the conversation with the model."""
        from video_llama.conversation.conversation_video import \
            default_conversation
        self.chat_state = default_conversation.copy()

        self.chat_state.system = (
            'You will be able to see a video once I '
            'provide it to you. Please answer my questions.')
        self.img_list = []

    def submit(self, prompt: str, video: str = None, verbose: bool = True):
        """Submit a prompt with a video to the model.

        Args:
            prompt (str): Prompt string.
            video (str, optional): Path to the video file. Defaults to None.
        """
        if video is not None:
            self.chat.upload_video_without_audio(video, self.chat_state,
                                                 self.img_list)
            if verbose:
                print(green(f'Prompt: ({video})'), prompt)
        else:
            if verbose:
                print(green('Prompt:'), prompt)

        self.chat.ask(prompt, self.chat_state)
        response = self.chat.answer(conv=self.chat_state,
                                    img_list=self.img_list,
                                    num_beams=1,
                                    temperature=1.0,
                                    max_new_tokens=300,
                                    max_length=2000)[0]

        if verbose:
            print(blue('Video_LLaMA:'), response)

        return response
