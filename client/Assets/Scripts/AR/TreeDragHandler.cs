using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

public class TreeDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ARRaycastManager arRaycaster; // AR Raycast Manager
    public GameObject treePrefab; // Tree 프리팹
    public TextMeshProUGUI treeDescriptionText;
    public Button feedButton;
    public Button playButton;
    public GameObject treeIcon;

    private GameObject treePreviewObject;

    void Start()
    {
        HideTreeIcon();
        feedButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        feedButton.onClick.AddListener(ShowTreeIcon);
    }

    public void ShowTreeIcon()
    {
        Debug.Log("지흔: ShowtreeIcon");
        treeIcon.SetActive(true);
        treeDescriptionText.gameObject.SetActive(true);
        feedButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    public void HideTreeIcon()
    {
        Debug.Log("지흔: UI 초기화");
        treeIcon.SetActive(false);
        treeDescriptionText.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (treePreviewObject == null)
        {
            treePreviewObject = Instantiate(treePrefab);
            treePreviewObject.SetActive(false);
        }

        Debug.Log("지흔: Tree 드래그가 시작되었습니다.");
    }

    public void OnDrag(PointerEventData eventData)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (arRaycaster.Raycast(eventData.position, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            treePreviewObject.SetActive(true);
            treePreviewObject.transform.position = hitPose.position;
            treePreviewObject.transform.rotation = hitPose.rotation;
        }
        else
        {
            treePreviewObject.SetActive(false);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (treePreviewObject != null && treePreviewObject.activeSelf)
        {
            GameObject placedTree = Instantiate(treePrefab, treePreviewObject.transform.position, treePreviewObject.transform.rotation);
            Debug.Log("지흔: Tree가 평면에 최종 배치되었습니다!");

            FindObjectOfType<ARPlaceOnPlane>().StartInsectMovement(placedTree);

            Destroy(treePreviewObject);
            treePreviewObject = null;
        }
        else
        {
            Debug.Log("지흔: 드래그가 끝났지만 평면에 Tree를 배치할 수 없습니다.");
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
