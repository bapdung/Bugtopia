using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Models.Response;

public class InsectApi : MonoBehaviour
{
    [SerializeField] private EnvironmentConfig environmentConfig;

    private string insectUrl;

    private void Awake()
    {
        // baseUrl과 엔드포인트를 조합하여 insectUrl 생성
        insectUrl = $"{environmentConfig.baseUrl}/insect";
    }

    // 곤충 정보를 가져오는 API 호출 메서드
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

                Debug.log(responseData);

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
