using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    bool isFirstTime = false;
    void Start()
    {
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("화면 전환");
            if (isFirstTime)
            {
                SceneManager.LoadScene("Greeting");
            } else {
                SceneManager.LoadScene("Nickname")
            }
        }
    }
}
