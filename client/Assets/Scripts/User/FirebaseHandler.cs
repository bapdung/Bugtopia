using Firebase.Extensions;
using UnityEngine;
using Firebase;
using Firebase.Messaging;
using UnityEngine.Networking;
using System.Collections;
using API.User;
using System.Text;
using Models.User.Request;
using Models.User.Response;

public class FirebaseHandler : MonoBehaviour
{
    private static bool instanceExists;
    private UserApi userApi; // UserApi 인스턴스

    private void Awake()
    {
        if (!instanceExists)
        {
            // 이 GameObject가 씬 전환 시에도 유지되도록 설정
            DontDestroyOnLoad(this.gameObject);
            instanceExists = true;
            Debug.Log("FirebaseHandler 가 전역 공간에 등록되었습니다.");
        }
        else
        {
            // 이미 인스턴스가 존재하는 경우 중복을 방지하기 위해 파괴
            Destroy(this.gameObject);
            return;
        }

        // UserApi 인스턴스 초기화
        userApi = gameObject.AddComponent<UserApi>();

        InitializeFirebase();
    }

    // Firebase 초기화
    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase 시작!");

                // Firebase Messaging 초기화
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
            }
            else
            {
                Debug.LogError("Firebase 시작 오류: " + task.Result);
            }
        });
    }

    // Firebase Messaging token 받아오기
    private void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Firebase token: " + token.Token);

        // 로그인 요청 전송
        StartCoroutine(userApi.PostLogin(
            new UserLoginRequest { deviceId = token.Token },
            OnLoginSuccess,
            OnRequestFailure
        ));
    }

    // 로그인 성공 시 호출되는 콜백
    private void OnLoginSuccess(UserLoginResponse response)
    {
        if (response.isJoined)
        {
            Debug.Log("이미 가입된 사용자: " + response.nickname);
        }
        else
        {
            Debug.Log("가입되지 않은 사용자, CreateNicknameScene으로 이동합니다.");

            // CreateNicknameScene으로 이동
            SceneManager.LoadScene("CreateNicknameScene");
        }
    }

    // 회원가입 성공 시 호출되는 콜백
    private void OnJoinSuccess(UserJoinResponse response)
    {
        Debug.Log("가입 완료 - 사용자 ID: " + response.userId + ", 닉네임: " + response.nickname);
    }

    // 요청 실패 시 호출되는 콜백
    private void OnRequestFailure(string error)
    {
        Debug.LogError("User Api 요청 실패: " + error);
    }

    // 메시지 수신 시 호출되는 이벤트 핸들러
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("알림 수신: " + e.Message.Notification.Body);
    }
}