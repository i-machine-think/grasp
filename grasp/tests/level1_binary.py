import json
import random

import pandas as pd

from .test import Test
from .test_instance import TestInstance

ONESHOT_PROMPT = "This is an example of a question about this video and the correct answer.\nQuestion: {}\nAnswer: {}\nNext, I want you to answer my next question in the same way with regards to the next video."


class Level1Binary(Test):

    def build_test(self):
        with open(self.prompt_file) as f:
            scene_prompts = json.load(f)

        df = pd.read_csv(self.data_csv)
        for idx, row in df.iterrows():
            if self.mode == 'standard':
                self.test_instances.append(
                    TestInstance(row['scene'], [
                        scene_prompts[row['scene']]['binary'].format(
                            *row['pos_observation'].split(','))
                    ], [row['video_file']], 'yes'))
                self.test_instances.append(
                    TestInstance(row['scene'], [
                        scene_prompts[row['scene']]['binary'].format(
                            *row['neg_observation'].split(','))
                    ], [row['video_file']], 'no'))
            elif self.mode == 'cot':
                if 'open' not in scene_prompts[row['scene']]:
                    initial_prompt = 'What can you see in this video?'
                else:
                    initial_prompt = scene_prompts[row['scene']]['open']

                self.test_instances.append(
                    TestInstance(row['scene'], [
                        initial_prompt,
                        scene_prompts[row['scene']]['binary'].format(
                            *row['pos_observation'].split(','))
                    ], [row['video_file'], None], 'yes'))
                self.test_instances.append(
                    TestInstance(row['scene'], [
                        initial_prompt,
                        scene_prompts[row['scene']]['binary'].format(
                            *row['neg_observation'].split(','))
                    ], [row['video_file'], None], 'no'))
            elif self.mode == 'oneshot':
                examples = df[(df['scene'] == row['scene'])
                              & (df.index != idx)]

                example = examples.sample(n=1)
                if random.random() < 0.5:
                    initial_prompt = ONESHOT_PROMPT.format(
                        scene_prompts[row['scene']]['binary'].format(
                            *example['pos_observation'].iloc[0].split(',')),
                        'yes')
                else:
                    initial_prompt = ONESHOT_PROMPT.format(
                        scene_prompts[row['scene']]['binary'].format(
                            *example['neg_observation'].iloc[0].split(',')),
                        'no')
                self.test_instances.append(
                    TestInstance(row['scene'], [
                        initial_prompt,
                        scene_prompts[row['scene']]['binary'].format(
                            *row['pos_observation'].split(','))
                    ], [example['video_file'].iloc[0], row['video_file']],
                                 'yes'))

                example = examples.sample(n=1)
                if random.random() < 0.5:
                    initial_prompt = ONESHOT_PROMPT.format(
                        scene_prompts[row['scene']]['binary'].format(
                            *example['pos_observation'].iloc[0].split(',')),
                        'yes')
                else:
                    initial_prompt = ONESHOT_PROMPT.format(
                        scene_prompts[row['scene']]['binary'].format(
                            *example['neg_observation'].iloc[0].split(',')),
                        'no')
                self.test_instances.append(
                    TestInstance(row['scene'], [
                        initial_prompt,
                        scene_prompts[row['scene']]['binary'].format(
                            *row['neg_observation'].split(','))
                    ], [example['video_file'].iloc[0], row['video_file']],
                                 'no'))
