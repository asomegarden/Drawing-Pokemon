using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public KeyCode nextSentenceKey = KeyCode.E;
    private Queue<string> sentences;
    private UnityEvent currentDialogueEndEvent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        currentDialogueEndEvent = dialogue.dialogueEndEvent;
        PlayerController.Instance.DisableInput();
        DisplayNextSentence();

        StartCoroutine(InputCoroutine());
    }

    private IEnumerator InputCoroutine()
    {
        yield return new WaitForFixedUpdate();
        while (dialoguePanel.activeSelf)
        {
            if (Input.GetKeyDown(nextSentenceKey))
            {
                DisplayNextSentence();
            }

            yield return null;
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        PlayerController.Instance.EnableInput();
        currentDialogueEndEvent?.Invoke();
    }
}
