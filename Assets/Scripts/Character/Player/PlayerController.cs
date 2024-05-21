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
    public TextMeshProUGUI inputIndicateText;
    private bool inputEnabled = true;

    public PokemonTrainer trainer;

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
                currentInteractable.TriggerInteraction();
                currentInteractable = null;
                RefreshIndicateText();
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
            inputIndicateText.text = "";
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
            
        }
    }

    public void RefreshIndicateText()
    {
        if (currentInteractable != null)
        {
            inputIndicateText.text = $"{currentInteractable.interactionName}[{currentInteractable.triggerKey}]";
        }
        else
        {
            inputIndicateText.text = "";
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
