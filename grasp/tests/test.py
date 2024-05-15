import csv
from abc import ABC, abstractmethod


class Test(ABC):

    def __init__(self, prompt_file, data_csv, mode):
        self.prompt_file = prompt_file
        self.data_csv = data_csv
        self.mode = mode
        self.test_instances = []
        self.build_test()

    @abstractmethod
    def build_test(self):
        pass

    def write(self, file):
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
