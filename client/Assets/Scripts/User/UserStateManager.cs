using UnityEngine;

// 전역 상태를 관리하는 싱글톤 클래스
public class UserStateManager : MonoBehaviour
{
    // 클래스의 단일 인스턴스를 저장하는 정적 변수
    private static UserStateManager _instance;

    // 외부에서 인스턴스에 접근할 수 있도록 하는 프로퍼티
    public static UserStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 현재 씬에서 UserStateManager 인스턴스를 찾음
                _instance = FindObjectOfType<UserStateManager>();
                if (_instance == null)
                {
                    // 인스턴스가 없으면 새 GameObject를 생성하고 UserStateManager 컴포넌트를 추가
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<UserStateManager>();
                    singletonObject.name = typeof(UserStateManager).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject); // 씬 전환 시에도 객체가 파괴되지 않도록 설정
                }
            }
            return _instance;
        }
    }

    // 사용자 ID를 저장하는 프로퍼티 (읽기 전용)
    public long UserId { get; private set; }

    // 기기 ID를 저장하는 프로퍼티 (읽기 전용)
    public string DeviceId { get; private set; }

    // 닉네임을 저장하는 프로퍼티 (읽기 전용)
    public string Nickname { get; private set; }

    // MonoBehaviour의 Awake 메서드 - 싱글톤 패턴을 보장
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 객체가 유지되도록 설정
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // 중복된 인스턴스가 있으면 삭제
        }

        // 초기 값 설정 (테스트용 하드코딩, 실제 사용 시 적절한 초기화 필요)
        UserId = 1; // 기본 사용자 ID 설정
        DeviceId = "default_device_id"; // 기본 기기 ID 설정
        Nickname = "default_nickname"; // 기본 닉네임 설정
    }

    // 외부에서 UserId를 설정할 수 있는 메서드
    public void SetUserId(long userId)
    {
        UserId = userId;
        Debug.Log("UserId가 변경되었습니다: " + UserId);
    }

    // 외부에서 DeviceId를 설정할 수 있는 메서드
    public void SetDeviceId(string deviceId)
    {
        DeviceId = deviceId;
        Debug.Log("DeviceId가 변경되었습니다: " + DeviceId);
    }

    // 외부에서 Nickname을 설정할 수 있는 메서드
    public void SetNickname(string nickname)
    {
        Nickname = nickname;
        Debug.Log("Nickname이 변경되었습니다: " + Nickname);
    }
}
