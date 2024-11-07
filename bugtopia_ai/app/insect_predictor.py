import torch
from .utils import load_and_resize_image
from ultralytics import YOLO

# 모델 로드
model_path = 'app/model/bugtopia_ai_model.pt'
model = YOLO(model_path)


def load_class_names(class_file_path='app/model/classes.txt'):
    class_names = {}
    with open(class_file_path, 'r') as file:
        for line in file:
            index, name = line.strip().split(' ', 1)
            class_names[int(index)] = name
    return class_names


class_names = load_class_names()


async def predict_insect(img_url: str) -> str:
    img = load_and_resize_image(img_url)
    results = model(img)

    # 결과 처리 방식을 YOLOv8 출력에 맞게 수정
    if len(results) > 0 and len(results[0].boxes) > 0:
        predicted_class = int(results[0].boxes[0].cls.item())
        insect_name = class_names.get(predicted_class, "Unknown Insect")
        return insect_name
    return "No insect detected"