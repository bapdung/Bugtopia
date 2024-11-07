using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public InsectListController insectListController;
    public Button forestButton;
    public Button waterButton;
    public Button gardenButton;

    private void Start()
    {
        // 탭 버튼 클릭 시 이벤트 추가
        forestButton.onClick.AddListener(() => ShowTabContent("FOREST"));
        waterButton.onClick.AddListener(() => ShowTabContent("WATER"));
        gardenButton.onClick.AddListener(() => ShowTabContent("GARDEN"));

        // 기본 탭 콘텐츠 설정 (예: Forest)
        ShowTabContent("FOREST");
    }

    private void ShowTabContent(string region)
    {
        // 탭에 맞는 곤충 리스트를 로드하도록 요청
        insectListController.LoadInsectList(region);
    }
}
