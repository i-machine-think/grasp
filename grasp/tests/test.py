import csv
from abc import ABC, abstractmethod


class Test(ABC):
    """Represents a wrapper for a test that contains the logic for building
    the test from the prompts and potentially for different prompting
    strategies.
    """

    def __init__(self, prompt_file: str, data_csv: str, mode: str = None):
        """
        Args:
            prompt_file (str): File that contains the prompts.
            data_csv (str): CSV file that contains paths to videos and
                labels.
            mode (str, optional): If the test allows for different prompting
                strategies, this argument can be used to specify which one
                should be used. Defaults to None.
        """
        self.prompt_file = prompt_file
        self.data_csv = data_csv
        self.mode = mode
        self.test_instances = []
        self.build_test()

    @abstractmethod
    def build_test(self):
        """Implements the logic to build the test, i.e. the prompts and labels.
        """
        pass

    def write(self, file: str):
        """Writes the responses by the models and additional metadata the is
        necessary for evaluation to a CSV file.

        Args:
            file (str): Where to save the responses and metadata.
        """
        data = [['scene', 'prompt', 'video_file', 'gt', 'response']]
        for i in self.test_instances:
            data.append([
                i.scene, '<SEP>'.join(i.prompt),
                ','.join('' if f is None else f for f in i.video_file), i.gt,
                '<SEP>'.join(i.response)
            ])

        with open(file, 'w', newline='') as f:
            csv_writer = csv.writer(f)
            for row in data:
                csv_writer.writerow(row)
