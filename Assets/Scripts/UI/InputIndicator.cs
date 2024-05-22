using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InputIndicator : MonoBehaviour
{
    public static InputIndicator Instance { get; private set; }
    public TextMeshProUGUI inputIndicateText;

    public List<ActionGuide> defaultActionGuides = new List<ActionGuide>();

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        ShowIndicator();
    }

    public void ShowIndicator(params ActionGuide[] guides)
    {
        string contetnt = string.Empty;

        foreach(ActionGuide guide in defaultActionGuides)
        {
            contetnt += $" {guide.actionName}[{guide.key}]";
        }

        foreach (ActionGuide guide in guides)
        {
            contetnt += $" {guide.actionName}[{guide.key}]";
        }

        inputIndicateText.text = contetnt;
    }

    public void ShowIndicatorNoDefault(params ActionGuide[] guides)
    {
        string contetnt = string.Empty;

        foreach (ActionGuide guide in guides)
        {
            contetnt += $" {guide.actionName}[{guide.key}]";
        }

        inputIndicateText.text = contetnt;
    }

    public void HideIndicator()
    {
        ShowIndicator();
    }

    public void HideAllIndicator()
    {
        inputIndicateText.text = string.Empty;
    }
}
