# GRASP: A novel benchmark for evaluating language GRounding And Situated Physics understanding in multimodal language models

## Quickstart
1. Create conda environment:
`conda create --name grasp python=3.9`
2. Install pytorch (adjust CUDA version if necessary):
`conda install pytorch torchvision torchaudio pytorch-cuda=12.1 -c pytorch -c nvidia`
3. Install GRASP:
`pip3 install -e .`

## Benchmark Data
The Unity source code and builds will be published shortly, such that the benchmark can be extended and new videos generated. The videos that we used in GRASP can be downloaded [here](https://drive.google.com/drive/folders/1F_9R1zLtAMQ7N_IIIio6HjEBkGuuMX4M?usp=drive_link). Download and unzip `videos.zip` in the `data` directory.