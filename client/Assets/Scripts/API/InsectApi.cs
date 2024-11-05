using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

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

        public IEnumerator GetInsectInfo(long raisingInsectId, System.Action<Models.Insect.Response.InsectInfo> onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}/{raisingInsectId}";

            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    Models.Insect.Response.InsectInfo responseData = JsonUtility.FromJson<Models.Insect.Response.InsectInfo>(jsonResponse);

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

        public IEnumerator PostIncreaseScore(Models.Insect.Request.IncreaseScore requestData, System.Action onSuccess, System.Action<string> onFailure)
        {
            string requestUrl = $"{insectUrl}/increaseScore";

            // JSON 직렬화
            string json = JsonUtility.ToJson(requestData);

            using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();

                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    if (request.responseCode == 201) // HTTP 상태 코드 201 확인
                    {
                        Debug.Log("점수 증가 성공");
                        onSuccess?.Invoke();
                    }
                    else
                    {
                        Debug.LogError("응답 성공이지만 예상치 못한 상태 코드: " + request.responseCode);
                    }
                }
                else
                {
                    Debug.LogError("점수 증가 요청 실패: " + request.error);
                    onFailure?.Invoke(request.error);
                }
            }
        }
    }
}
