using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager aRRaycaster; // AR Raycast Manager를 참조하여 평면에 대한 레이캐스팅 수행
    public GameObject mealPrefab; // 평면에 배치할 Meal 오브젝트
    public GameObject bugPrefab; // 화면 중앙에 배치할 Bug 오브젝트

    private GameObject mealObject; // 생성된 Meal 오브젝트
    private GameObject bugObject; // 생성된 Bug 오브젝트

    void Start()
    {
        // 초기화 작업: 평면 인식 후 중앙에 Bug 배치 시도
        UpdateCenterObject();
    }

    void Update()
    {
        // 평면 중앙에 Bug 배치
        if (bugObject == null)
        {
            UpdateCenterObject();
        }
    }

    // 화면 중앙에 Bug 오브젝트를 배치하는 함수
    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (aRRaycaster.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            Pose placementPose = hits[0].pose;
            if (bugObject == null) // Bug가 없는 상태일 때만 생성
            {
                bugObject = Instantiate(bugPrefab, placementPose.position, placementPose.rotation);
                Debug.Log("지흔: Bug가 화면 중앙에 배치되었습니다!");
            }
        }
    }

    // Meal을 외부에서 생성한 경우 이 메서드를 호출하여 Bug가 이동을 시작하게 함
    public void CreateMeal(Vector3 position)
    {
        mealObject = Instantiate(mealPrefab, position, Quaternion.identity);
        Debug.Log("지흔: Meal이 생성되었습니다.");

        if (bugObject != null)
        {
            BugController bugController = bugObject.GetComponent<BugController>();
            if (bugController != null)
            {
                bugController.StartBugMovement(mealObject); // Bug가 Meal로 이동 시작
            }
        }
    }
}
