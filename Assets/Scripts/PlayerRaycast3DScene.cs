using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast3DScene : MonoBehaviour
{

    public Camera camera3D;
    public Camera camera2D;
    public Transform playerTrans;

    public Ray ray;

    public float distance = 15.5f; //WHY?
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var bounds = playerTrans.GetComponent<Collider2D>().bounds;

        if (Physics.Raycast(camera3D.transform.position, GetRayDirectionFromPosition(bounds.center + Vector3.down * bounds.extents.y)))
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            Debug.DrawRay(camera3D.transform.position, GetRayDirectionFromPosition(bounds.center + Vector3.down * bounds.extents.y) * 100, Color.red);
            Vector3 offset = Vector3.zero;
            while (Physics.Raycast(camera3D.transform.position, GetRayDirectionFromPosition(bounds.center + Vector3.down * bounds.extents.y + offset)))
            {
                Debug.DrawRay(camera3D.transform.position, GetRayDirectionFromPosition(bounds.center + Vector3.down * bounds.extents.y + offset) * 100, Color.blue);
                offset += Vector3.up * .05f;
            }
            transform.position += offset;
        }
        else if (!Physics.Raycast(camera3D.transform.position, GetRayDirectionFromPosition(bounds.center + Vector3.down * bounds.extents.y + Vector3.down * .06f)))
            rb.gravityScale = 1;

        if (Input.GetKey(KeyCode.A))
            rb.velocity = new Vector2(-7, rb.velocity.y);
        else if (Input.GetKey(KeyCode.D))
            rb.velocity = new Vector2(7, rb.velocity.y);
        else
            rb.velocity = Vector2.MoveTowards(rb.velocity, new Vector2(0, rb.velocity.y), 40 * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            rb.velocity = new Vector2(rb.velocity.x, 16);
    }


    private void OnDrawGizmos()
    {
        UpdateRay();
        Gizmos.color = Color.yellow;
        //PlayerCenter
        Gizmos.DrawRay(ray.origin, GetRayDirectionFromPosition(playerTrans.position) * 100);
        //Bounds
        var bounds = playerTrans.GetComponent<Collider2D>().bounds;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ray.origin, GetRayDirectionFromPosition(bounds.center + Vector3.left * bounds.extents.x) * 100);
        Gizmos.DrawRay(ray.origin, GetRayDirectionFromPosition(bounds.center + Vector3.right * bounds.extents.x) * 100);
        Gizmos.DrawRay(ray.origin, GetRayDirectionFromPosition(bounds.center + Vector3.up * bounds.extents.y) * 100);
        Gizmos.DrawRay(ray.origin, GetRayDirectionFromPosition(bounds.center + Vector3.down* bounds.extents.y) * 100);
    }

    void UpdateRay()
    {
        ray.origin = camera3D.transform.position;
        //ray.direction = Quaternion.AngleAxis(yAxisFactor * 30, Vector3.left) * camera3D.transform.forward ;
        ray.direction = GetRayDirectionFromPosition(playerTrans.position);
    }

    private Vector3 GetRayDirectionFromPosition(Vector2 position)
    {
        return Quaternion.AngleAxis(GetRayAngle(position), Quaternion.Euler(0, 0, 90) * position) * camera3D.transform.forward;
        float GetRayAngle(Vector3 position) { 
                if(distance > 0)
                    return Mathf.Atan(position.magnitude / distance) * Mathf.Rad2Deg;
                return 0;
        }
    }
}
