using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public GameObject forestContent;
    public GameObject waterContent;
    public GameObject gardenContent;

    public Button forestButton;
    public Button waterButton;
    public Button gardenButton;

    private void Start()
    {
        forestButton.onClick.AddListener(() => ShowTabContent(forestContent));
        waterButton.onClick.AddListener(() => ShowTabContent(waterContent));
        gardenButton.onClick.AddListener(() => ShowTabContent(gardenContent));

        ShowTabContent(forestContent);
    }

    private void ShowTabContent(GameObject tabContent)
    {
        forestContent.SetActive(false);
        waterContent.SetActive(false);
        gardenContent.SetActive(false);

        tabContent.SetActive(true);
    }
}
