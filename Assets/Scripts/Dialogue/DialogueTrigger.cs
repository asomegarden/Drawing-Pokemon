using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        Debug.Log("대화 시작");
        //DialogueManager.Instance.StartDialogue(dialogue);
    }
}
