using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadARBattleScene()
    {
        SceneManager.LoadScene("ARBattleScene");
    }
}
