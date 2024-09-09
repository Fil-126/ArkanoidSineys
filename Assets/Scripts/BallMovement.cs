using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BallMovement : MonoBehaviour
{
    public float speed = 1;
    public float minVerticalSpeed = 0.01f;
    [HideInInspector]
    public Vector2 startPosition;
    
    private Vector2 velocity;  // Let it be normalized, please
    private bool started;
    private bool bounced;
    
    void Start()
    {
        startPosition = transform.position;
    }
    
    void Update()
    {
        if (!started)
        {
            started = Input.GetButtonDown("Jump");  // Space
            SetRandomVelocity();
        }
    }

    private void FixedUpdate()
    {
        if (!started)
            return;
        
        bounced = false;
        transform.Translate(speed * Time.deltaTime * velocity);
    }

    private void SetRandomVelocity()
    {
        velocity = new Vector2(0.5f - Random.value, -1).normalized;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var contact = other.GetContact(0);

        // If already bounced in this frame, treat as normal collision
        if (other.gameObject.CompareTag("Player") && !bounced)
        {
            var d = contact.point.x - other.transform.position.x;
            var cos = d / other.collider.bounds.extents.x;
            var sin = Mathf.Sqrt(1 - cos * cos);
            velocity = new Vector2(cos, sin);
            
            return;
        }

        var dot = Vector2.Dot(contact.normal, velocity);

        // Bounce only if velocity points towards the block (or wall)
        if (dot < 0)
            velocity -= contact.normal * (2 * dot);
        
        bounced = true;
        
        // if ball is moving horizontally, slightly change y-velocity
        if (Mathf.Abs(velocity.y) < minVerticalSpeed)
        {
            var new_y = Mathf.Sign(velocity.y) * minVerticalSpeed;
            var new_x = Mathf.Sign(velocity.x) * Mathf.Sqrt(1 - new_y * new_y);
            velocity = new Vector2(new_x, new_y);
        }

        var brick = other.gameObject.GetComponent<Brick>();
        brick?.BallHit();
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bottom"))
        {
            GameController.instance.playerHp -= 1;
            if (GameController.instance.playerHp <= 0)
            {
                SceneManager.LoadScene("LoseMenu", LoadSceneMode.Single);
                return;
            }

            ResetBall();
        }
    }

    public void ResetBall()
    {
        transform.position = startPosition;
        started = false;
    }
}
