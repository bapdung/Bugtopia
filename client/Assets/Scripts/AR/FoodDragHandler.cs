using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FoodDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ARRaycastManager arRaycaster; // AR Raycast Manager
    public GameObject foodPrefab; // Food 프리팹
    private GameObject foodPreviewObject; // 드래그 중 미리보기로 나타날 Food 오브젝트

    // 드래그 시작 시 호출되는 함수입니다
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (foodPreviewObject == null)
        {
            // 드래그가 시작되면 Food의 미리보기 오브젝트 생성
            foodPreviewObject = Instantiate(foodPrefab);
            foodPreviewObject.SetActive(false); // 평면이 인식될 때만 활성화
        }

        Debug.Log("지흔: Food 드래그가 시작되었습니다.");
    }

    // 드래그 중일 때 호출되는 함수
    public void OnDrag(PointerEventData eventData)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        // 드래그 위치가 평면에 닿는지 Raycast로 확인
        if (arRaycaster.Raycast(eventData.position, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            // 미리보기 오브젝트를 평면에 표시
            foodPreviewObject.SetActive(true);
            foodPreviewObject.transform.position = hitPose.position;
            foodPreviewObject.transform.rotation = hitPose.rotation;

            // Debug.Log("지흔: Food가 평면 미리보기로 표시 중...");
        }
        else
        {
            // 평면이 감지되지 않으면 미리보기 오브젝트 비활성화
            foodPreviewObject.SetActive(false);
        }
    }

    // 드래그가 끝났을 때 호출되는 함수
    public void OnEndDrag(PointerEventData eventData)
    {
        if (foodPreviewObject != null && foodPreviewObject.activeSelf)
        {
            // 미리보기가 활성화된 상태라면 그 위치에 Food를 최종 배치
            GameObject placedFood = Instantiate(foodPrefab, foodPreviewObject.transform.position, foodPreviewObject.transform.rotation);
            Debug.Log("지흔: Food가 평면에 최종 배치되었습니다!");

            // ARPlaceOnPlane의 StartBugMovement 호출
            FindObjectOfType<ARPlaceOnPlane>().StartInsectMovement(placedFood);

            // 미리보기 오브젝트 삭제
            Destroy(foodPreviewObject);
            foodPreviewObject = null;
        }
        else
        {
            Debug.Log("지흔: 드래그가 끝났지만 평면에 Food를 배치할 수 없습니다.");
        }
    }

    public void TestFunction()
    {
        Debug.Log("TestFunction 호출됨");
    }

    public void OnBeginDragWrapper()
    {
        OnBeginDrag(new PointerEventData(EventSystem.current));
    }

    public void OnDragWrapper()
    {
        OnDrag(new PointerEventData(EventSystem.current));
    }

    public void OnEndDragWrapper()
    {
        OnEndDrag(new PointerEventData(EventSystem.current));
    }
}
