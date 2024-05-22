using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionGuide
{
    public KeyCode key;
    public string actionName;

    public ActionGuide(KeyCode key, string actionName)
    {
        this.key = key;
        this.actionName = actionName;
    }
}
