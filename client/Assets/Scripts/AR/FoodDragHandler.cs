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
    public GameObject foodIcon;

    private GameObject foodPreviewObject;

    void Start()
    {
        HideFoodIcon();
        feedButton.gameObject.SetActive(true);
        feedButton.GetComponentInChildren<TextMeshProUGUI>().text = "오늘의 먹이주기";
        feedButton.onClick.AddListener(ShowFoodIcon);
    }

    public void ShowFoodIcon()
    {
        foodIcon.SetActive(true);
        foodDescriptionText.gameObject.SetActive(true);
        feedButton.gameObject.SetActive(false);
    }

    public void HideFoodIcon()
    {
        foodIcon.SetActive(false);
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

            FindObjectOfType<ARPlaceOnPlane>().StartInsectMovement(placedFood);

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
