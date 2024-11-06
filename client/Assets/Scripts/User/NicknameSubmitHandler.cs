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
        Debug.Log(nicknameInputField);
        string nickname = nicknameInputField.text;
        if (nickname.Length > 0)
        {
            Debug.Log(nickname);
            SceneManager.LoadScene("checkNicknameScene");
        }
    }

}
