import json

from .test import Test
from .test_instance import TestInstance


class Level2Text(Test):

    def build_test(self):
        with open(self.prompt_file) as f:
            scene_prompts = json.load(f)

        for scene, prompt in scene_prompts.items():
            self.test_instances.append(
                TestInstance(scene, [prompt], [None],
                             'yes' if scene.split('_')[0] == 'P' else 'no'))
