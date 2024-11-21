using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.UI;
public class ARBattleStart : MonoBehaviour
{       
    public TextMeshProUGUI countdownText;   
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public ARBattleManager battleManager;     
    private bool battleStarted = false;

    public Button battleStartButton;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        battleStartButton.onClick.AddListener(StartButtonClick);
    }

    void StartButtonClick()
    {
        battleStartButton.gameObject.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString(); 
            yield return new WaitForSeconds(1);
        }
        
        countdownText.text = "Battle Start!";
        yield return new WaitForSeconds(1);
        countdownText.text = "";
        Debug.Log("지흔: 스타트카운트다운 끝");
        StartBattle();
    }

    void StartBattle()
    {
        Debug.Log("지흔: 스타트배틀함수시작");
        if (!battleStarted)
        {
            Debug.Log("지흔: 11111");
            StartCoroutine(battleManager.StartBattleScene());
            battleStarted = true;
        }
    }
}
