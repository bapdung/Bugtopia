using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using API.Catch;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public RawImage CameraView;
    private WebCamTexture webCamTexture;
    public GameObject infoPopup; // 민채: 팝업 창 오브젝트 연결
    public Button confirmButton; // 민채: 확인 버튼 오브젝트 연결
    public Button captureButton; // 민채: 촬영 버튼 오브젝트 연결
    public Button returnButton;
    public GameObject loadingPanel; // 민채: 로딩 패널 오브젝트 연결
    public CatchApi catchApi;

    private void Awake()
    {
        // 민채: CatchApi 초기화
        if (catchApi == null)
        {
            GameObject catchApiObject = new GameObject("CatchApiObject");
            catchApi = catchApiObject.AddComponent<CatchApi>();
        }
    }

    void Start()
    {
        // 민채: 버튼 클릭 시 팝업 창을 닫는 함수 연결
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(ClosePopup);
        }
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(onClickReturnButton);
        }
        

        // 민채: 촬영 버튼 클릭 시 사진을 찍는 함수 연결
        if (captureButton != null)
        {
            captureButton.onClick.AddListener(TakePhotoButton);
        }

        // 민채: 안드로이드 카메라 접근 권한 요청
        if (Application.platform == RuntimePlatform.Android)
        {
            ShowInfoPopup(); // 민채: 권한 요청 전 팝업 창 보여주기
            StartCoroutine(CheckAndRequestCameraPermission());
        }
        else
        {
            InitializeCamera();
        }

        // 민채: 로딩 패널 초기화
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
            Debug.Log("민채: 로딩 패널 숨김");
        }
    }

    private void ShowInfoPopup()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(true); // 민채: 팝업 창 활성화
        }
    }

    private void ClosePopup()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(false); // 민채: 팝업 창 비활성화
            confirmButton.gameObject.SetActive(false);
        }
    }

    private IEnumerator CheckAndRequestCameraPermission()
    {
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("민채: 카메라 권한 요청 중...");
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Debug.LogError("민채: 카메라 권한이 거부되었습니다.");
                yield break; // 권한이 거부된 경우 초기화를 중단
            }
        }
        Debug.Log("민채: 카메라 권한이 승인되었습니다.");
        InitializeCamera();
    }

    private void InitializeCamera()
    {
        // 민채: 웹캠 초기화
        if (WebCamTexture.devices.Length > 0)
        {
            Debug.Log("민채: 카메라 장치 발견: " + WebCamTexture.devices[0].name);

            // 민채: 해상도 1080 x 1920으로 웹캠 설정
            webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name, 1080, 1920);
            CameraView.texture = webCamTexture;
            webCamTexture.Play();
            // RawImage rawImage = GetComponent<RawImage>();

            if (CameraView != null)
            {

                StartCoroutine(AdjustRotation(CameraView));

                if (webCamTexture.isPlaying)
                {
                    Debug.Log("민채: 카메라가 성공적으로 시작되었습니다.");
                }
                else
                {
                    Debug.LogError("민채: 카메라를 시작할 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("민채: RawImage 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("민채: 사용 가능한 카메라 장치를 찾을 수 없습니다.");
        }
    }


    private IEnumerator AdjustRotation(RawImage rawImage)
    {
        // 민채: WebCamTexture가 시작될 때까지 대기
        yield return new WaitUntil(() => webCamTexture.didUpdateThisFrame);

        // 민채: 비디오 회전 각도를 적용하여 Raw Image 회전
        rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);

        // 민채: 필요 시 좌우 반전
        if (webCamTexture.videoVerticallyMirrored)
        {
            rawImage.rectTransform.localScale = new Vector3(rawImage.rectTransform.localScale.x, -rawImage.rectTransform.localScale.y, rawImage.rectTransform.localScale.z);
        }
    }

    public void TakePhotoButton()
    {
        if (webCamTexture == null || !webCamTexture.isPlaying)
        {
            Debug.LogWarning("민채: 카메라가 활성화되어 있지 않습니다.");
            return;
        }

        // 민채: 사진 캡처 및 Texture2D 생성
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true); // 민채: 로딩 패널 활성화
        }

        string fileName = "photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        byte[] photoBytes = photo.EncodeToPNG();

        Debug.Log("민채: 사진 촬영 및 S3 업로드 준비 완료: " + fileName);

        // 민채: S3 URL 요청 및 사진 업로드
        StartCoroutine(catchApi.GetS3Url(fileName, photoBytes, "EntryScanScene"));
        loadingPanel.SetActive(false);
    }

    public void onClickReturnButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    void OnDestroy()
    {
        if (webCamTexture != null)
        {
            Debug.Log("민채: 카메라 정지 중...");
            webCamTexture.Stop();
        }
    }
}
