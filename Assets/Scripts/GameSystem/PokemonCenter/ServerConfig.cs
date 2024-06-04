using UnityEngine;

[System.Serializable]
public class ServerConfig
{
    public string serverUrl;

    public static ServerConfig LoadConfig()
    {
        TextAsset configFile = Resources.Load<TextAsset>("server_config");
        if (configFile == null)
        {
            Debug.LogError("서버 설정 파일을 로드할 수 없습니다.");
            return null;
        }

        return JsonUtility.FromJson<ServerConfig>(configFile.text);
    }
}
