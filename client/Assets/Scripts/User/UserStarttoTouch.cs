using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserStarttoTouch : MonoBehaviour
{
    private string mainScene = "GreetingScene";
    private string nickNameScene = "CreateNicknameScene";

    // TODO : 이거 API 요청을 하든 기기 정보를 받든 해서 바꿀것!! 민채 파이팅!
    private bool isRegistered = false;


    void Start()
    {
        
    }
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonUp(0))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (isRegistered)
        {
            SceneManager.LoadScene(mainScene);
        }
        else
        {
            SceneManager.LoadScene(nickNameScene);
        }
    }

}
