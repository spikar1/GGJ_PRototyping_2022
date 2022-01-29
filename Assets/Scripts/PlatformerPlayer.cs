using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    float walkSpeed = 7, jumpStrength = 4;
    public  bool frozen;

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
        frozen = true;
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
    }
    public void UnFreeze()
    {
        frozen = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
