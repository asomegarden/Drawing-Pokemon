using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class HumanController : MonoBehaviour
{
    public Animator animator;

    public float moveSpeed = 5f;
    protected Vector3 targetPosition;
    protected bool isMoving = false;
    protected bool isAttacking = false;

    protected bool isMoveCoroutineRunning = false;

    public void MoveCommand(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            Vector3 newPosition = this.transform.position + moveDirection;

            if (GetComponent<Collider>() == null)
            {
                if (moveDirection.x != 0 || moveDirection.y != 0)
                {
                    animator.SetFloat("dirX", moveDirection.x);
                    animator.SetFloat("dirY", moveDirection.y);
                }

                Collider2D collider = GetCollider(newPosition, "Obstacle", "Interactable");

                if (collider == null)
                {
                    targetPosition = newPosition;
                    isMoving = true;

                    animator.SetBool("isWalking", true);

                    if (!isMoveCoroutineRunning) StartCoroutine(MoveCharacterCoroutine());
                }
                else
                {
                    HandleCollider(collider);
                }
            }
        }
    }

    protected virtual void HandleCollider(Collider2D collider)
    {

    }


    protected Collider2D GetCollider(Vector3 position, params string[] layers)
    {
        return Physics2D.OverlapCircle(position, 0.1f, LayerMask.GetMask(layers));
    }


    protected IEnumerator MoveCharacterCoroutine()
    {
        isMoveCoroutineRunning = true;

        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            if ((transform.position - targetPosition).sqrMagnitude < Mathf.Epsilon)
            {
                transform.position = targetPosition;

                isMoving = false;
            }

            yield return new WaitForFixedUpdate();
        }

        if (animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", false);
        }

        isMoveCoroutineRunning = false;
    }
}
