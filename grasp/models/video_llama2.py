from grasp.models import VideoLLaMA
from grasp.utils import red


class VideoLLaMA2(VideoLLaMA):
    """Wrapper for VideoLLaMA2: https://github.com/DAMO-NLP-SG/Video-LLaMA."""

    def reset(self):
        """Reset the history of the conversation with the model."""
        from video_llama.conversation.conversation_video import \
            conv_llava_llama_2
        self.chat_state = conv_llava_llama_2.copy()
        self.img_list = []
        print(red('System message:'), self.chat_state.system)
