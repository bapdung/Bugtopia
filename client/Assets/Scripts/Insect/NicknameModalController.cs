using UnityEngine;
using TMPro;
using API.Insect;
using UnityEngine.UI; 

public class NicknameModalController : MonoBehaviour
{
    public TMP_InputField nicknameInputField; // InputField에 대한 참조
    private string nickname; // 입력받은 닉네임을 저장할 변수
    public Button setNicknameButton; // Button 오브젝트에 대한 참조
    public InsectApi insectApi; // InsectApi 스크립트에 대한 참조
    private long insectId = 8; // 임의로 8로 설정

    void Start()
    {
        nickname = ""; // 닉네임 초기화

        if (insectApi == null)
        {
          GameObject insectApiObject = new GameObject("InsectApiObject");
          insectApi = insectApiObject.AddComponent<InsectApi>();

        }

        // 버튼 클릭 이벤트에 SetNickname 메서드 연결
        if (setNicknameButton != null)
        {
            setNicknameButton.onClick.AddListener(SetNickname);
        }
        else
        {
            Debug.LogError("민채: SetNicknameButton이 연결되지 않았습니다!");
        }
    }

    // 닉네임 결정 버튼 클릭 시 호출할 메서드
    public void SetNickname()
    {
        nickname = nicknameInputField.text; // InputField의 텍스트 값을 가져와 저장
        Debug.Log("민채: 닉네임 설정! => " + nickname);
        
        // RaiseInsect 호출
        StartCoroutine(insectApi.RaiseInsect(
            insectId,
            nickname,
            onSuccess: response =>
            {
                Debug.Log("민채: RaiseInsect 성공: " + response);
                // 성공 시 처리
            },
            onFailure: error =>
            {
                Debug.LogError("민채: RaiseInsect 실패: " + error);
                // 실패 시 처리
            }
        ));
    }
}
