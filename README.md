# GRASP: A novel benchmark for evaluating language GRounding And Situated Physics understanding in multimodal language models

## Setup
1. Create conda environment:
`conda create --name grasp python=3.9`
2. Install pytorch (adjust CUDA version if necessary):
`conda install pytorch torchvision torchaudio pytorch-cuda=12.1 -c pytorch -c nvidia`
3. Install GRASP:
`pip3 install -e .`

## Usage
### Evaluating models
We currently provide some configurations (in `configs/models`) and [setup instructions](https://github.com/i-machine-think/grasp/wiki/Video%E2%80%90LLM-Setup-Instructions) for the following Video-LLMs:
* [PandaGPT](https://arxiv.org/abs/2305.16355)
* [VTimeLLM](https://arxiv.org/abs/2311.18445)
* [Video-ChatGPT](https://arxiv.org/abs/2306.05424)
* [Video-LLaMA](https://arxiv.org/abs/2306.02858)
* [Video-LLaMA 2](https://arxiv.org/abs/2306.02858)

Further, we provide configurations for different tests in `configs/tests`:
* `level1_binary.yaml`: Binary Level 1.
* `level1_binary_cot.yaml`: Binary Level 1 with chain-of-thought prompting.
* `level1_binary_oneshot.yaml`: Binary Level 1 with one-shot prompting.
* `level1_open.yaml`: Level 1 with open questions.
* `level2.yaml`: Level 2 with videos.
* `level2_text.yaml`: Level 2 with scene descriptions (no videos).

Models can be evaluated on our benchmark:
1. Our benchmark videos can be downloaded from [this](https://drive.google.com/drive/folders/1F_9R1zLtAMQ7N_IIIio6HjEBkGuuMX4M) Google Drive. Specifically, download `videos.zip` and unzip it in the `data` directory.
2. Create necessary csv files for the benchmark:
```bash
python3 tools/prepare_data.py
```
3. Evaluate a model on a specific test:
```bash
python3 tools/run_test.py <model_config> <test_config>
```
This will create a folder in `results` and dump the outputs into a csv file.

4. Analyze the accuracy of previously collected outputs:
```bash
python3 tools/analyze_results.py <results_dir>
```
This will print a table with results to the terminal and will also write a table to a text file in the results directory.

### Collecting additional videos
TO COME

### Creating additional scenes
TO COME
