using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckNicknameHandler : MonoBehaviour
{
    public GameObject confirmButton;
    public GameObject cancelButton;
    void Start()
    {
        confirmButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnConfirmButtonClicked);
        cancelButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCancelButtonClicked);
    }
    void OnConfirmButtonClicked()
    {
        // GrettingScene으로 이동
        SceneManager.LoadScene("GreetingScene");
    }

    void OnCancelButtonClicked()
    {
        // CreateNicknameScene으로 이동
        SceneManager.LoadScene("CreateNicknameScene");
    }
}
