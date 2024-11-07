using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public RawImage cameraView;
    private WebCamTexture webCamTexture;

    void Start()
    {
        if (WebCamTexture.devices.Length > 0) 
        {
            webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
            cameraView.texture = webCamTexture;
            webCamTexture.Play();
        }
    }

    public void TakePhoto()
    {
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        // AI 모델에 사진을 전송하는 코드 추가 필요

        SceneManager.LoadScene("ResultScene");  // 결과 화면으로 이동
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("MainScene");  
    }
}
