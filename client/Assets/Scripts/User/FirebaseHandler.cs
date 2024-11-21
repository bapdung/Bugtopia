
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Messaging;
using System.Collections;
using API.User;
using Models.User.Request;
using Models.User.Response;
using UnityEngine.UI;
using TMPro;

public class FirebaseHandler : MonoBehaviour
{
  private static bool instanceExists;
  public UserApi userApi;
  public Button button;

  private void Awake()
  {
    Debug.Log("지흔: FirebaseHandler의 Awake 메서드가 실행되었습니다.");

    if (!instanceExists)
    {
      // 이 GameObject가 씬 전환 시에도 유지되도록 설정
      DontDestroyOnLoad(this.gameObject);
      instanceExists = true;
      Debug.Log("지흔: FirebaseHandler가 전역 공간에 등록되었습니다.");
    }
    else
    {
      // 이미 인스턴스가 존재하는 경우 중복을 방지하기 위해 파괴
      Destroy(this.gameObject);
      return;
    }

    // UserApi 인스턴스 초기화
    userApi = gameObject.AddComponent<UserApi>();

    // 코루틴을 시작하여 Firebase 초기화
    StartCoroutine(InitializeFirebase());
  }

  // Firebase 초기화
  private IEnumerator InitializeFirebase()
  {
    var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
    yield return new WaitUntil(() => dependencyTask.IsCompleted);

    if (dependencyTask.Result == DependencyStatus.Available)
    {
      FirebaseApp app = FirebaseApp.DefaultInstance;
      Debug.Log("지흔: Firebase 시작!");

      // Firebase Messaging 초기화
      FirebaseMessaging.TokenReceived += OnTokenReceived;
      FirebaseMessaging.MessageReceived += OnMessageReceived;
    }
    else
    {
      Debug.LogError("지흔: Firebase 시작 오류: " + dependencyTask.Result);
    }
  }

  // Firebase Messaging token 받아오기
  public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
  {
    Debug.Log("지흔: Firebase token: " + token.Token);

    // 전역 공간에 deviceId 저장
    UserStateManager.Instance.SetDeviceId(token.Token);

    // 로그인 요청을 코루틴으로 전환
    StartCoroutine(SendLoginRequest(token.Token));
  }

  // 로그인 요청 코루틴
  private IEnumerator SendLoginRequest(string token)
  {
    yield return userApi.PostLogin(
        new UserLoginRequest { deviceId = token },
        OnLoginSuccess,
        OnRequestFailure
    );
  }

  // 로그인 성공 시 호출되는 콜백
  public void OnLoginSuccess(UserLoginResponse response)
  {
    if (response == null)
    {
      Debug.LogError("지흔: 응답이 null입니다. 로그인 응답을 확인하세요.");
      return; // response가 null일 경우 함수 종료
    }

    if (response.joined == true)
    {
      Debug.Log("지흔: 이미 가입된 사용자: " + response.nickname);

      // 전역 공간에 userId와 nickname 저장
      UserStateManager.Instance.SetUserId(response.userId ?? 0);
      UserStateManager.Instance.SetNickname(response.nickname);

      // button 누르면 MainScene으로 이동
      button.onClick.AddListener(() => moveScene("MainScene"));
    }
    else if (response.joined == false)
    {
      Debug.Log("지흔: 가입되지 않은 사용자, CreateNicknameScene으로 이동합니다.");

      // button 누르면 CreateNicknameScene으로 이동
      button.onClick.AddListener(() => moveScene("CreateNicknameScene"));
    }
  }

  // 회원가입 로직
  public IEnumerator JoinUser(string nickname)
  {
    string deviceId = UserStateManager.Instance.DeviceId;

    if (!string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(nickname))
    {
      yield return userApi.PostJoin(
          new UserJoinRequest { deviceId = deviceId, nickname = nickname },
          OnJoinSuccess,
          OnRequestFailure
      );
    }
    else
    {
      Debug.LogError("지흔: deviceId 또는 nickname이 설정되지 않았습니다.");
    }
  }

  // 회원가입 성공 시 호출되는 콜백
  public void OnJoinSuccess(UserJoinResponse response)
  {
    Debug.Log("지흔: 회원가입 성공 - 사용자 ID: " + response.userId + ", 닉네임: " + response.nickname);

    // 전역 공간에 userId와 nickname 저장
    UserStateManager.Instance.SetUserId(response.userId);
    UserStateManager.Instance.SetNickname(response.nickname);

    SceneManager.LoadScene("MainScene");
  }

  // 요청 실패 시 호출되는 콜백
  public void OnRequestFailure(string error)
  {
    Debug.LogError("지흔: User Api 요청 실패: " + error);
  }

  // 메시지 수신 시 호출되는 이벤트 핸들러
  public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
  {
    Debug.Log("지흔: 알림 수신: " + e.Message.Notification.Body);
  }

  public void moveScene(string sceneName)
  {
    SceneManager.LoadScene(sceneName);
  }
}