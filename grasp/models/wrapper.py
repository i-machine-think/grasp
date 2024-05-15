from abc import ABC, abstractmethod


class ModelWrapper(ABC):
    """Base wrapper class for Video LLMs to interact with the GRASP benchmark.
    """

    def __init__(self, config_file: str):
        """
        Args:
            config_file (str): Path to the configuration file.
        """
        self.config = self.load_config(config_file)

    @abstractmethod
    def load_config(self, config_file: str):
        """Loads parameters from configuration file.

        Args:
            config_file (str): Path to the configuration file.
        """
        pass

    @abstractmethod
    def reset(self):
        """Reset the history of the conversation with the model."""
        pass

    @abstractmethod
    def submit(self, prompt: str, video: str = None):
        """Submit a prompt with a video to the model.

        Args:
            prompt (str): Prompt string.
            video (str, optional): Path to the video file. Defaults to None.
        """
        pass
