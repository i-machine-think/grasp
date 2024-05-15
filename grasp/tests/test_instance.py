class TestInstance:

    def __init__(self,
                 scene=None,
                 prompt=[],
                 video_file=[],
                 gt=None,
                 response=[]):
        self.scene = scene
        self.prompt = prompt
        self.video_file = video_file
        self.gt = gt
        self.response = response
