// PrefabLoader.cs
using UnityEngine;

public static class PrefabLoader
{
  public static GameObject LoadInsectPrefab(string family)
  {
    family = family.Replace(" ", "");
    Debug.Log("지흔: 프리팹을 로드하자");
    string prefabPath = $"Prefabs/{family}"; // family 값에 따라 경로 설정
    GameObject loadedPrefab = Resources.Load<GameObject>(prefabPath);

    if (loadedPrefab != null)
    {
      Debug.Log($"지흔: {family} 프리팹 로드 성공!");
      return loadedPrefab;
    }
    else
    {
      Debug.LogError($"지흔: {family} 프리팹 로드 실패! 경로 확인 필요.");
      return null;
    }
  }
}
