using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool movementAnimations = true;

    protected Vector2 movement;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    protected bool foward = true;
    protected bool flipped = false;
    // Update is called once per frame
    Vector2 lastPosition;
    protected virtual void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        Vector2 positionShift =  lastPosition - rb.position;
        
        updateAnim(positionShift);

        if (flipped) transform.rotation = new Quaternion(0, 180, 0, 1);
        else transform.rotation = new Quaternion(0, 0, 0, 1);

        lastPosition = rb.position;
    }

    protected void updateAnim(Vector2 direction) {
        if (animator != null && movementAnimations)
        {
            animator.SetBool("Walking", direction != Vector2.zero);
            if (direction.y > 0 && !foward)
            {
                animator.SetTrigger("Foward");
                foward = true;
            }
            else if (direction.y < 0 && foward)
            {
                animator.SetTrigger("Back");
                foward = false;
            }
            animator.SetBool("FacingFoward", foward);
        }

        if (!flipped && ((direction.x > 0 && foward) || (direction.x < 0 && !foward)))
        {
            flipped = true;
        }
        else if (flipped && ((direction.x < 0 && foward) || (direction.x > 0 && !foward)))
        {
            flipped = false;
        }
    }
}
