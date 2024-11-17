using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using Models.Insect.Response;
using Models.Insect.Request;
using UnityEngine.SceneManagement;

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
        public IEnumerator GetS3Url(string fileName, byte[] photoBytes, string nextScene)
        {
            string requestUrl = $"{environmentConfig.baseUrl}/files/upload/{fileName}";
            string responseS3Url = string.Empty;
            
            string jsonRequest = "{\"prefix\":\"catch\"}";
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
                    var jsonResponse = JsonUtility.FromJson<S3Response>(request.downloadHandler.text);
                    responseS3Url = jsonResponse.url;
                    Debug.Log(responseS3Url);
                }
            }

            if (!string.IsNullOrEmpty(responseS3Url))
            {
                StartCoroutine(UploadPhotoToS3(responseS3Url, fileName, photoBytes, nextScene));
            }
        }

        // S3에 사진을 업로드하는 메서드
        public IEnumerator UploadPhotoToS3(string s3Url, string fileName, byte[] photoBytes, string nextScene)
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

                    SceneManager.LoadScene(nextScene);
                }));
            }
        }

        // 'postSearchInsect' API에 사진 URL을 전송
        public IEnumerator PostSearchInsect(string photoUrl, Action<SearchInsectResponse> callback)
        {
            string requestUrl = $"{catchUrl}/search";

            var searchRequest = new SearchInsectRequest
            {
                imgUrl = photoUrl
            };

            string jsonRequest = JsonUtility.ToJson(searchRequest);
            string userIdForRequest = UserStateManager.Instance.UserId.ToString();

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequest);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("userId", userIdForRequest);

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
        public IEnumerator PostCatch(Action<bool> callback)
        {
            string userIdForRequest = UserStateManager.Instance.UserId.ToString();

            using (UnityWebRequest request = new UnityWebRequest(catchUrl, "POST"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("userId", userIdForRequest);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                    callback?.Invoke(false);
                }
                else
                {
                    callback?.Invoke(true);
                }
            }
        }

        public IEnumerator PostInsectNickname(long userId, InsectNicknameRequest requestBody, Action<InsectNicknameResponse> callback)
        {
            string requestUrl = $"{environmentConfig.baseUrl}/insect";
            if (requestBody.insectId == 0 || requestBody.insectId == null)
            {
                requestBody.insectId = 6;
            }

            string jsonRequest = JsonUtility.ToJson(requestBody);
            Debug.Log(jsonRequest);
            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequest);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("userId", userId.ToString());
                
                yield return request.SendWebRequest();
                Debug.Log(request);
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    InsectNicknameResponse response = JsonUtility.FromJson<InsectNicknameResponse>(request.downloadHandler.text);
                    callback?.Invoke(response);
                }
            }
        }
    }
}