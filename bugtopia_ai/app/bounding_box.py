import torch
from PIL import Image
from pathlib import Path
from ultralytics import YOLO

# 모델 로딩 함수
def load_model(model_path):
    model = YOLO(model_path)
    return model

# 경로 설정
model_path = 'app/model/bounding_model.pt'  # YOLO 모델 경로

# 모델 로드
model = load_model(model_path)

# 이미지를 받아서 예측하는 함수
def get_bounding_box(img):
    # 이미지를 열어서 모델의 입력에 맞게 처리

    # 이미지 예측
    results = model(img)
    print(results)
    # 예측 결과에서 가장 높은 확률을 가진 클래스 추출
    try:
        return results[0].boxes.data[0]
    except:
        width, height = img.size
        return 0, 0, width, height
    