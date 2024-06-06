import os
import sys

import torch
import yaml

from grasp.models.wrapper import ModelWrapper
from grasp.utils import blue, green


class PandaGPT(ModelWrapper):
    """Wrapper for PandaGPT: https://github.com/yxuansu/PandaGPT."""

    def __init__(self, config_file: str):
        """
        Args:
            config_file (str): Path to the configuration file.
        """
        super(PandaGPT, self).__init__(config_file)
        print('Loading PandaGPT ...')

        sys.path.insert(0, os.path.join(self.config['repository_path'],
                                        'code'))
        from model.openllama import OpenLLAMAPEFTModel

        self.model = OpenLLAMAPEFTModel(**self.config)
        delta_ckpt = torch.load(self.config['delta_ckpt_path'],
                                map_location=torch.device('cpu'))
        self.model.load_state_dict(delta_ckpt, strict=False)
        self.model = self.model.eval().half().cuda()
        self.reset()

    def load_config(self, config_file: str):
        """Loads parameters from configuration file.

        Args:
            config_file (str): Path to the configuration file.
        """
        with open(config_file, 'r') as file:
            config = yaml.load(file, Loader=yaml.FullLoader)

        return config

    def reset(self):
        """Reset the history of the conversation with the model."""
        self.history = []
        self.modality_cache = []
        self.video_paths = []

    def submit(self, prompt: str, video: str = None, verbose: bool = True):
        """Submit a prompt with a video to the model.

        Args:
            prompt (str): Prompt string.
            video (str, optional): Path to the video file. Defaults to None.
            verbose (bool, optional): Whether to print to the prompt and
                answers. Defaults to True.
        """
        if verbose:
            print(green(f'Prompt: ({video})'), prompt)

        prompt_text = ''
        for idx, (q, a) in enumerate(self.history):
            if idx == 0:
                prompt_text += f'{q}\n### Assistant: {a}\n###'
            else:
                prompt_text += f' Human: {q}\n### Assistant: {a}\n###'

        if len(self.history) == 0:
            prompt_text += f'{prompt}'
        else:
            prompt_text += f' Human: {prompt}'

        if video is not None:
            self.video_paths.append(video)

        response = self.model.generate({
            'prompt':
            prompt_text,
            'image_paths': [],
            'audio_paths': [],
            'video_paths':
            self.video_paths,
            'thermal_paths': [],
            'top_p':
            self.config['top_p'],
            'temperature':
            self.config['temperature'],
            'max_tgt_len':
            self.config['max_length'],
            'modality_embeds':
            self.modality_cache
        })
        self.history.append((prompt, response))
        if verbose:
            print(blue('PandaGPT:'), response)

        return response
