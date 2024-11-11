using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.UI;
using API.Catch;
using Models.Insect.Response;

public class CameraManager : MonoBehaviour
{
    public RawImage cameraView;
    private WebCamTexture webCamTexture;
    public GameObject HelpPanel;
    public GameObject LoadingPanel;
    public CatchApi catchApi;
    private void Awake()
    {
        if (catchApi == null)
        {
            GameObject catchApiObject = new GameObject("CatchApiObject");  // 새 GameObject 생성
            catchApi = catchApiObject.AddComponent<CatchApi>();  // catchApi 컴포넌트를 추가하여 할당
        }
    }
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

            int rotationAngle = webCamTexture.videoRotationAngle;
            cameraView.rectTransform.localEulerAngles = new Vector3(0, 0, -rotationAngle);
        }

        if (HelpPanel != null)
        {
            HelpPanel.SetActive(false); // 도움말 숨기고 시작
            Debug.Log("도움말판넬 찾고 숨겻음");
        }
        if (LoadingPanel != null)
        {
            LoadingPanel.SetActive(false); // 로딩중 숨기고 시작
            Debug.Log("로딩중판넬 찾고 숨겻음");
        }
        

    }

    public void TakePhotoButton()
    {
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        if (LoadingPanel != null)
        {
            LoadingPanel.SetActive(!LoadingPanel.activeSelf);
        }

        string fileName = "photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        byte[] photoBytes = photo.EncodeToPNG(); 

        // S3 URL 요청 및 사진 업로드
        StartCoroutine(catchApi.GetS3Url(fileName, photoBytes));
    }
    // 사진 업로드 후 API 결과를 EntryScanManager로 전달
    public void OnInsectSearched(SearchInsectResponse response)
    {
        // 검색된 곤충 정보를 EntryScanManager에 전달하여 UI 업데이트
        var entryScanManager = FindObjectOfType<EntryScanManager>();
        if (entryScanManager != null)
        {
            entryScanManager.UpdateInsectInfo(response);
        }
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