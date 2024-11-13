using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentConfig", menuName = "Config/EnvironmentConfig")]
public class EnvironmentConfig : ScriptableObject
{
    public string baseUrl;
    public string s3Url;
}
