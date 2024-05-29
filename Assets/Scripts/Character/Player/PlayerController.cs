using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D.Animation;

public class PlayerController : HumanController
{
    public static PlayerController Instance { get; private set; }
    private Vector3 lookDirection;
    private InteractObject currentInteractable;
    private bool hasInteract = false;
    private bool inputEnabled = true;

    public bool InputEnabled { get { return inputEnabled; } }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        if (!isMoving && inputEnabled)
        {
            if (!hasInteract) GetInteractable();
            HandleInput();
        }
    }

    protected override void HandleCollider(Collider2D collider)
    {
        
    }

    private void HandleInput()
    {
        if (currentInteractable != null)
        {
            if (Input.GetKeyDown(currentInteractable.triggerKey))
            {
                InputIndicator.Instance.HideIndicator();
                currentInteractable.TriggerInteraction();
                currentInteractable = null;
            }
        }

        Vector3 moveDirection = Vector3.zero;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) <= 0.3f) horizontalInput = 0;
        if (Mathf.Abs(verticalInput) <= 0.3f) verticalInput = 0;

        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            moveDirection = horizontalInput > 0 ? Vector3.right : Vector3.left;
        }
        else if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
        {
            moveDirection = verticalInput > 0 ? Vector3.up : Vector3.down;
        }

        if (moveDirection != Vector3.zero)
        {
            hasInteract = false;
            currentInteractable = null;
            InputIndicator.Instance.HideIndicator();
            lookDirection = moveDirection;
            MoveCommand(moveDirection);
        }
    }

    private void GetInteractable()
    {
        hasInteract = true;
        Collider2D collider = GetCollider(transform.position + lookDirection, "Interactable");

        if (collider != null)
        {
            currentInteractable = collider.GetComponent<InteractObject>();
            InputIndicator.Instance.ShowIndicator(new ActionGuide(currentInteractable.triggerKey, currentInteractable.interactionName));
        }
    }

    public void DisableInput()
    {
        inputEnabled = false;
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }
}
