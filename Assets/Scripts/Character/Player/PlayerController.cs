using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D.Animation;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : CharacterController
{
    private void Update()
    {
        HandleInput();
    }

    protected override void HandleCollider(Collider2D collider)
    {
        
    }

    private void HandleInput()
    {
        if (isMoving) return;
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

        MoveCommand(moveDirection);
    }
}