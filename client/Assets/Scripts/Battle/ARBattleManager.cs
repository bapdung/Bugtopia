using System.Collections;
using UnityEngine;
using TMPro;
using Models.Insect.Response;
public class ARBattleManager : MonoBehaviour
{
    public GameObject prefab1;          
    public GameObject prefab2;          

    private GameObject instance1;       
    private GameObject instance2;       

    public GameObject battleEndPanel;
    private int health1 = 100;         
    private int health2 = 100;          

    void Start()
    {
        if (battleEndPanel != null)
        {
            battleEndPanel.SetActive(false);
        }
    }

    public IEnumerator StartBattleScene()
    {
        Debug.Log("지흔: 22222");

        // API 요청 전 임시 하드 코딩
        if (string.IsNullOrEmpty(family))
        {
            family = "Stagbeetle";
        }
        if (insectId == 0)
        {
            insectId = 6;
        }
        if (string.IsNullOrEmpty(eventType))
        {
            eventType = "FOOD_C1";
        }

        prefab1 = PrefabLoader.LoadInsectPrefab(family);
        prefab2 = PrefabLoader.LoadInsectPrefab(family);

        // 두 캐릭터 배치
        Vector3 position1 = new Vector3(-0.5f, 0, 4f);
        Vector3 position2 = new Vector3(0.5f, 0, 4f);  

        instance1 = Instantiate(prefab1, position1, Quaternion.identity);
        instance2 = Instantiate(prefab2, position2, Quaternion.identity);

        instance1.transform.LookAt(Camera.main.transform.position);
        instance2.transform.LookAt(Camera.main.transform.position);
        Debug.Log("지흔: 33333");
        yield return new WaitForSeconds(2);

        StartCoroutine(RotateTowardsTarget(instance1, instance2.transform.position));
        StartCoroutine(RotateTowardsTarget(instance2, instance1.transform.position));

        // 전투 시작
        StartCoroutine(StartBattle());
    }

    // 오브젝트 시선처리
    private float rotationSpeed = 5f; // 회전 속도
    IEnumerator RotateTowardsTarget(GameObject obj, Vector3 targetPosition)
    {
        while (true)
        {
            // 목표 방향으로 회전
            Vector3 direction = targetPosition - obj.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 회전 속도에 맞게 회전
            obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 회전이 완료되면 종료
            if (Quaternion.Angle(obj.transform.rotation, targetRotation) < 1f)
            {
                break;
            }
            yield return null;
        }
    }


    // 정보 받아오기
    private long insectId;
    private string eventType;
    private string family;
    public void Initialize(InsectArInfoResponse infoResponse, string getEventType)
    {
        insectId = infoResponse.raisingInsectId;
        family = infoResponse.family;
        eventType = getEventType;
    }

    IEnumerator StartBattle()
    {
        Debug.Log("지흔: 전투가 시작했음");
        // 애니메이션 페이즈 1
        Debug.Log("지흔: 페이즈 1");
        // 터치이벤트 발생
        Debug.Log("지흔: 터치이벤트");
        // 애니메이션 페이즈 2
        Debug.Log("지흔: 페이즈 2");
        // 전투 종료
        Debug.Log("지흔: 전투 끝");
        
        if (battleEndPanel != null)
        {
            battleEndPanel.SetActive(true);
        }
        yield return null;
    }
}
