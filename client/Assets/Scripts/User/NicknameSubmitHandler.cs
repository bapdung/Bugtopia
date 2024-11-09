using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class nicknameSubmitHandler : MonoBehaviour
{
    public Button nicknameSubmitButton;
    public TMP_InputField nicknameInputField;
    
    void Start()
    {
        nicknameSubmitButton.onClick.AddListener(onClickButton);
    }

    void onClickButton()
    {
        // 입력 필드에서 닉네임을 가져옴
        string nickname = nicknameInputField.text;

        if (nickname.Length > 0 && nickname.Length < 13)
        {
            // 닉네임이 유효하면 전역 공간에 저장
            UserStateManager.Instance.SetNickname(nickname);
            Debug.Log("민채: 전역 공간에 설정된 닉네임: " + nickname);

            // checkNicknameScene으로 이동
            SceneManager.LoadScene("checkNicknameScene");
        }
        else
        {
            Debug.Log("민채: 닉네임이 입력되지 않았습니다.");
        }
    }
}
