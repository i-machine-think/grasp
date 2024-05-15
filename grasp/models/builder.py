from grasp.models import (PandaGPT, VideoChatGPT, VideoLLaMA, VideoLLaMA2,
                          VTimeLLM)


def build_model(model_name, config):
    if model_name == 'pandagpt':
        model = PandaGPT(config)
    elif model_name == 'videollama':
        model = VideoLLaMA(config)
    elif model_name == 'videollama2':
        model = VideoLLaMA2(config)
    elif model_name == 'videochatgpt':
        model = VideoChatGPT(config)
    elif model_name == 'vtimellm':
        model = VTimeLLM(config)
    else:
        raise RuntimeError(f'Model {model_name} not supported')

    return model
