from ultralytics import YOLO
from PIL import Image, ImageDraw
import requests
from io import BytesIO
import yaml
from pathlib import Path
import os, json

# 바운딩 박스 좌표를 가져오는 함수 임포트
from .bounding_box import get_bounding_box
from .predict_config import predict_run
# 모델 및 데이터 경로 설정
MODEL_PATH = 'app/model/prediction_model.pt'
YAML_PATH = 'app/model/data.yaml'
OUTPUT_DIR = '/results'
CLASSES_JSON_PATH = 'app/model/classes.json'

# classes.json 파일에서 곤충 정보를 로드하는 함수
def load_insect_info(class_id):
    with open(CLASSES_JSON_PATH, 'r', encoding='utf-8') as file:
        insect_data = json.load(file)
    return insect_data.get(str(class_id), {})

# 모델 로드 함수
def load_model(model_path):
    return YOLO(model_path)

# 클래스 이름 로드 함수
def load_class_names(yaml_path):
    with open(yaml_path, 'r') as file:
        data = yaml.safe_load(file)
    return data['names']

# S3 URL에서 이미지 로드
def load_image_from_s3(url):
    response = requests.get(url)
    response.raise_for_status()
    img = Image.open(BytesIO(response.content))
    return img

# 이미지를 받아 예측하고 바운딩 박스 그리기
def predict_insect2(image_url):
    model = load_model(MODEL_PATH)
    class_names = load_class_names(YAML_PATH)
    # S3 URL에서 이미지 로드
    img = load_image_from_s3(image_url)
    
    # 예측 수행
    results = model(img)
    
    # 바운딩 박스 좌표 가져오기
    bbox = get_bounding_box(img)
    draw = ImageDraw.Draw(img)
    draw.rectangle(bbox, outline="red", width=3)
    
    # 예측 라벨과 클래스 이름 출력
    predictions = []
    for pred in results[0].boxes.data:
        class_id = int(pred[5])  # 예측된 클래스 ID
        insect_info = load_insect_info(class_id)  # 클래스 ID로 곤충 정보 조회
        predictions.append(insect_info)
    
    # 결과 저장 경로 설정 및 이미지 저장
    image_name = Path(image_url).name
    output_image_path = Path(OUTPUT_DIR) / f"pred_{image_name}"
    img.save(output_image_path)
    
    # 예측 결과 출력
    print(f"Image: {image_name}")
    print("Bounding Box:", bbox)
    print("Predictions (Class ID, Class Name):", predictions)
    return predictions

def predict_insect(img_url):
    response = predict_run(img_url)
    print(response)
    return response