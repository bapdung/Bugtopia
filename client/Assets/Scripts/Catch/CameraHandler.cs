using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private WebCamTexture webCamTexture;

    void Start()
    {
        // 안드로이드 카메라 접근 권한 요청
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(CheckAndRequestCameraPermission());
        }
        else
        {
            InitializeCamera();
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
        // 웹캠 초기화
        if (WebCamTexture.devices.Length > 0)
        {
            Debug.Log("민채: 카메라 장치 발견: " + WebCamTexture.devices[0].name);

            // 해상도 1080 x 1920으로 웹캠 설정
            webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name, 1080, 1920);
            RawImage rawImage = GetComponent<RawImage>();

            if (rawImage != null)
            {
                rawImage.texture = webCamTexture;
                webCamTexture.Play();

                StartCoroutine(AdjustRotation(rawImage));

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
        // WebCamTexture가 시작될 때까지 대기
        yield return new WaitUntil(() => webCamTexture.didUpdateThisFrame);

        // 비디오 회전 각도를 적용하여 Raw Image 회전
        rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);

        // 필요 시 좌우 반전
        if (webCamTexture.videoVerticallyMirrored)
        {
            rawImage.rectTransform.localScale = new Vector3(rawImage.rectTransform.localScale.x, -rawImage.rectTransform.localScale.y, rawImage.rectTransform.localScale.z);
        }
    }

    void OnDestroy()
    {
        if (webCamTexture != null)
        {
            Debug.Log("민채: 카메라 중지 중...");
            webCamTexture.Stop();
        }
    }
}
