using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public class Player : MonoBehaviour
{

    // Use this for initialization
    public float speed;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        DetectMovement();
    }

    void DetectMovement()
    {
        animator.speed = 1;
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            animator.SetInteger("direction", (int)Direction.Up);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            animator.SetInteger("direction", (int)Direction.Down);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            animator.SetInteger("direction", (int)Direction.Left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            animator.SetInteger("direction", (int)Direction.Right);
        }
        else animator.speed = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        //Destroy(collision.gameObject);
    }

}
