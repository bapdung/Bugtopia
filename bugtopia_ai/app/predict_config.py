import requests
import base64
import json
API_KEY = "u1imsDa5DpAY8FrNHR3syspdcZScwGzAbzWWR9vqRZrpGXJZPY"
S3URL = ''
# S3에서 이미지를 가져와 Base64로 인코딩하는 함수
def s3_image_to_base64(s3_url):
    try:
        # S3 URL에서 이미지 다운로드
        response = requests.get(s3_url)
        response.raise_for_status()  # 요청 오류 발생 시 예외 발생

        # 이미지를 Base64로 인코딩
        base64_image = base64.b64encode(response.content).decode('utf-8')
        return f"data:image/jpeg;base64,{base64_image}"  # JPEG 형식으로 반환

    except requests.exceptions.RequestException as e:
        print(f"Error downloading image from S3: {e}")
        return None

latitude = 49.207
longitude = 16.608

def predict_run(s3_url):
    base64_image = s3_image_to_base64(s3_url)

    url = "https://insect.kindwise.com/api/v1/identification?details=common_names%2Curl%2Cdescription%2Cimage"
    headers = {
        "Api-Key": API_KEY,
        "Content-Type": "application/json"
    }

    data = {
        "images": [base64_image],  
        "latitude": latitude,
        "longitude": longitude,
        "similar_images": True
    }

    try:
        response = requests.post(url, headers=headers, json=data)
        response.raise_for_status() 
        print(response)
        json_response = response.json()
        print(json_response)
        insect_name = json_response["result"]["classification"]["suggestions"][0]["name"]
        print(insect_name)
        return insect_name

    except requests.exceptions.RequestException as e:
        print(f"Error making request to API: {e}")
        return None
