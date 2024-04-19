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

    bool foward = true;
    bool flipped = false;
    // Update is called once per frame
    void Update()
    {

        Vector2 movement = new Vector2();
        if (Input.GetKey(KeyCode.W)) movement.y += 1f/2;
        if (Input.GetKey(KeyCode.S)) movement.y -= 1f/2;
        if (Input.GetKey(KeyCode.D)) movement.x += 1;
        if (Input.GetKey(KeyCode.A)) movement.x -= 1;

        rb.MovePosition(rb.position + Time.deltaTime * movement * speed);
        animator.SetBool("Walking", movement.x != 0 || movement.y != 0);

        if (movement.y < 0 && !foward)
        {
            animator.SetTrigger("Foward");
            foward = true;
        }
        else if (movement.y > 0 && foward)
        {
            animator.SetTrigger("Back");
            foward = false;
        }
        animator.SetBool("FacingFoward", foward);

        if (!flipped && ((movement.x < 0 && foward) || (movement.x > 0 && !foward)))
        {
            flipped = true;
        }
        else if (flipped && ((movement.x > 0 && foward) || (movement.x < 0 && !foward)))
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
