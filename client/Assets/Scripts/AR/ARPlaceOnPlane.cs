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
    public TextMeshProUGUI foodDescriptionText;
    public Button feedButton;
    public Button playButton;
    public GameObject foodIcon;
    public TextMeshProUGUI nicknameText;
    public TextMeshProUGUI notificationText;
    public FoodDragHandler foodDragHandler;

    private GameObject foodObject; // 생성된 Food 오브젝트
    private GameObject insectObject; // 생성된 Insect 오브젝트

    private InsectArInfoResponse insectInfoResponse; // Insect 정보
    private IncreaseScoreResponse increaseScoreResponse; //Insect 애정도 관련 정보
    private Animator insectAnimator; // Insect의 Animator
    private bool isInsectMoving = false; // Insect가 Food로 이동 중인지 확인
    private float rotationSpeed = 2.0f; // 회전 속도

    void Awake()
    {
        GameObject insectApiObject = new GameObject("InsectApiObject");
        insectApi = insectApiObject.AddComponent<InsectApi>();

        GameObject foodDragHandlerObject = new GameObject("foodDragHandlerObject");
        foodDragHandler = foodDragHandlerObject.AddComponent<FoodDragHandler>();
    }

    void Start()
    {
        long raisingInsectId = 1;

        StartCoroutine(insectApi.GetInsectArInfo(raisingInsectId, (response) =>
        {
            insectInfoResponse = response;
            nicknameText.text = insectInfoResponse.nickname;

            insectPrefab = PrefabLoader.LoadInsectPrefab(insectInfoResponse.family);
            
            if (insectPrefab != null)
            {
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
        
    }

    void Update()
    {
        if (insectObject == null)
        {
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

            if (insectObject == null)
            {
                insectObject = Instantiate(insectPrefab, placementPose.position, placementPose.rotation);
                insectAnimator = insectObject.GetComponent<Animator>();
                insectObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                var touchHandler = insectObject.AddComponent<InsectTouchHandler>();
                touchHandler.Initialize(insectApi, insectInfoResponse);
                touchHandler.notificationText = notificationText; 

                if (insectAnimator != null)
                {
                    SetInsectIdle();
                }

                ShowNotification("Tip: 곤충을 가볍게 터치해서 쓰다듬을 수 있어요!", 5f);
            }
        }
        else
        {
            // Debug.Log("지흔: 평면 감지 실패 - Raycast 결과 없음");
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

        if (Vector3.Distance(insectObject.transform.position, foodObject.transform.position) < 0.3f)
        {
            isInsectMoving = false;
            SetInsectEat();

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
            insectAnimator.SetBool("bite", false);
            insectAnimator.SetBool("hit", false);
        }
    }

    private void SetInsectEat()
    {
        if (insectAnimator != null)
        {
            Debug.Log("지흔 : 잠깐 멈췄다가 바로 공격");
            SetInsectIdle();
            if(insectInfoResponse.family == "Tarantula"){
                insectAnimator.SetTrigger("bite");
            } else{
                insectAnimator.SetTrigger("attack");
            }
            StartCoroutine(SwitchToIdleAfterAttack());
        }
    }

    private IEnumerator SwitchToIdleAfterAttack()
    {
        yield return new WaitForSeconds(3.0f);
        SetInsectIdle();
        Destroy(foodObject);
        foodObject = null;
        ShowNotification(insectInfoResponse.nickname + "(이)가 먹이를 먹었어요!", 3f);
        ResetUIAfterFeeding();
    }


    public void StartInsectMovement(GameObject newFoodObject)
    {
        foodObject = newFoodObject;
        isInsectMoving = true;
    }

    private void ShowNotification(string message, float duration)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        StartCoroutine(HideNotificationAfterDelay(duration));
    }

    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationText.gameObject.SetActive(false);
    }

    private void ResetUIAfterFeeding()
    {
        feedButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        foodIcon.SetActive(false);
        foodDescriptionText.gameObject.SetActive(false);
    }
}