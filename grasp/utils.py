# Helper functions for printing colored text
# Not necessary but makes output much more readable


def bold(text):
    return '\033[1m' + text + '\033[0m'


def red(text):
    return '\033[1m\033[91m' + text + '\033[0m'


def green(text):
    return '\033[1m\033[92m' + text + '\033[0m'


def blue(text):
    return '\033[1m\033[94m' + text + '\033[0m'
