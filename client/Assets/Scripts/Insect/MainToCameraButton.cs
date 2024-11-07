using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainToCameraButton : MonoBehaviour
{
    public GameObject cameraButton;
    void Awake()	
    {
        cameraButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnCameraButtonClicked);
    }
    void OnCameraButtonClicked()
    {
        SceneManager.LoadScene("CameraScene");
    }
}
