using System;
using System.Collections.Generic;
using UnityEngine.Events;

[Serializable]
public class Dialogue
{
    public List<string> sentences;
    public UnityEvent dialogueEndEvent;
}
