from fastapi import FastAPI
from pydantic import BaseModel
from .insect_predictor import predict_insect

app = FastAPI()

class ImageRequest(BaseModel):
    img_url: str

@app.post("")
async def predict(request: ImageRequest):
    img_url = request.img_url
    insect_name = await predict_insect(img_url)
    return insect_name