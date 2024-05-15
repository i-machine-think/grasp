import sys

from grasp.models.wrapper import ModelWrapper
from grasp.utils import blue, green, red


class VideoChatGPT(ModelWrapper):
    """Wrapper for VideoChatGpt: https://github.com/mbzuai-oryx/Video-ChatGPT.
    """

    def __init__(self, config_file: str):
        """
        Args:
            config_file (str): Path to the configuration file.
        """
        super(VideoChatGPT, self).__init__(config_file)
        print('Loading Video-ChatGPT...')

        from video_chatgpt.constants import (DEFAULT_VID_END_TOKEN,
                                             DEFAULT_VID_START_TOKEN,
                                             DEFAULT_VIDEO_PATCH_TOKEN)
        from video_chatgpt.demo.chat import Chat  # type: ignore
        from video_chatgpt.eval.model_utils import initialize_model
        from video_chatgpt.utils import disable_torch_init

        sys.path.insert(0, self.config['repository_path'])

        disable_torch_init()
        model, vision_tower, tokenizer, image_processor, \
            video_token_len = initialize_model(
                self.config.model_name, self.config.projection_path)
        replace_token = DEFAULT_VIDEO_PATCH_TOKEN * video_token_len
        replace_token = (DEFAULT_VID_START_TOKEN + replace_token +
                         DEFAULT_VID_END_TOKEN)
        self.chat = Chat('Video-ChatGPT', 'conv_simulator', tokenizer,
                         image_processor, vision_tower, model, replace_token)
        self.reset()

    def load_cfg(self, config_file: str):
        """Loads parameters from configuration file.

        Args:
            config_file (str): Path to the configuration file.
        """
        pass

    def reset(self):
        """Reset the history of the conversation with the model."""
        from video_chatgpt.video_conversation import conv_templates
        self.state = conv_templates['simulator'].copy()
        self.img_list = []
        system_msg = self.state.system
        print(red('System message:'), system_msg)

    def submit(self, prompt: str, video: str = None):
        """Submit a prompt with a video to the model.

        Args:
            prompt (str): Prompt string.
            video (str, optional): Path to the video file. Defaults to None.
        """
        prompt = prompt[:1536]  # Hard cut-off
        if video is not None:
            print(green(f'Prompt: ({video})'), prompt)
            prompt = prompt[:1200]  # Hard cut-off for videos
            if '<video>' not in prompt:
                prompt = prompt + '\n<video>'
            prompt = (prompt, video)
            self.chat.upload_video(video, self.img_list)
        else:
            print(green('Prompt:'), prompt)
        self.state.append_message(self.state.roles[0], prompt)
        self.state.append_message(self.state.roles[1], None)
        self.state.skip_next = False
        # Chat.answer returns a generator for the UI
        gen = self.chat.answer(self.state, self.img_list, self.cfg.temperature,
                               self.cfg.max_output_tokens, False)
        list(gen)  # Run generator
        response = self.state.messages[-1][-1]
        print(blue('Video-ChatGPT:'), response)
        return response
