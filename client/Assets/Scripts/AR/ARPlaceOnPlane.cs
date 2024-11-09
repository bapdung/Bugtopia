using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using API.Insect;
using Models.Insect.Response;
using Models.Insect.Request;
using TMPro;
using UnityEngine.UI;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager aRRaycaster;
    public GameObject foodPrefab;
    public GameObject insectPrefab;
    public InsectApi insectApi;
    public TextMeshProUGUI nicknameText;
    public TextMeshProUGUI notificationText;
    public GameObject foodIcon;
    public TextMeshProUGUI foodDescriptionText;
    public Button feedButton;

    private GameObject foodObject; // 생성된 Food 오브젝트
    private GameObject insectObject; // 생성된 Insect 오브젝트

    private InsectInfoResponse insectInfoResponse; // Insect 정보
    private IncreaseScoreResponse increaseScoreResponse; //Insect 애정도 관련 정보
    private Animator insectAnimator; // Insect의 Animator
    private bool isInsectMoving = false; // Insect가 Food로 이동 중인지 확인
    private float rotationSpeed = 2.0f; // 회전 속도

    void Awake()
    {
        if (insectApi == null)
        {
            GameObject insectApiObject = new GameObject("InsectApiObject");
            insectApi = insectApiObject.AddComponent<InsectApi>();
        }
    }

    void Start()
    {
        long raisingInsectId = 1;

        StartCoroutine(insectApi.GetInsectInfo(raisingInsectId, (response) =>
        {
            insectInfoResponse = response;
            nicknameText.text = insectInfoResponse.nickname;
            Debug.Log("지흔: insectInfoResponse.nickname: " + insectInfoResponse.nickname);

            insectPrefab = PrefabLoader.LoadInsectPrefab(insectInfoResponse.family);
            
            if (insectPrefab != null)
            {
                Debug.Log("지흔: insectPrefab 로드 성공 - family: " + insectInfoResponse.family);
                UpdateInsectObject();
            }
            else
            {
                Debug.LogError("지흔: insectPrefab이 null입니다. 프리팹 로드 실패 - family: " + insectInfoResponse.family);
            }
        },
        (error) =>
        {
            Debug.LogError("지흔: insect 정보 불러오기 실패 - " + error);
        }));
        
        ShowNotification("Tip: 곤충을 가볍게 터치해서 쓰다듬을 수 있어요!", 5f);
    }

    void Update()
    {
        if (insectObject == null)
        {
            Debug.Log("지흔: insectObject가 null입니다. UpdateInsectObject 호출 시도");
            UpdateInsectObject();
        }

        if (isInsectMoving && insectObject != null && foodObject != null)
        {
            MoveInsectTowardsFood();
        }
    }

    private void UpdateInsectObject()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (aRRaycaster.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            Pose placementPose = hits[0].pose;
            Debug.Log("지흔: 평면 발견 - 위치: " + placementPose.position + ", 회전: " + placementPose.rotation);

            if (insectObject == null)
            {
                insectObject = Instantiate(insectPrefab, placementPose.position, placementPose.rotation);
                insectAnimator = insectObject.GetComponent<Animator>();
                insectObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                var touchHandler = insectObject.AddComponent<InsectTouchHandler>();
                touchHandler.Initialize(insectApi, insectInfoResponse);

                if (insectAnimator != null)
                {
                    SetInsectIdle();
                }

                Debug.Log("지흔: Insect 오브젝트가 화면 중앙에 배치되었습니다 - 위치: " + insectObject.transform.position + ", 스케일: " + insectObject.transform.localScale);
            }
        }
        else
        {
            Debug.Log("지흔: 평면 감지 실패 - Raycast 결과 없음");
        }
    }

    private void MoveInsectTowardsFood()
    {
        if (insectAnimator != null)
        {
            insectAnimator.SetBool("walk", true);
            insectAnimator.SetBool("idle", false);
        }

        float step = 0.5f * Time.deltaTime;
        Vector3 direction = (foodObject.transform.position - insectObject.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        insectObject.transform.rotation = Quaternion.Slerp(insectObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        insectObject.transform.position = Vector3.MoveTowards(insectObject.transform.position, foodObject.transform.position, step);

        if (Vector3.Distance(insectObject.transform.position, foodObject.transform.position) < 0.4f)
        {
            isInsectMoving = false;
            SetInsectIdle();

            var increaseScoreRequest = new IncreaseScoreRequest
            {
                raisingInsectId = insectInfoResponse.raisingInsectId,
                category = 1
            };

            StartCoroutine(insectApi.PostIncreaseScore(increaseScoreRequest,
                onSuccess: (response) =>
                {
                    increaseScoreResponse = response;
                    Debug.Log("점수 증가 성공 - 애정도 총합: " + response.loveScore);
                },
                onFailure: error => Debug.LogError("점수 증가 실패: " + error)
            ));

            Destroy(foodObject);
            foodObject = null;
            ShowNotification(insectInfoResponse.nickname + "(이)가 먹이를 먹었어요!", 3f);
            ResetUIAfterFeeding();
        }
    }

    private IEnumerator SwitchToIdleAfterAttack()
    {
        Debug.Log("지흔: 3초 동안 공격 애니메이션 유지");
        yield return new WaitForSeconds(3);

        if (insectAnimator != null)
        {
            insectAnimator.SetBool("attack", false);
            SetInsectIdle();
        }
    }

    private void SetInsectIdle()
    {
        if (insectAnimator != null)
        {
            insectAnimator.SetBool("idle", true);
            insectAnimator.SetBool("walk", false);
            insectAnimator.SetBool("turnleft", false);
            insectAnimator.SetBool("turnright", false);
            insectAnimator.SetBool("flyleft", false);
            insectAnimator.SetBool("flyright", false);
            insectAnimator.SetBool("attack", false);
            insectAnimator.SetBool("hit", false);
        }
    }

    public void StartInsectMovement(GameObject newFoodObject)
    {
        foodObject = newFoodObject;
        isInsectMoving = true;
        Debug.Log("지흔: Insect가 Food로 이동을 시작합니다 - Food 위치: " + foodObject.transform.position);
    }

    private void ResetUIAfterFeeding()
    {
        foodIcon.SetActive(false);
        foodDescriptionText.gameObject.SetActive(false);
        feedButton.GetComponentInChildren<TextMeshProUGUI>().text = "지금은 배불러요!";
        feedButton.interactable = false;
        feedButton.gameObject.SetActive(true);
    }

    private void ShowNotification(string message, float duration)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        Debug.Log("지흔: 알림 표시 - 메시지: " + message);
        StartCoroutine(HideNotificationAfterDelay(duration));
    }

    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationText.gameObject.SetActive(false);
        Debug.Log("지흔: 알림 숨김");
    }
}