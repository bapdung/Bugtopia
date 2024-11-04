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
                Debug.Log("지흔: Insect가 화면 중앙에 배치되었습니다!");
            }
        }
    }

    // Food을 외부에서 생성한 경우 이 메서드를 호출하여 Insect가 이동을 시작하게 함
    public void CreateFood(Vector3 position)
    {
        foodObject = Instantiate(foodPrefab, position, Quaternion.identity);
        Debug.Log("지흔: Food이 생성되었습니다.");

        if (insectObject != null)
        {
            InsectController insectController = insectObject.GetComponent<InsectController>();
            if (insectController != null)
            {
                insectController.StartInsectMovement(foodObject); // Insect가 Food로 이동 시작
            }
        }
    }
}
