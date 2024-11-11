using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Models.User.Request;
using Models.User.Response;

namespace API.User
{
    public class UserApi : MonoBehaviour
    {
        [SerializeField] private EnvironmentConfig environmentConfig;

        private string loginUrl;
        private string joinUrl;

        private void Awake()
        {
            if (environmentConfig == null)
            {
                environmentConfig = Resources.Load<EnvironmentConfig>("EnvironmentConfig");
            }

            loginUrl = $"{environmentConfig.baseUrl}/user/login";
            joinUrl = $"{environmentConfig.baseUrl}/user/join";
        }

        // 로그인 요청 메서드
        public IEnumerator PostLogin(UserLoginRequest requestData, System.Action<UserLoginResponse> onSuccess, System.Action<string> onFailure)
        {
            Debug.Log("지흔: 로그인");
            string json = JsonUtility.ToJson(requestData);

            using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;

                    
                    UserLoginResponse responseData = JsonUtility.FromJson<UserLoginResponse>(jsonResponse);

                    // 성공 콜백 호출
                    onSuccess?.Invoke(responseData);
                }
                else
                {
                    Debug.LogError("지흔: 로그인 요청 실패: " + request.error);
                    // 실패 콜백 호출 (오류 메시지 전달)
                    onFailure?.Invoke(request.error);
                }
            }
        }

        // 회원가입 요청 메서드
        public IEnumerator PostJoin(UserJoinRequest requestData, System.Action<UserJoinResponse> onSuccess, System.Action<string> onFailure)
        {
            Debug.Log("지흔: 회원가입 실행");
            string json = JsonUtility.ToJson(requestData);

            using (UnityWebRequest request = new UnityWebRequest(joinUrl, "POST"))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    UserJoinResponse responseData = JsonUtility.FromJson<UserJoinResponse>(jsonResponse);

                    // 성공 콜백 호출
                    onSuccess?.Invoke(responseData);
                }
                else
                {
                    Debug.LogError("민채: 회원가입 요청 실패: " + request.error);
                    // 실패 콜백 호출 (오류 메시지 전달)
                    onFailure?.Invoke(request.error);
                }
            }
        }
    }
}
