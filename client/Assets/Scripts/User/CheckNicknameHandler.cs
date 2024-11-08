using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckNicknameHandler : MonoBehaviour
{
    public GameObject confirmButton;
    public GameObject cancelButton;
    private FirebaseHandler firebaseHandler;
    void Start()
    {
        // FirebaseHandler 인스턴스 찾기
        firebaseHandler = FindObjectOfType<FirebaseHandler>();

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
                Debug.Log("회원가입 요청을 시작했습니다.");
            }
            else
            {
                Debug.LogError("닉네임이 설정되지 않았습니다.");
            }
        }
        else
        {
            Debug.LogError("FirebaseHandler 인스턴스를 찾을 수 없습니다.");
        }
    }

    void OnCancelButtonClicked()
    {
        // CreateNicknameScene으로 이동
        SceneManager.LoadScene("CreateNicknameScene");
        Debug.Log("CreateNicknameScene 으로 이동.");
    }
}
