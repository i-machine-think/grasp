import argparse
import os
import string

import pandas as pd
import yaml
from tabulate import tabulate

OPEN_CHOICES = {
    "Direction": {
        "forward": ["forward", "forwards", "up", "upward", "upwards"],
        "backward": ["backward", "backwards", "down", "downward", "downwards"],
        "left": ["left"],  # "right to left" etc. are hardcoded
        "right": ["right"],
    },
    "Shape": {
        "ball": ["ball", "sphere", "round"],
        "cube": ["cube", "box", "rectangular", "square"],
    },
    "Color": {
        "red": ["red"],
        "green": ["green"],
        "blue": ["blue"],
        "black": ["black"],
    },
    "RelationalPosition": {
        "left": ["left"],
        "right": ["right"],
    },
}


def intersects(a, b):
    for x in a:
        if x in b:
            return True
    return False


def count_open(scene: str, df: pd.DataFrame):
    """Parses the responses by the models for strings that are valid responses
    for each scene.

    Args:
        scene (str): The name of the current scene, so we can get the possible
            answers from OPEN_CHOICES above.
        df (pd.DataFrame): Dataframe containing responses and ground truths.

    Returns:
        tuple[int, int]: Number of correct answers, number of wrong
            answers.
    """
    n_correct = 0
    n_false = 0
    for _, row in df.iterrows():
        true_label = row['gt']
        response = str(row['response']).lower()
        words = [w for w in response.split()]
        words = [
            w.translate(str.maketrans('', '', string.punctuation))
            for w in words
        ]
        contains_true_label = intersects(words,
                                         OPEN_CHOICES[scene][true_label])
        contains_wrong_label = False
        for label, variants in OPEN_CHOICES[scene].items():
            if label == true_label:
                continue
            # Special case for Direction
            if scene == "Direction":
                if label == "right" and true_label == "left":
                    if "right to left" in response or \
                            "right to the left" in response:
                        continue
                elif label == "left" and true_label == "right":
                    if "left to right" in response or \
                            "left to the right" in response:
                        continue
            if intersects(words, variants):
                contains_wrong_label = True
                break
        if contains_wrong_label and not contains_true_label:
            n_false += 1
        elif contains_true_label and not contains_wrong_label:
            n_correct += 1
    return n_correct, n_false


def accuracy_open(df: pd.DataFrame, out_file: str):
    """Calculates accuracies for open-ended tests (multi-class classification).

    Args:
        df (pd.DataFrame): Dataframe containing responses and ground truths.
        out_file (str):  Where the results should be stored.
    """
    # if there was a back-and-forth with the model,
    # we are only interested in the final response
    df['response'] = df['response'].apply(lambda x: x.split('<SEP>')[-1])

    # Group by scenario
    scenes = df.groupby('scene')
    scenes = sorted(scenes)

    results = []
    for scene, df in scenes:
        # Split by true label
        dfs = df.groupby("gt")
        combined_correct = 0
        combined_false = 0

        # Get number of correct responses
        for true_label, sdf in dfs:
            correct, false = count_open(scene, sdf)
            combined_correct += correct
            combined_false += false

        # Print
        for true_label, sdf in dfs:
            correct, false = count_open(scene, sdf)
            invalid = len(sdf) - correct - false

            results.append([
                scene, true_label, correct, false, invalid,
                f'{(correct/len(sdf)*100):.1f}%',
                f'{(combined_correct/len(sdf)*100):.1f}%'
            ])

    results_df = pd.DataFrame(results,
                              columns=[
                                  'Scene', 'Label', 'Correct', 'False', '?',
                                  'Acc', 'Acc (comb)'
                              ])

    table = tabulate(results_df, headers='keys', tablefmt='fancy_grid')
    with open(out_file, 'w') as file:
        file.write(table)

    print(table)


def count(df: pd.DataFrame):
    """Parses the responses by the models and counts yes/no answers.

    Args:
        df (pd.DataFrame): Dataframe containing responses and ground truths.

    Returns:
        tuple[int, int, int, int]: Number of "yes" responses, Number of "no"
            responses, invalid responses, correct responses.
    """
    # Extract responses
    responses = list(df['response'])

    # Get first word as lowercase
    responses = [str(r).split()[0].lower() for r in responses]

    # Remove punctuation
    responses = [
        r.translate(str.maketrans('', '', string.punctuation))
        for r in responses
    ]

    # Count yes/no's
    n_yes = responses.count('yes')
    n_no = responses.count('no')

    # Responses that don't begin with yes or no are invalid
    n_invalid = len(responses) - n_yes - n_no

    # Count correct responses by comparing to true label
    tl = list(df['gt'])
    n_correct = 0
    for (response, label) in zip(responses, tl):
        if response == label:
            n_correct += 1

    # Return counts
    return n_yes, n_no, n_invalid, n_correct


def accuracy_binary(df: pd.DataFrame, out_file: str):
    """Calculates accuracies for binary classification tests.

    Args:
        df (pd.DataFrame): Dataframe containing the outputs and ground truths.
        out_file (str): Where the results should be stored.
    """
    # if there was a back-and-forth with the model,
    # we are only interested in the final response
    df['response'] = df['response'].apply(lambda x: x.split('<SEP>')[-1])
    if 'experiment' in df:
        experiments = df.groupby('experiment')
    else:
        experiments = df.groupby('scene')
    experiments = sorted(experiments)

    results = []
    for experiment, df in experiments:
        # Split by true label
        p_df = df.loc[df['gt'] == 'yes']
        ip_df = df.loc[df['gt'] == 'no']

        # Get number of yes/no and correct responses
        p_y, p_n, p_i, p_c = count(p_df)
        ip_y, ip_n, ip_i, ip_c = count(ip_df)

        # Sanity check
        assert p_c == p_y
        assert ip_c == ip_n

        # Compute accuracy
        p_acc = p_y / len(p_df) if len(p_df) > 0 else 0
        ip_acc = ip_n / len(ip_df) if len(ip_df) > 0 else 0
        combined_acc = (p_y + ip_n) / (len(p_df) + len(ip_df))

        # Print
        results.append([
            f'{experiment} (pos.)', p_y, p_n, p_i, f'{(p_acc*100):.1f}%',
            f'{(combined_acc*100):.1f}%'
        ])
        results.append([
            f'{experiment} (neg.)', ip_y, ip_n, ip_i, f'{(ip_acc*100):.1f}%',
            f'{(combined_acc*100):.1f}%'
        ])

    results_df = pd.DataFrame(
        results, columns=['Scene', 'Yes', 'No', '?', 'Acc', 'Acc (comb)'])

    table = tabulate(results_df, headers='keys', tablefmt='fancy_grid')
    with open(out_file, 'w') as file:
        file.write(table)

    print(table)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description='Compute accuracies for results')
    parser.add_argument('results_dir',
                        type=str,
                        help='Directory containing the results')
    args = parser.parse_args()

    with open(os.path.join(args.results_dir, 'test_config.yaml'), 'r') as file:
        test_config = yaml.load(file, Loader=yaml.FullLoader)

    test_name = test_config['test_name']
    df = pd.read_csv(os.path.join(args.results_dir, 'results.csv'))
    out_file = os.path.join(args.results_dir, 'accuracy.txt')

    if 'level2' in test_name:
        df['experiment'] = df['scene'].apply(lambda x: x.split('_')[1])

    if test_name == 'level1-open':
        accuracy_open(df, out_file)
    else:
        accuracy_binary(df, out_file)
