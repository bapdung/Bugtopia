using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Models.Insect.Response;
using Models.Insect.Request;

namespace API.Insect
{
    public class InsectApi : MonoBehaviour
    {
        [SerializeField] private EnvironmentConfig environmentConfig;

        private string insectUrl;

        private void Awake()
        {
            if (environmentConfig == null)
            {
                environmentConfig = Resources.Load<EnvironmentConfig>("EnvironmentConfig");
            }

            insectUrl = $"{environmentConfig.baseUrl}/insect";
        }

        public IEnumerator GetInsectInfo(long raisingInsectId, System.Action<InsectDetailInfoResponse> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}/{raisingInsectId}";

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    InsectDetailInfoResponse responseData = JsonUtility.FromJson<InsectDetailInfoResponse>(jsonResponse);

                    // 성공 콜백 호출
                    onSuccess?.Invoke(responseData);
                }
                else
                {
                    // 실패 콜백 호출 (오류 메시지 전달)
                    onFailure?.Invoke(request.error);
                }
            }
        }

        public IEnumerator GetInsectArInfo(long raisingInsectId , System.Action<InsectArInfoResponse> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}/ar/{raisingInsectId}";

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if(request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    InsectArInfoResponse responseData = JsonUtility.FromJson<InsectArInfoResponse>(jsonResponse);

                    onSuccess?.Invoke(responseData);
                }
                else 
                {
                    onFailure?.Invoke(request.error);
                }
            }
        }

        public IEnumerator PostIncreaseScore(IncreaseScoreRequest requestData, System.Action<IncreaseScoreResponse> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}/love-score";

            // JSON 직렬화
            string json = JsonUtility.ToJson(requestData);
            Debug.Log("JSON: " + json);

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();

                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    IncreaseScoreResponse responseData = JsonUtility.FromJson<IncreaseScoreResponse>(jsonResponse);

                    onSuccess?.Invoke(responseData);
                    
                }
                else
                {
                    Debug.LogError("점수 증가 요청 실패: " + request.error);
                    onFailure?.Invoke(request.error);
                }
            }
        }

        public IEnumerator GetInsectListWithRegion(string region, System.Action<InsectListWithRegionResponse> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}/area?areaType={region}";

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("userId", UserStateManager.Instance.UserId.ToString());

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    InsectListWithRegionResponse responseData = JsonUtility.FromJson<InsectListWithRegionResponse>(jsonResponse);

                    Debug.Log("InsectListWithRegionResponse: " + responseData.num);
                    // Debug.Log("InsectListWithRegionResponse: " + responseData.insectList);
                    // foreach (InsectInfo child in responseData.insectList)
                    // {
                    //     Debug.Log(child.family + " " + child.raisingInsectId + " " + child.nickname);
                    // }

                    // 성공 콜백 호출
                    onSuccess?.Invoke(responseData);
                }
                else
                {
                    // 실패 콜백 호출 (오류 메시지 전달)
                    onFailure?.Invoke(request.error);
                }
            }
        }

        // 곤충 키우기
        public IEnumerator RaiseInsect(long insectId, string nickname, System.Action<string> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}";

            // InsectNicknameRequest 객체 생성
            InsectNicknameRequest requestBody = new InsectNicknameRequest
            {
                insectId = insectId,
                nickname = nickname
            };
            
            string json = JsonUtility.ToJson(requestBody);

            Debug.Log($"민채: Request URL: {requestUrl}");
            Debug.Log($"민채: JSON Body: {json}");

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();

                // 요청 헤더 설정
                request.SetRequestHeader("Content-Type", "application/json");
                string userId = UserStateManager.Instance.UserId.ToString();
                request.SetRequestHeader("userId", userId);

                Debug.Log($"민채: Header 'Content-Type': application/json");
                Debug.Log($"민채: Header 'userId': {userId}");

                yield return request.SendWebRequest();

                // 응답 결과 확인
                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log($"민채: RaiseInsect 성공 - 응답 데이터: {jsonResponse}");

                    // 성공 콜백 호출
                    onSuccess?.Invoke(jsonResponse);
                }
                else
                {
                    Debug.LogError($"민채: 곤충 키우기 RaiseInsect 요청 실패");
                    Debug.LogError($"민채: HTTP 상태 코드: {request.responseCode}");
                    Debug.LogError($"민채: 요청 에러: {request.error}");
                    Debug.LogError($"민채: 응답 데이터: {request.downloadHandler.text}");

                    // 실패 콜백 호출
                    onFailure?.Invoke(request.error);
                }
            }
        }


    }
}
