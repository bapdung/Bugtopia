# Python 3.9 slim 이미지 사용
FROM python:3.9-slim-buster

# 작업 디렉토리 설정
WORKDIR /app

# 시스템 패키지 및 Python 개발 도구 설치
RUN apt-get update && apt-get install -y \
    libgl1-mesa-glx \
    libglib2.0-0 \
    gcc \
    python3-dev \
    && rm -rf /var/lib/apt/lists/*

# pip 업그레이드
RUN pip install --upgrade pip

# requirements.txt 복사 및 패키지 설치
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# 애플리케이션 코드 복사
COPY ./app ./app

# 모델 파일이 있는 디렉토리 생성 및 복사
RUN mkdir -p /app/app/model
COPY ./app/model/bounding_model.pt /app/app/model/
COPY ./app/model/prediction_model.pt /app/app/model/
COPY ./app/model/classes.txt /app/app/model/

# Python 경로 설정
ENV PYTHONPATH="/app:${PYTHONPATH}"

# 포트 80 노출
EXPOSE 8001

# FastAPI 앱 실행
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8001"]
