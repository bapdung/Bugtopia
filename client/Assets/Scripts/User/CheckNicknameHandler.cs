using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro 사용을 위해 추가

public class CheckNicknameHandler : MonoBehaviour
{
    public GameObject confirmButton;
    public GameObject cancelButton;
    public TextMeshProUGUI nicknameCheck; // TextMeshProUGUI로 변경

    private FirebaseHandler firebaseHandler;

    void Start()
    {
        // FirebaseHandler 인스턴스 찾기
        firebaseHandler = FindObjectOfType<FirebaseHandler>();

        // UserStateManager에서 닉네임 가져와서 nicknameCheck에 설정
        if (UserStateManager.Instance != null)
        {
            string nickname = UserStateManager.Instance.Nickname;
            if (!string.IsNullOrEmpty(nickname))
            {
                nicknameCheck.text = nickname; // TextMeshProUGUI에 닉네임 설정
                Debug.Log("닉네임 설정됨: " + nickname);
            }
            else
            {
                Debug.LogError("닉네임이 설정되지 않았습니다.");
            }
        }
        else
        {
            Debug.LogError("UserStateManager 인스턴스를 찾을 수 없습니다.");
        }

        // 버튼 이벤트 등록
        confirmButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnConfirmButtonClicked);
        cancelButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCancelButtonClicked);
    }

    void OnConfirmButtonClicked()
    {
        if (firebaseHandler != null)
        {
            // UserStateManager에서 닉네임 가져오기
            string nickname = UserStateManager.Instance.Nickname;

            if (!string.IsNullOrEmpty(nickname))
            {
                // FirebaseHandler의 회원가입 메서드 호출
                StartCoroutine(firebaseHandler.JoinUser(nickname));
                Debug.Log("민채: 회원가입 요청을 시작했습니다.");
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Debug.LogError("민채: 닉네임이 설정되지 않았습니다.");
            }
        }
        else
        {
            Debug.LogError("민채: FirebaseHandler 인스턴스를 찾을 수 없습니다.");
        }
    }

    void OnCancelButtonClicked()
    {
        // CreateNicknameScene으로 이동
        SceneManager.LoadScene("CreateNicknameScene");
        Debug.Log("민채: CreateNicknameScene 으로 이동.");
    }
}
