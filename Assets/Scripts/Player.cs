using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum Direction
    {
        Right,
        Left,
    }

    Rigidbody2D rb;
    Animator animator;
    float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    Direction direction = Direction.Right;
    bool foward = true;
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

        if (movement.x > 0)
        {

        }
        else if (movement.x < 0)
        {

        }

        if (movement.y > 0 && !foward)
        {
            animator.SetTrigger("Back");
            foward = true;
        }
        else if (movement.y < 0 && foward)
        {
            animator.SetTrigger("Foward");
            foward = false;
        }
    }
}
