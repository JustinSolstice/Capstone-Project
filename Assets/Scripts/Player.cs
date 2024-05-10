using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    float speed = 4;

    public bool allowControl = false;
    public bool inBox = false;
    // Update is called once per frame
    Vector3 lastPosition;
    protected override void Update()
    {
        base.Update()

        if (allowControl)  {
            Vector2 movement = new Vector2();

            if (Input.GetKey(KeyCode.W)) movement.y += 1f/2;
            if (Input.GetKey(KeyCode.S)) movement.y -= 1f/2;
            if (Input.GetKey(KeyCode.D)) movement.x += 1;
            if (Input.GetKey(KeyCode.A)) movement.x -= 1;
            rb.MovePosition(rb.position + Time.deltaTime * movement * speed);
            if (movement != new Vector2()) inBox = false;

            if (Input.GetKeyDown(KeyCode.Space)) {
                animator.Play("BoxDive");
            }
            if (Input.GetKeyDown(KeyCode.Space)) animator.SetTrigger("Ponder");
        }

        //this is so the player isnt set to inside the box until the animation is complete
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BoxDive") && 
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            inBox = true;
        }
    }
}
