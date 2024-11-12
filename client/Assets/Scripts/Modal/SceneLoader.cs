using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // UI Image 오브젝트를 연결
    public Image eventModalImage; // Inspector에서 연결

    private void Start()
    {
        // Image 컴포넌트의 모서리를 33px로 둥글게 설정 (스프라이트가 9-slice로 설정되어 있어야 함)
        if (eventModalImage != null)
        {
            eventModalImage.type = Image.Type.Sliced; // 9-slice가 설정된 스프라이트여야 작동
        }
    }

    public void LoadARBattleScene()
    {
        SceneManager.LoadScene("ARBattleScene");
    }
}
