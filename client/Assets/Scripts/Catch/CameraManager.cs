using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.UI;
using API.Catch;

public class CameraManager : MonoBehaviour
{
    public RawImage cameraView;
    private WebCamTexture webCamTexture;
    public GameObject HelpPanel;
    private CatchApi catchApi;

    void Start()
    {
        // 권한 받기
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (WebCamTexture.devices.Length > 0) 
        {
            Debug.Log("카메라 디바이스 감지됨: " + WebCamTexture.devices[0].name);
            webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
            cameraView.texture = webCamTexture;
            webCamTexture.Play();
        }

        if (HelpPanel != null)
        {
            HelpPanel.SetActive(false); // 도움말 숨기고 시작
            Debug.Log("판넬 찾고 숨겻음");
        }

        catchApi = FindObjectOfType<CatchApi>();
    }

    public void TakePhotoButton()
    {
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        string fileName = "photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        byte[] photoBytes = photo.EncodeToPNG(); 

        // S3 URL 요청 및 사진 업로드
        StartCoroutine(catchApi.GetS3Url(fileName, photoBytes));

        SceneManager.LoadScene("LoadingScene"); 
    }

    public void BackToHomeButton()
    {
        SceneManager.LoadScene("MainScene");  
    }

    public void HelpButton()
    {
        Debug.Log("버튼 클릭됨");
        // Help Panel의 활성화 상태 반전
        if (HelpPanel != null)
        {
            HelpPanel.SetActive(!HelpPanel.activeSelf);
        }
    }
}
