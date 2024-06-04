import yaml

from grasp.tests import Level1Binary, Level1Open, Level2, Level2Text


def build_test(config_file: str):
    """Build a test instance from a configuration.

    Args:
        config_file (str): Configuration file.

    Raises:
        RuntimeError: Raises an error if the specified test does not exist.

    Returns:
        Test: An instance of the test.
    """
    with open(config_file, 'r') as file:
        config = yaml.load(file, Loader=yaml.FullLoader)

    args = [
        config.get('prompt_file', None),
        config.get('data_csv', None),
        config.get('mode', None)
    ]

    if config['test_name'] == 'level1-binary':
        test = Level1Binary(*args)
    elif config['test_name'] == 'level1-open':
        test = Level1Open(*args)
    elif config['test_name'] == 'level2':
        test = Level2(*args)
    elif config['test_name'] == 'level2-text':
        test = Level2Text(*args)
    else:
        raise RuntimeError(f'test {config["test_name"]} not supported')

    return test
