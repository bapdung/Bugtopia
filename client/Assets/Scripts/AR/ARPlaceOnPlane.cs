using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager aRRaycaster; // AR Raycast Manager를 참조하여 평면에 대한 레이캐스팅 수행
    public GameObject foodPrefab; // 평면에 배치할 Food 오브젝트
    public GameObject insectPrefab; // 화면 중앙에 배치할 Insect 오브젝트

    private GameObject foodObject; // 생성된 Food 오브젝트
    private GameObject insectObject; // 생성된 Insect 오브젝트
    private Animator insectAnimator; // Insect의 Animator
    private bool isInsectMoving = false; // Insect가 Food로 이동 중인지 확인
    private float rotationSpeed = 2.0f; // 회전 속도

    void Start()
    {
        // 초기화 작업: 평면 인식 후 중앙에 Insect 배치 시도
        UpdateCenterObject();
    }

    void Update()
    {
        // 평면 중앙에 Insect 배치
        if (insectObject == null)
        {
            UpdateCenterObject();
        }

        // Insect가 Food를 향해 이동
        if (isInsectMoving && insectObject != null && foodObject != null)
        {
            MoveInsectTowardsFood();
        }
    }

    // 화면 중앙에 Insect 오브젝트를 배치하는 함수
    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (aRRaycaster.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            Pose placementPose = hits[0].pose;
            if (insectObject == null) // Insect가 없는 상태일 때만 생성
            {
                insectObject = Instantiate(insectPrefab, placementPose.position, placementPose.rotation);
                insectAnimator = insectObject.GetComponent<Animator>(); // Animator 초기화
                if (insectAnimator != null)
                {
                    insectAnimator.SetBool("idle", true); // 초기 상태를 idle로 설정
                }
                Debug.Log("지흔: Insect가 화면 중앙에 배치되었습니다!");
            }
        }
    }

    // Insect가 Food를 향해 이동하는 함수
    private void MoveInsectTowardsFood()
    {
        if (insectAnimator != null)
        {
            // 이동 중일 때 walk 애니메이션 활성화
            insectAnimator.SetBool("walk", true);
            insectAnimator.SetBool("idle", false);
        }

        float step = 0.5f * Time.deltaTime; // 이동 속도 조정

        // 이동 방향 계산
        Vector3 direction = (foodObject.transform.position - insectObject.transform.position).normalized;

        // Insect의 회전 설정 (부드럽게 회전)
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        insectObject.transform.rotation = Quaternion.Slerp(insectObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Insect 오브젝트의 위치를 Food 오브젝트의 위치로 이동
        insectObject.transform.position = Vector3.MoveTowards(insectObject.transform.position, foodObject.transform.position, step);

        Debug.Log("지흔: 이동 후 Insect 위치 - " + insectObject.transform.position);

        // 이동 후 Insect와 Food의 거리가 충분히 가까워졌을 때 충돌로 간주
        if (Vector3.Distance(insectObject.transform.position, foodObject.transform.position) < 0.1f)
        {
            Debug.Log("지흔: Food를 먹었습니다!");

            // Food 제거 및 상태 업데이트
            Destroy(foodObject); // Food 제거
            isInsectMoving = false; // 이동 중지
            foodObject = null; // Food 참조 제거

            Debug.Log("지흔: 점수 3점 증가");
            Debug.Log("쿨타임 발동 및 먹이 횟수 증가");
            Debug.Log("foodIcon 버튼 비활성화");

            // 이동 중지 후 idle 애니메이션으로 전환
            if (insectAnimator != null)
            {
                insectAnimator.SetBool("walk", false);
                insectAnimator.SetBool("idle", true);
            }
        }
    }

    // Food을 외부에서 생성한 경우 이 메서드를 호출하여 Insect가 이동을 시작하게 함
    public void StartInsectMovement(GameObject newFoodObject)
    {
        foodObject = newFoodObject;
        isInsectMoving = true;
        Debug.Log("지흔: Insect가 Food로 이동을 시작합니다.");
    }
}