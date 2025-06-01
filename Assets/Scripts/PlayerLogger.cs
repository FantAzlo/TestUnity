using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class PlayerLogger : MonoBehaviour
{
    [SerializeField] private GameObject _logsMenu;
    [SerializeField] private TextMeshProUGUI _logsText;

    public void VisibilityLogs()
    {
        _logsMenu.SetActive(!_logsMenu.activeSelf);
    }

    public void AddObjectInLogs(PickableObjectLogic pickableObject)
    {
        _logsText.text += $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} --- {pickableObject.Type}\n";
    }

    private void OnApplicationQuit()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/logs.txt");
        bf.Serialize(file, _logsText.text);
        file.Close();
    }
}
