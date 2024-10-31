using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startToTouch : MonoBehaviour
{
    private string mainScene = "MainScene";
    private string nicknameScene = "NickNameScene";

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
            SceneManager.LoadScene(nicknameScene);
        }
    }
}
