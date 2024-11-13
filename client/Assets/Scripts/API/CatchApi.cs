using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using Models.Insect.Response;
using Models.Insect.Request;
using Models.InsectBook.Response;

namespace API.Catch
{
    public class CatchApi : MonoBehaviour
    {
        [SerializeField] private EnvironmentConfig environmentConfig;

        private string catchUrl;
        private long userId ;

        void Awake()
        {
            userId = UserStateManager.Instance.UserId;

            if (environmentConfig == null)
            {
                environmentConfig = Resources.Load<EnvironmentConfig>("EnvironmentConfig");
            }
            catchUrl = $"{environmentConfig.baseUrl}/catch";
        }

        // // S3 URL을 얻는 메서드
        // public IEnumerator GetS3Url(string fileName, byte[] photoBytes)
        // {
        //     string requestUrl = $"{environmentConfig.baseUrl}/files/upload/{fileName}";
        //     string responseS3Url = string.Empty;

        //     using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        //     {
        //         yield return request.SendWebRequest();

        //         if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        //         {
        //             Debug.LogError(request.error);
        //         }
        //         else
        //         {
        //             var jsonResponse = JsonUtility.FromJson<S3Response>(request.downloadHandler.text);
        //             responseS3Url = jsonResponse.data.path;
        //             Debug.Log(responseS3Url);
        //         }
        //     }

        //     if (!string.IsNullOrEmpty(responseS3Url))
        //     {
        //         StartCoroutine(UploadPhotoToS3(responseS3Url, fileName, photoBytes));
        //     }
        // }



        // // S3 URL을 얻는 메서드 (POST 요청으로 변경)
        // public IEnumerator GetS3Url(string fileName, byte[] photoBytes)
        // {
        //     string requestUrl = $"{environmentConfig.baseUrl}/files/upload/{fileName}";
        //     string responseS3Url = string.Empty;

        //     // JSON body에 prefix 추가
        //     var postData = new
        //     {
        //         prefix = "sample"
        //     };

        //     string jsonRequest = JsonUtility.ToJson(postData);

        //     using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
        //     {
        //         byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequest);
        //         request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        //         request.downloadHandler = new DownloadHandlerBuffer();
        //         request.SetRequestHeader("Content-Type", "application/json");
        //         Debug.Log(requestUrl);
        //         Debug.Log(postData);
        //         Debug.Log(jsonRequest);
        //         yield return request.SendWebRequest();

        //         if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        //         {
        //             Debug.LogError(request.error);
        //         }
        //         else
        //         {
        //             // 응답 JSON 파싱
        //             var jsonResponse = JsonUtility.FromJson<S3Response>(request.downloadHandler.text);
        //             responseS3Url = jsonResponse.data.path;
        //             Debug.Log(responseS3Url);
        //         }
        //     }

        //     if (!string.IsNullOrEmpty(responseS3Url))
        //     {
        //         // S3에 파일 업로드
        //         StartCoroutine(UploadPhotoToS3(responseS3Url, fileName, photoBytes));
        //     }
        // }

        [System.Serializable]
        public class PostData
        {
            public string prefix;
        }

        public IEnumerator GetS3Url(string fileName, byte[] photoBytes)
        {
            string requestUrl = $"{environmentConfig.baseUrl}/files/upload/{fileName}";
            string responseS3Url = string.Empty;

            // 명시적으로 클래스를 정의하여 객체 생성
            var postData = new PostData
            {
                prefix = "catch"
            };

            string jsonRequest = JsonUtility.ToJson(postData); // 이제 jsonRequest에 올바르게 값이 직렬화됩니다.

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequest);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                Debug.Log(requestUrl);
                Debug.Log(postData.prefix); // "sample"이 찍힙니다.
                Debug.Log(jsonRequest); // 이제 제대로 직렬화된 JSON이 출력됩니다.
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    // 응답 JSON 파싱
                    var jsonResponse = JsonUtility.FromJson<S3Response>(request.downloadHandler.text);
                    Debug.Log(jsonResponse);
                    responseS3Url = jsonResponse.url;
                    Debug.Log(responseS3Url);
                }
            }

            if (!string.IsNullOrEmpty(responseS3Url))
            {
                // S3에 파일 업로드
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
        public IEnumerator PostCatch(Action<bool> callback)
        {
            using (UnityWebRequest request = new UnityWebRequest(catchUrl, "POST"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

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
            string jsonRequest = JsonUtility.ToJson(requestBody);

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequest);
                request.uploadHandler = new UploadHandlerRaw(jsonBytes);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("userId", userId.ToString());
                
                yield return request.SendWebRequest();
                Debug.Log(request.result);
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

        public IEnumerator GetCatchInsectList(System.Action<CatchListResponse> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{catchUrl}?viewType=CATCHED";

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("userId", userId.ToString());

                yield return request.SendWebRequest();

                if(request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    CatchListResponse responseData = JsonUtility.FromJson<CatchListResponse>(jsonResponse);
                    onSuccess?.Invoke(responseData);
                }
                else
                {
                    onFailure?.Invoke(request.error);
                }
            }
        }

        public IEnumerator GetBookDetail(int insectId, System.Action<BookDetailResponse> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{catchUrl}/{insectId}?viewType=CATCHED";

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("userId", userId.ToString());

                yield return request.SendWebRequest();

                if(request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    BookDetailResponse responseData = JsonUtility.FromJson<BookDetailResponse>(jsonResponse);
                    onSuccess?.Invoke(responseData);
                }
                else
                {
                    onFailure?.Invoke(request.error);
                }
            }
        }
    }
}