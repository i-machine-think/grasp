import argparse
import os
import shutil
from datetime import datetime

from tqdm import tqdm

from grasp.models.builder import build_model
from grasp.models.wrapper import ModelWrapper
from grasp.tests.builder import build_test
from grasp.tests.test import Test


def run(model: ModelWrapper, test: Test):
    """Collects responses from a model on a specific test.

    Args:
        model (ModelWrapper): The model to be evaluated.
        test (Test): The test on which the model is evaluated.
    """
    for test_instance in tqdm(test.test_instances):
        model.reset()
        responses = []
        for prompt, video_file in zip(test_instance.prompt,
                                      test_instance.video_file):
            responses.append(model.submit(prompt, video_file, False))

        test_instance.response = responses


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Evaluate a model')
    parser.add_argument('model_config_file',
                        type=str,
                        help='Path to the config file')
    parser.add_argument('test_config_file', type=str, help='Name of the test')
    args = parser.parse_args()

    # load the model
    model_name = args.model_config_file.split('/')[-2]
    model = build_model(model_name, args.model_config_file)

    # setup output
    dt = datetime.now()
    dt = dt.strftime("%Y%m%d_%H%M%S")
    output_dir = (f'{dt}_{model_name}_'
                  f'{args.test_config_file.split("/")[-1].split(".")[0]}')
    os.makedirs(os.path.join('./results', output_dir), exist_ok=True)

    results_file = os.path.join('./results', output_dir, 'results.csv')
    shutil.copy(
        args.model_config_file,
        os.path.join(
            './results', output_dir, f'{model_name}_'
            f'{args.model_config_file.split("/")[-1]}'))
    shutil.copy(args.test_config_file,
                os.path.join('./results', output_dir, 'test_config.yaml'))

    # load the test
    test = build_test(args.test_config_file)

    run(model, test)
    test.write(results_file)
