using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using Models.Insect.Response;
using Models.Insect.Request;

namespace API.Catch
{
    public class CatchApi : MonoBehaviour
    {
        [SerializeField] private EnvironmentConfig environmentConfig;

        private string catchUrl;

        void Awake()
        {
            if (environmentConfig == null)
            {
                environmentConfig = Resources.Load<EnvironmentConfig>("EnvironmentConfig");
            }
            catchUrl = $"{environmentConfig.baseUrl}/catch";
        }

        // S3 URL을 얻는 메서드
        public IEnumerator GetS3Url(string fileName, byte[] photoBytes)
        {
            string requestUrl = $"{environmentConfig.baseUrl}/files/upload/{fileName}";
            string responseS3Url = string.Empty;

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    var jsonResponse = JsonUtility.FromJson<S3Response>(request.downloadHandler.text);
                    responseS3Url = jsonResponse.data.path;
                    Debug.Log(responseS3Url);
                }
            }

            if (!string.IsNullOrEmpty(responseS3Url))
            {
                StartCoroutine(UploadPhotoToS3(responseS3Url, fileName, photoBytes));
            }
        }

        // S3에 사진을 업로드하는 메서드
        public IEnumerator UploadPhotoToS3(string s3Url, string fileName, byte[] photoBytes)
        {
            UnityWebRequest request = new UnityWebRequest(s3Url, "PUT");
            request.uploadHandler = new UploadHandlerRaw(photoBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "image/png");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("s3에 사진 업로드 성공");
                string photoUrl = s3Url; 
                StartCoroutine(PostSearchInsect(photoUrl, (SearchInsectResponse response) =>
                {
                    var cameraManager = FindObjectOfType<CameraManager>();
                    if (cameraManager != null)
                    {
                        cameraManager.OnInsectSearched(response);
                    }
                }));
            }
        }

        // 'postSearchInsect' API에 사진 URL을 전송
        public IEnumerator PostSearchInsect(string photoUrl, Action<SearchInsectResponse> callback)
        {
            string requestUrl = $"{catchUrl}/search";

            var searchRequest = new SearchInsectRequest
            {
                photoUrl = photoUrl
            };

            string jsonRequest = JsonUtility.ToJson(searchRequest);

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequest);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    // API 응답을 SearchInsectResponse 객체로 파싱
                    SearchInsectResponse response = JsonUtility.FromJson<SearchInsectResponse>(request.downloadHandler.text);
                    callback?.Invoke(response);  // 콜백 함수 호출
                }
            }
        }
    }

    [System.Serializable]
    public class SearchInsectRequest
    {
        public string photoUrl;
    }

    // 수정된 응답 객체
    [System.Serializable]
    public class SearchInsectResponse
    {
        public int insectId;           // 곤충ID
        public string krName;          // 곤충명 (한글)
        public string engName;         // 곤충명 (영어)
        public string info;            // 곤충 정보
        public int canRaise;           // 육성 가능(0), 육성 불가능(1), 슬롯 부족(2)
        public string family;          // 곤충 종류
        public string area;            // 서식지
        public string rejectedReason;  // 육성 불가능인 경우 이유, 그 외는 null
    
    }

    [System.Serializable]
    public class S3Response
    {
        public S3Data data;
    }

    [System.Serializable]
    public class S3Data
    {
        public string path;
    }

}
