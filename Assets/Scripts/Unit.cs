using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    bool foward = true;
    bool flipped = false;
    // Update is called once per frame
    Vector3 lastPosition;
    protected virtual void Update()
    {
        lastPosition = rb.position
        rb.velocity = Vector2.zero;
    }

    protected virtual void LateUpdate()
    {
        Vector2 positionShift = rb.position - lastPosition
        animator.SetBool("Walking", positionShift.x != 0 || positionShift.y != 0);

        if (positionShift.y < 0 && !foward)
        {
            animator.SetTrigger("Foward");
            foward = true;
        }
        else if (positionShift.y > 0 && foward)
        {
            animator.SetTrigger("Back");
            foward = false;
        }
        animator.SetBool("FacingFoward", foward);

        if (!flipped && ((positionShift.x < 0 && foward) || (positionShift.x > 0 && !foward)))
        {
            flipped = true;
        }
        else if (flipped && ((positionShift.x > 0 && foward) || (positionShift.x < 0 && !foward)))
        {
            flipped = false;
        }

        if (flipped) transform.rotation = new Quaternion(0, 180, 0, 1);
        else transform.rotation = new Quaternion(0, 0, 0, 1);
    }
}
