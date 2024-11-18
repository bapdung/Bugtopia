from fastapi import FastAPI
from pydantic import BaseModel
from .insect_predictor import predict_insect

app = FastAPI()

class ImageRequest(BaseModel):
    img_url: str

@app.post("/fastapi/api/insects-detection")
async def predict(request: ImageRequest):
    # 일단 임시로
    return {"status":200, "content":"Megasoma elephas"}

    img_url = request.img_url
    insect_info  = predict_insect(img_url)

    if insect_info == "No Insect Detected":status_code = 401
    else: status_code = 200


    response_data = {
        "status":status_code,
        "content":insect_info
    }
    return response_data
