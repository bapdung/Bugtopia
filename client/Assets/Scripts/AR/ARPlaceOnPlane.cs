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
    public GameObject treePrefab;
    public GameObject insectPrefab;
    public InsectApi insectApi;
    public TextMeshProUGUI foodDescriptionText;
    public Button feedButton;
    public Button playButton;
    public GameObject foodIcon;
    public GameObject treeIcon;
    public TextMeshProUGUI nicknameText;
    public TextMeshProUGUI notificationText;
    public FoodDragHandler foodDragHandler;
    public TreeDragHandler treeDragHandler;

    private GameObject foodObject; 
    private GameObject treeObject;
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

        GameObject treeDragHandlerObject = new GameObject("treeDragHandlerObject");
        treeDragHandler = treeDragHandlerObject.AddComponent<TreeDragHandler>();
    }

    void Start()
    {
        feedButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        foodIcon.SetActive(false);
        treeIcon.SetActive(false);
        foodDescriptionText.gameObject.SetActive(false);

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

        if (isInsectMoving && insectObject != null)
        {
            if (foodObject != null)
            {
                MoveInsectTowardsFood();
            }
            else if (treeObject != null)
            {
                MoveInsectTowardsTree();
            }
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
                touchHandler.Initialize(insectApi, insectInfoResponse, insectAnimator);
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

    private void MoveInsectTowardsTree()
    {
        if (insectAnimator != null)
        {
            insectAnimator.SetBool("walk", true);
            insectAnimator.SetBool("idle", false);
        }

        float step = 0.5f * Time.deltaTime;
        Vector3 direction = (treeObject.transform.position - insectObject.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        insectObject.transform.rotation = Quaternion.Slerp(insectObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        insectObject.transform.position = Vector3.MoveTowards(insectObject.transform.position, treeObject.transform.position, step);

        if (Vector3.Distance(insectObject.transform.position, treeObject.transform.position) < 4f)
        {
            Debug.Log("지흔: 나무와의 거리 4f 이하, 트리 꼭대기로 바로 이동 시작");
            isInsectMoving = false;
            SetInsectIdle();

            StartCoroutine(MoveToTopOfTree());
        }
    }

    private IEnumerator MoveToTopOfTree()
    {
        Debug.Log("지흔: 트리 꼭대기로 올라가는 애니메이션 시작");

        if (insectAnimator != null)
        {
            yield return new WaitForSeconds(0.5f);

            SetInsectTakeOff();
            Debug.Log("지흔: takeoff 애니메이션 실행");

            yield return new WaitForSeconds(0.5f);

            insectAnimator.SetBool("takeoff", false);
            insectAnimator.SetTrigger("fly");
            Debug.Log("지흔: fly 애니메이션 실행 및 트리 꼭대기 위치로 이동 시작");

            Vector3 topPosition = treeObject.transform.position + new Vector3(0, 2.0f, 0);
            float step = 0.5f * Time.deltaTime;

            // 꼭대기까지 이동하는 동안 fly 애니메이션 유지 및 나무 방향으로 회전
            while (true)
            {
                // 나무 방향을 바라보도록 회전 조절
                Vector3 direction = (topPosition - insectObject.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                insectObject.transform.rotation = Quaternion.Slerp(insectObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // 트리 꼭대기 위치로 이동
                insectObject.transform.position = Vector3.MoveTowards(insectObject.transform.position, topPosition, step);

                // X, Y 좌표가 목표 위치와 가까워지면 착륙 조건을 충족
                if (Mathf.Abs(insectObject.transform.position.x - topPosition.x) < 0.05f &&
                    Mathf.Abs(insectObject.transform.position.y - topPosition.y) < 0.05f)
                {
                    Debug.Log("지흔: X, Y 좌표가 목표 위치와 일치하여 landing 애니메이션 시작");
                    insectAnimator.SetBool("fly", false);
                    insectAnimator.SetTrigger("landing");
                    break;
                }

                yield return null;
            }

            Debug.Log("지흔: 트리 꼭대기 도착, landing 애니메이션 시작");
            yield return new WaitForSeconds(1.5f); // 충분한 지연 시간으로 fly 애니메이션이 완료되도록 함
            insectAnimator.SetBool("fly", false);
            insectAnimator.SetTrigger("landing");

            yield return new WaitForSeconds(0.5f);
            insectAnimator.SetBool("landing", false);
            insectAnimator.SetBool("idle", true);
            SetInsectIdle();
            Debug.Log("지흔: landing 완료 후 idle 상태로 전환");

            ResetUIAfterPlaying();

            //TODO : 여기까지는 못했어 미안해요 여러분 원래는 떨어지는 모션까지 완성하고자 함
            // yield return new WaitForSeconds(2.0f);
            // Debug.Log("지흔: 2초 후 지면으로 떨어지는 모션 시작");
            // // StartCoroutine(FallToGround());
        }
    }

    private IEnumerator FallToGround()
    {
        Debug.Log("지흔: 지면으로 떨어지기 위해 곤충 뒤집힘");
        Quaternion flippedRotation = Quaternion.Euler(180, insectObject.transform.eulerAngles.y, 0);
        insectObject.transform.rotation = flippedRotation;

        Vector3 groundPosition = treeObject.transform.position + new Vector3(0, -0.5f, 0);
        float fallSpeed = 1.0f;

        Debug.Log("지흔: 떨어지는 중...");
        while (Vector3.Distance(insectObject.transform.position, groundPosition) > 0.05f)
        {
            insectObject.transform.position = Vector3.MoveTowards(insectObject.transform.position, groundPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("지흔: 지면에 도착, idle 상태로 전환 및 원래 자세 복원");
        insectAnimator.SetBool("idle", true);
        insectObject.transform.rotation = Quaternion.Euler(0, insectObject.transform.eulerAngles.y, 0);
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
            Debug.Log("지흔: 곤충 상태 idle로 설정");
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

    private void SetInsectTakeOff()
    {
        if (insectAnimator != null)
        {
            Debug.Log("지흔 : 날아갈 준비 takeOff");
            SetInsectIdle();
            if(insectInfoResponse.family == "Tarantula"){
                insectAnimator.SetTrigger("bite");
            } else{
                insectAnimator.SetTrigger("takeoff");
            }
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

    public void StartInsectMovement(GameObject targetObject, bool isFood)
    {
        isInsectMoving = true;

        if (isFood)
        {
            foodObject = targetObject;
            treeObject = null; // 나무 이동을 중지
        }
        else
        {
            treeObject = targetObject;
            foodObject = null; // 음식 이동을 중지
        }
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

    private void ResetUIAfterPlaying()
    {
        feedButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        treeIcon.SetActive(false);
        foodDescriptionText.gameObject.SetActive(false);
    }
}