using UnityEngine;

public class ModalController : MonoBehaviour
{
    public GameObject panelModal;  // 모달 패널을 참조할 변수

    // Confirm 버튼을 눌렀을 때 호출될 함수
    public void CloseModal()
    {
        if (panelModal != null)
        {
            panelModal.SetActive(false);  // 모달 패널 비활성화
        }
    }
}
