using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed = 1;

    private bool touchingRightWall;
    private bool touchingLeftWall;
    
    void Start()
    {
        
    }

    void Update()
    {
        var moveX = Input.GetAxis("Horizontal");
        
        if (touchingRightWall && moveX > 0)
        {
            moveX = 0;
        }
        if (touchingLeftWall && moveX < 0)
        {
            moveX = 0;
        }

        transform.Translate(Vector3.right * (moveX * speed * Time.deltaTime), Space.World);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            if (other.transform.position.x > transform.position.x)
            {
                touchingRightWall = true;
            }
            else
            {
                touchingLeftWall = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            touchingRightWall = false;
            touchingLeftWall = false;
        }
    }
}
