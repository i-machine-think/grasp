import argparse
import os

import cv2
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.envs.unity_gym_env import UnityToGymWrapper
from mlagents_envs.exception import UnityTimeOutException


def load_env(env_path: str, seed: int):
    """Load the environment from the Unity build.

    Args:
        env_path (str): Path to the environment files.
        seed (int): Seed for randomization.

    Returns:
        UnityToGymWrapper: The generated environment.
    """
    fname = os.path.join(env_path, "Simulator")
    try:
        unity_env = UnityEnvironment(file_name=fname,
                                     seed=seed,
                                     side_channels=[])
        env = UnityToGymWrapper(unity_env,
                                uint8_visual=True,
                                allow_multiple_obs=True)
    except UnityTimeOutException:
        print("Failed to create Unity env! "
              "Make sure you're using xvfb-run.")
        raise
    return env


def frames2video(images: list, output_file: str, fps: int = 50):
    """Creates a video from all the frames.

    Args:
        images (list): A list of frames.
        output_file (str): Where to store the video.
        fps (int, optional): At how many FPS to generate video. Defaults to 50.
    """
    height, width, _ = images[0].shape
    fourcc = cv2.VideoWriter_fourcc(*'mp4v')
    video_writer = cv2.VideoWriter(output_file, fourcc, fps, (width, height))
    for image in images:
        video_writer.write(cv2.cvtColor(image, cv2.COLOR_BGR2RGB))
    video_writer.release()


def create_videos(env_paths: list,
                  out: str,
                  txt_file: str = None,
                  n: int = 128):
    """Creates <n> videos for all environemtns in <env_paths>.

    Args:
        env_paths (list): List of directories containing environments.
        out (str): Output directory.
        txt_file (str, optional): Path to text files that contains labels in
            case the test requires them. Defaults to None.
        n (int, optional): Number of videos to collect. Defaults to 128.
    """
    for env_path in env_paths:
        for i in range(n):
            env = load_env(env_path, i)
            imgs = []
            action = [0.0, 0.0]
            dname = os.path.join(out, env_path.split("/")[-2].lower(),
                                 env_path.split("/")[-1])
            os.makedirs(dname, exist_ok=True)

            try:
                obs, _ = env.reset()
                if txt_file is not None:
                    with open(txt_file, 'r') as f:
                        content = f.read()
                        with open(os.path.join(dname, f"{i:03}.txt"),
                                  'w') as f2:
                            f2.write(content)
                imgs.append(obs)
                done = False
                while not done:
                    (obs, _), _, done, _ = env.step(action)
                    imgs.append(obs)
            finally:
                env.close()

            fname = f"{i:03}.mp4"
            path = os.path.join(dname, fname)
            # 50 fps because physics timestep is set to 0.02 in Unity project
            frames2video(imgs, path, fps=50)

            print("Created", path)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(
        description='Generate videos from environments')
    parser.add_argument('--dir',
                        type=str,
                        help='Directory that contains the environment builds')
    parser.add_argument('--scenes', type=str, nargs='+', help='Scene names')
    parser.add_argument(
        '--txt-file',
        type=str,
        help='Text file from which to parse additional information')
    parser.add_argument('--N',
                        type=int,
                        default=128,
                        help='Number of videos to collect')
    parser.add_argument('--out',
                        default='data/videos',
                        type=str,
                        help='Where results are dumped')
    args = parser.parse_args()

    assert (args.dir is None) ^ (
        args.scenes is None
    ), 'Specify either a directory containing scenes or list individual scenes'

    if args.dir is None:
        env_paths = args.scenes
    else:
        env_paths = [os.path.join(args.dir, f) for f in os.listdir(args.dir)]

    envs_without_obj_ordering = []
    if args.txt_file is None:
        for path in env_paths:
            if 'ordering' not in path.lower():
                envs_without_obj_ordering.append(path)
        env_paths = envs_without_obj_ordering

    if args.txt_file is not None:
        assert len(env_paths) == 1, \
            'When txt-file is specified, only a single scene can be specified'

    create_videos(env_paths, args.out, args.txt_file, args.N)
