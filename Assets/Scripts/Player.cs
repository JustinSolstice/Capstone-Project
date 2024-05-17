using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    float speed = 4f;

    public bool allowControl = true;
    public bool inBox = false;
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (allowControl)  {
            Vector2 movement = new Vector2();

            if (Input.GetKey(KeyCode.W)) movement.y += 1f/2;
            if (Input.GetKey(KeyCode.S)) movement.y -= 1f/2;
            if (Input.GetKey(KeyCode.D)) movement.x += 1;
            if (Input.GetKey(KeyCode.A)) movement.x -= 1;
            rb.MovePosition(rb.position + Time.fixedDeltaTime * movement * speed);
            if (movement != new Vector2()) {
                movementAnimations = true;
                inBox = false;
            }

            if (Input.GetKeyDown(KeyCode.RightShift)) {
                animator.Play("BoxDive");
                movementAnimations = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Ponder");
                movementAnimations = false;
            }
        }

        //this is so the player isnt set to inside the box until the animation is complete
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BoxDive") && 
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            inBox = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            GameManager.Instance.Lose();
        }
    }
}
