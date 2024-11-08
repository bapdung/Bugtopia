using Firebase.Extensions;
using UnityEngine;
using Firebase;
using Firebase.Messaging;
using System.Threading.Tasks;

public class FirebaseHandler : MonoBehaviour
{
    private static bool instanceExists;

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

        InitializeFirebase();
    }

    // Firebase 초기화
    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            if (task.Result == DependencyStatus.Available)
            {
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
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("알림 수신: " + e.Message.Notification.Body);
    }
}
