import json

import pandas as pd

from .test import Test
from .test_instance import TestInstance


class Level1Open(Test):

    def build_test(self):
        with open(self.prompt_file) as f:
            scene_prompts = json.load(f)

        df = pd.read_csv(self.data_csv)
        for _, row in df.iterrows():
            if 'open' not in scene_prompts[row['scene']]:
                continue

            self.test_instances.append(
                TestInstance(row['scene'],
                             [scene_prompts[row['scene']]['open']],
                             [row['video_file']], row['pos_observation']))
