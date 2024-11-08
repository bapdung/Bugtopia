using UnityEngine;
using UnityEngine.UI;
using API.Catch;
using Models.Insect.Response;

public class EntryScanManager : MonoBehaviour
{
    public Text insectNameText;
    public Text descriptionText;
    public Text familyText;
    public Text areaText;
    public Text rejectedReasonText;

    public void UpdateInsectInfo(SearchInsectResponse response)
    {
        // 결과 화면에 표시
        if (insectNameText != null)
        {
            insectNameText.text = "곤충명 (한글): " + response.krName + "\n" + "곤충명 (영어): " + response.engName;
        }
        if (descriptionText != null)
        {
            descriptionText.text = "곤충 정보: " + response.info;
        }
        if (familyText != null)
        {
            familyText.text = "곤충 종류: " + response.family;
        }
        if (areaText != null)
        {
            areaText.text = "서식지: " + response.area;
        }
        if (rejectedReasonText != null)
        {
            rejectedReasonText.text = response.rejectedReason != null ? "육성 불가능 사유: " + response.rejectedReason : "육성 가능";
        }
    }
}
