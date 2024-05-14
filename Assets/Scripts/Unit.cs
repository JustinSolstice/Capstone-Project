using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool movementAnimations;

    protected Vector2 movement;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        print(animator);
    }

    bool foward = true;
    bool flipped = false;
    // Update is called once per frame
    Vector2 lastPosition;
    protected virtual void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        Vector2 positionShift =  lastPosition - rb.position;
        if (animator != null && movementAnimations)
        {
            animator.SetBool("Walking", positionShift != Vector2.zero);
            if (positionShift.y > 0 && !foward)
            {
                animator.SetTrigger("Foward");
                foward = true;
            }
            else if (positionShift.y < 0 && foward)
            {
                animator.SetTrigger("Back");
                foward = false;
            }
            animator.SetBool("FacingFoward", foward);
        }


        if (!flipped && ((positionShift.x > 0 && foward) || (positionShift.x < 0 && !foward)))
        {
            flipped = true;
        }
        else if (flipped && ((positionShift.x < 0 && foward) || (positionShift.x > 0 && !foward)))
        {
            flipped = false;
        }

        if (flipped) transform.rotation = new Quaternion(0, 180, 0, 1);
        else transform.rotation = new Quaternion(0, 0, 0, 1);

        lastPosition = rb.position;
    }
}
