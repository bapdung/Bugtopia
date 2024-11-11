using System.Collections;
using UnityEngine;
using TMPro;

public class ARBattleManager : MonoBehaviour
{
    public GameObject prefab1;          
    public GameObject prefab2;          

    private GameObject instance1;       
    private GameObject instance2;       

    private int health1 = 100;         
    private int health2 = 100;          

    public void StartBattleScene()
    {
        // 두 캐릭터를 AR 평면 위에 배치합니다
        Vector3 position1 = new Vector3(-0.5f, 0, 0);
        Vector3 position2 = new Vector3(0.5f, 0, 0);  

        instance1 = Instantiate(prefab1, position1, Quaternion.identity);
        instance2 = Instantiate(prefab2, position2, Quaternion.identity);

        // 전투 시작
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        Debug.Log("지흔: 전투가 시작했음");
        // 전투 로직
        yield return null;
    }
}
