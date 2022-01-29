using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    float walkSpeed = 7, jumpStrength = 4;

        private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * walkSpeed, rb.velocity.y);
    }

    public void Freeze()
    {
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
    }
    public void UnFreeze()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
