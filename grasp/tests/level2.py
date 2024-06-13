import json

import pandas as pd

from .test import Test
from .test_instance import TestInstance


class Level2(Test):

    def build_test(self):
        with open(self.prompt_file) as f:
            scene_prompts = json.load(f)

        df = pd.read_csv(self.data_csv)
        for _, row in df.iterrows():
            self.test_instances.append(
                TestInstance(
                    row['scene'], [scene_prompts[row['scene'].split('_')[1]]],
                    [row['video_file']],
                    'yes' if row['scene'].split('_')[0] == 'P' else 'no'))
