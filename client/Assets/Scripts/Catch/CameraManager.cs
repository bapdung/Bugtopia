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
            GameObject catchApiObject = new GameObject("CatchApiObject");
            catchApi = catchApiObject.AddComponent<CatchApi>();
        }
    }

    void Start()
    {
        // 카메라 권한 확인 및 요청
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            StartCoroutine(CheckCameraPermission());
        }
        else
        {
            InitializeCamera();
        }

        // HelpPanel과 LoadingPanel 초기화
        if (HelpPanel != null)
        {
            HelpPanel.SetActive(false);
            Debug.Log("도움말 패널 숨김");
        }
        if (LoadingPanel != null)
        {
            LoadingPanel.SetActive(false);
            Debug.Log("로딩 패널 숨김");
        }
    }

    IEnumerator CheckCameraPermission()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return null; // 계속 대기
        }

        Debug.Log("카메라 권한 승인됨");
        InitializeCamera();
    }

    private void InitializeCamera()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            Debug.Log("카메라 디바이스 감지됨: " + WebCamTexture.devices[0].name);
            webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
            cameraView.texture = webCamTexture;
            webCamTexture.Play();

            // 카메라 비율 유지 및 화면에 꽉 차도록 설정
            float videoRatio = (float)webCamTexture.width / webCamTexture.height;
            float screenRatio = (float)Screen.width / Screen.height;

            if (videoRatio > screenRatio)
            {
                // 비디오가 더 넓은 경우 - 높이를 기준으로 너비 조정
                cameraView.rectTransform.sizeDelta = new Vector2(cameraView.rectTransform.sizeDelta.y * videoRatio, cameraView.rectTransform.sizeDelta.y);
            }
            else
            {
                // 화면이 더 넓은 경우 - 너비를 기준으로 높이 조정
                cameraView.rectTransform.sizeDelta = new Vector2(cameraView.rectTransform.sizeDelta.x, cameraView.rectTransform.sizeDelta.x / videoRatio);
            }

            // 카메라 회전 각도 보정
            int rotationAngle = webCamTexture.videoRotationAngle;
            cameraView.rectTransform.localEulerAngles = new Vector3(0, 0, -rotationAngle);

            Debug.Log("카메라 초기화 완료 및 출력 시작");
        }
        else
        {
            Debug.LogError("사용 가능한 카메라가 없습니다.");
        }
    }

    public void TakePhotoButton()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying)
        {
            Debug.LogWarning("카메라가 활성화되어 있지 않습니다.");
            return;
        }

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        if (LoadingPanel != null)
        {
            LoadingPanel.SetActive(!LoadingPanel.activeSelf);
        }

        string fileName = "photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        byte[] photoBytes = photo.EncodeToPNG();

        Debug.Log("사진 촬영 및 파일 저장 준비 완료: " + fileName);

        // S3 URL 요청 및 사진 업로드
        StartCoroutine(catchApi.GetS3Url(fileName, photoBytes, "EntryScanScene"));
    }

    public void OnInsectSearched(SearchInsectResponse response)
    {
        var entryScanManager = FindObjectOfType<EntryScanManager>();
        if (entryScanManager != null)
        {
            entryScanManager.UpdateInsectInfo(response);
            Debug.Log("곤충 정보 UI 업데이트 완료");
        }
        else
        {
            Debug.LogWarning("EntryScanManager를 찾을 수 없습니다.");
        }
    }

    public void BackToHomeButton()
    {
        Debug.Log("메인 화면으로 돌아가기 버튼 클릭");
        SceneManager.LoadScene("InsectBook");
    }

    public void HelpButton()
    {
        Debug.Log("도움말 버튼 클릭됨");
        if (HelpPanel != null)
        {
            HelpPanel.SetActive(!HelpPanel.activeSelf);
        }
    }

    void OnDestroy()
    {
        if (webCamTexture)
        {
            webCamTexture.Stop();
            webCamTexture = null;
            Debug.Log("카메라 정지 및 리소스 해제");
        }
    }
}
