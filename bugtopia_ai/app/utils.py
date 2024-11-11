import requests
from PIL import Image
from io import BytesIO

def load_and_resize_image(image_url: str, target_size=(640, 640)) -> Image.Image:
    response = requests.get(image_url)
    response.raise_for_status()
    image = Image.open(BytesIO(response.content))
    resized_image = image.resize(target_size)
    return resized_image
