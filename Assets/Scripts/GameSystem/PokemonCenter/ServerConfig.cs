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
            Debug.LogError("���� ���� ������ �ε��� �� �����ϴ�.");
            return null;
        }

        return JsonUtility.FromJson<ServerConfig>(configFile.text);
    }
}
