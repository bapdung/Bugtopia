using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

public class FoodDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ARRaycastManager arRaycaster; // AR Raycast Manager
    public GameObject foodPrefab; // Food 프리팹
    public TextMeshProUGUI foodDescriptionText;
    public Button feedButton;
    public Button playButton;
    public GameObject foodIcon;
    public GameObject treeIcon;

    private GameObject foodPreviewObject;

    void Start()
    {
    }

    public void ShowFoodIcon()
    {
        Debug.Log("지흔: ShowFoodIcon");
        foodIcon.SetActive(true);
        treeIcon.SetActive(false);
        foodDescriptionText.gameObject.SetActive(true);
        feedButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        foodDescriptionText.text = "먹이를 드래그해서 평면에 놓아주세요!";
    }

    public void HideFoodIcon()
    {
        Debug.Log("지흔: UI 초기화");
        foodIcon.SetActive(false);
        treeIcon.SetActive(false);
        foodDescriptionText.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (foodPreviewObject == null)
        {
            foodPreviewObject = Instantiate(foodPrefab);
            foodPreviewObject.SetActive(false);
        }

        Debug.Log("지흔: Food 드래그가 시작되었습니다.");
    }

    public void OnDrag(PointerEventData eventData)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (arRaycaster.Raycast(eventData.position, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            foodPreviewObject.SetActive(true);
            foodPreviewObject.transform.position = hitPose.position;
            foodPreviewObject.transform.rotation = hitPose.rotation;
        }
        else
        {
            foodPreviewObject.SetActive(false);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (foodPreviewObject != null && foodPreviewObject.activeSelf)
        {
            GameObject placedFood = Instantiate(foodPrefab, foodPreviewObject.transform.position, foodPreviewObject.transform.rotation);
            Debug.Log("지흔: Food가 평면에 최종 배치되었습니다!");

            FindObjectOfType<ARPlaceOnPlane>().StartInsectMovement(placedFood, true);

            Destroy(foodPreviewObject);
            foodPreviewObject = null;
        }
        else
        {
            Debug.Log("지흔: 드래그가 끝났지만 평면에 Food를 배치할 수 없습니다.");
        }
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
