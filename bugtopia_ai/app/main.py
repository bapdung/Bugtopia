# from fastapi import FastAPI
# from pydantic import BaseModel
# from .insect_predictor import predict_insect

# app = FastAPI()

# class ImageRequest(BaseModel):
#     img_url: str

# @app.post("/fastapi/api/insects-detection")
# async def predict(request: ImageRequest):
#     img_url = request.img_url
#     insect_info  = await predict_insect(img_url)
#     return insect_info 

# 임시 api
from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

class ImageRequest(BaseModel):
    img_url: str

# 임시로 사용할 predict_insect 함수 정의
async def predict_insect(img_url: str):
    # 임시 JSON 데이터를 반환
    return {
        "status": "200",
        "content": {
            "name": "장수풍뎅이",
            "order": "장수풍뎅이",
            "family": "장수풍뎅이",
            "genus": "장수풍뎅이",
            "species": "장수풍뎅이"
        }
    }

@app.post("/fastapi/api/insects-detection")
async def predict(request: ImageRequest):
    img_url = request.img_url
    # 임시 predict_insect 함수를 호출
    insect_info = await predict_insect(img_url)
    return insect_info
