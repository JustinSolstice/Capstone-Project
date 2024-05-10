using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public bool allowMovement = false;
    bool foward = true;
    bool flipped = false;
    // Update is called once per frame
    Vector3 lastPosition;
    void Update()
    {
        lastPosition = rb.position

        if (allowMovement)  {
            Vector2 movement = new Vector2();

            if (Input.GetKey(KeyCode.W)) movement.y += 1f/2;
            if (Input.GetKey(KeyCode.S)) movement.y -= 1f/2;
            if (Input.GetKey(KeyCode.D)) movement.x += 1;
            if (Input.GetKey(KeyCode.A)) movement.x -= 1;
            rb.MovePosition(rb.position + Time.deltaTime * movement * speed);
        }

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

        rb.velocity = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Space)) animator.Play("BoxDive");
        if (Input.GetKeyDown(KeyCode.Return)) animator.SetTrigger("Ponder");
    }
}
