using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractObject : MonoBehaviour
{
    public KeyCode triggerKey;
    public UnityEvent interactionEvent;

    public void TriggerInteraction()
    {
        interactionEvent.Invoke();
    }
}
