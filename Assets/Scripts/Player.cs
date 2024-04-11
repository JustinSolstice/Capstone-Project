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
       
    }
}
