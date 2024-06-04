class TestInstance:
    """Data wrapper for a single instance of a test.
    """

    def __init__(self,
                 scene: str = None,
                 prompt: list[str] = [],
                 video_file: list[str] = [],
                 gt: str = None,
                 response: list[str] = []):
        """
        Args:
            scene (str, optional): Name of the scene. Defaults to None.
            prompt (list[str], optional): List of prompts. The model will be
                prompted with these sequentially. Defaults to [].
            video_file (list[str], optional): List of video files to supply
                with prompts. There should be a video file for each prompt. In
                case no video should be supplied with the prompt, use None as
                a placeholder. Defaults to [].
            gt (str, optional): Ground truth label. Defaults to None.
            response (list[str], optional): List of responses by the model
                after each prompt. Defaults to [].
        """
        self.scene = scene
        self.prompt = prompt
        self.video_file = video_file
        self.gt = gt
        self.response = response
