import argparse
import csv
import os
import random


def prepare_level1_scene(level_dir, scene_name, mapping=None):
    scenes = [scene for scene in os.listdir(level_dir) if scene_name in scene]
    possible_observations = [
        scene[len(scene_name):].lower() for scene in scenes
    ]

    data = []
    for scene in scenes:
        video_files = [
            file for file in os.listdir(os.path.join(level_dir, scene))
            if 'mp4' in file
        ]
        video_files = sorted(video_files, key=lambda x: int(x.split('.')[0]))
        for video_file in video_files:
            neg_observations_choices = possible_observations.copy()
            neg_observations_choices.remove(scene[len(scene_name):].lower())
            if mapping is None:
                data.append([
                    scene_name,
                    os.path.join(level_dir, scene, video_file),
                    scene[len(scene_name):].lower(),
                    random.choice(neg_observations_choices)
                ])
            else:
                data.append([
                    scene_name,
                    os.path.join(level_dir, scene, video_file),
                    mapping[scene[len(scene_name):].lower()],
                    mapping[random.choice(neg_observations_choices)]
                ])

    return data


def permute_list(list_):
    original = list_.copy()
    while True:
        random.shuffle(list_)
        if list_ != original:
            break
    return list_


def prepare_level1_object_ordering(level_dir):
    scenes = [
        scene for scene in os.listdir(level_dir) if 'ObjectOrdering' in scene
    ]

    data = []
    for scene in scenes:
        video_files = [
            file for file in os.listdir(os.path.join(level_dir, scene))
            if 'mp4' in file
        ]
        video_files = sorted(video_files, key=lambda x: int(x.split('.')[0]))
        for video_file in video_files:
            with open(
                    os.path.join(level_dir, scene,
                                 f'{video_file.split(".")[0]}.txt'), 'r') as f:
                label = f.readline()[:-1]

                # there was a bug in how the labels were generated so we fix it
                # here for now
                # TODO: fix this in the unity code
                if 'Two' in scene:
                    label = ','.join(label.split(',')[-2:])
                elif 'Three' in scene:
                    label = ','.join(label.split(',')[-3:])
                elif 'Four' in scene:
                    label = ','.join(label.split(',')[-4:])

            data.append([
                scene,
                os.path.join(level_dir, scene, video_file), label,
                ','.join(permute_list(label.split(',')))
            ])

    return data


def prepare_level1(level_dir, out_file):
    data = [['scene', 'video_file', 'pos_observation', 'neg_observation']]
    data += prepare_level1_scene(level_dir, 'Movement', {
        'true': 'moving',
        'false': 'not moving'
    })
    data += prepare_level1_scene(level_dir, 'RelationalPosition')
    data += prepare_level1_scene(level_dir, 'Direction')
    data += prepare_level1_scene(level_dir, 'Shape')
    data += prepare_level1_scene(level_dir, 'Color')
    data += prepare_level1_object_ordering(level_dir)

    with open(out_file, 'w', newline='') as f:
        csv_writer = csv.writer(f)
        for row in data:
            csv_writer.writerow(row)


def prepare_level2(level_dir, out_file):
    data = [['scene', 'video_file']]
    for scene in os.listdir(level_dir):
        video_files = os.listdir(os.path.join(level_dir, scene))
        video_files = sorted(video_files, key=lambda x: int(x.split('.')[0]))
        for video_file in video_files:
            data.append([scene, os.path.join(level_dir, scene, video_file)])

    with open(out_file, 'w', newline='') as f:
        csv_writer = csv.writer(f)
        for row in data:
            csv_writer.writerow(row)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Prepare video data.')
    parser.add_argument('video_dir',
                        type=str,
                        help='Where videos are located.')
    parser.add_argument('out_dir', type=str, help='Where to dump csv files.')
    args = parser.parse_args()

    print('Preparing Level 1 ...')
    prepare_level1(os.path.join(args.video_dir, 'level1'),
                   os.path.join(args.out_dir, 'level1.csv'))

    print('Preparing Level 2 ...')
    prepare_level2(os.path.join(args.video_dir, 'level2'),
                   os.path.join(args.out_dir, 'level2.csv'))
