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

        public IEnumerator GetInsectInfo(long raisingInsectId, System.Action<InsectInfoResponse> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}/{raisingInsectId}";

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    InsectInfoResponse responseData = JsonUtility.FromJson<InsectInfoResponse>(jsonResponse);

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
    }
}
