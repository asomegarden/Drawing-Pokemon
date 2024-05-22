using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour
{
    public GameObject panel;
    void Start()
    {
        Button btn = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true); 
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
