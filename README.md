# GRASP: A novel benchmark for evaluating language GRounding And Situated Physics understanding in multimodal language models

## Setup
1. Create conda environment:
`conda create --name grasp python=3.9`
2. Install PyTorch (adjust CUDA version if necessary):
`conda install pytorch torchvision torchaudio pytorch-cuda=12.1 -c pytorch -c nvidia`
3. Install GRASP:
`pip3 install -e .`

## Evaluating models
We currently provide some configurations (in `configs/models`) and [setup instructions](https://github.com/i-machine-think/grasp/wiki/Video%E2%80%90LLM-Setup-Instructions) for the following Video-LLMs:
* [PandaGPT](https://arxiv.org/abs/2305.16355)
* [VTimeLLM](https://arxiv.org/abs/2311.18445)
* [Video-ChatGPT](https://arxiv.org/abs/2306.05424)
* [Video-LLaMA](https://arxiv.org/abs/2306.02858)
* [Video-LLaMA 2](https://arxiv.org/abs/2306.02858)

Further, we provide configurations for different tests in `configs/tests`:
* `level1_binary.yaml`: Binary Level 1 with default prompting.
* `level1_binary_cot.yaml`: Binary Level 1 with chain-of-thought prompting.
* `level1_binary_oneshot.yaml`: Binary Level 1 with one-shot prompting.
* `level1_open.yaml`: Level 1 with open questions.
* `level2.yaml`: Level 2 with default prompting.
* `level2_text.yaml`: Level 2 with scene descriptions instead of videos.

Models can be evaluated on our benchmark:
1. Our benchmark videos can be downloaded from [this](https://drive.google.com/drive/folders/1F_9R1zLtAMQ7N_IIIio6HjEBkGuuMX4M) Google Drive. Specifically, download `videos.zip` and unzip it in the `data` directory.
2. Create necessary CSV files for the benchmark:
```bash
python3 tools/prepare_data.py
```
3. Evaluate a model on a specific test:
```bash
python3 tools/run_test.py <model_config> <test_config>
```
This will create a folder in `results` and dump the outputs into a CSV file.

4. Compute the accuracy of previously collected outputs:
```bash
python3 tools/compute_accuracy.py <results_dir>
```
This will print a table with results to the terminal and will also write a table to a text file in the results directory.

## Collecting additional videos
We provide access to the Unity scene builds in the aforementioned [Google Drive](https://drive.google.com/drive/folders/1F_9R1zLtAMQ7N_IIIio6HjEBkGuuMX4M). These are compiled Unity scenes that we used to generate the benchmark videos. Videos can be generated as follows:
1. Download and unzip `build.zip` from the Google Drive.
2. The video generation requires the installation of Unity's ML-Agents Python package:
```bash
git clone --branch release_21 https://github.com/Unity-Technologies/ml-agents.git
cd ml-agents
python -m pip install ./ml-agents-envs
```
3. We recommend using `xvfb-run` to generate the videos so that the simulation does not actually need to be rendered on screen. Install `xvfb-run` with:
```
sudo apt install `xvfb-run`
```
4. Generate new videos:
```bash
xvfb-run python3 tools/generate_videos.py --dir <builds_dir> --scenes <scene1,scene2,...,sceneN> --txt-file <labels.txt> --N <number of videos> --out <output_dir>
```
The scenes for which videos should be collected can be either implicitly specified with `--dir`, in which case videos are collected for all scenes contained in that directory, or explicitly listed with `--scenes`.
The `--txt-file` flag specifies from which file the labels should be read during video annotation: Some benchmark tests require some text labels for evaluation. Currently, these are only necessary for the object ordering tests and the files are `2obj_ordering.txt`,  `3obj_ordering.txt`, and  `4obj_ordering.txt`. Whenever `--txt-file ` is used, it is only possible to specify a single scene!

The benchmark videos were collected using:
```bash
xvfb-run python3 tools/generate_videos.py --dir build/Level2 # By default 128 videos are generated and saved to data/videos
xvfb-run python3 tools/generate_videos.py --dir build/Level1
xvfb-run python3 tools/generate_videos.py --scenes build/Level1/TwoObjectOrdering --txt-file 2obj_ordering.txt
xvfb-run python3 tools/generate_videos.py --scenes build/Level1/ThreeObjectOrdering --txt-file 3obj_ordering.txt
xvfb-run python3 tools/generate_videos.py --scenes build/Level1/FourObjectOrdering --txt-file 4obj_ordering.txt
```


## Creating additional scenes
Our entire Unity source code can be found in the [GRASP](https://github.com/i-machine-think/grasp/tree/main/GRASP) directory. This contains the scenes and scripts for all tests in Levels 1 and 2 of GRASP. We also provide some [instructions](https://github.com/i-machine-think/grasp/wiki/Add-Unity-Scenes) on how to add further tests in Unity. We encourage you to create pull requests with the addition of new tests!
