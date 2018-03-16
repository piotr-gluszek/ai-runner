using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public float[] inp;
    public Text inputText;
    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        SetUpInputsDisplay();
    }

   

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        DetectMovement();
    }

    private void CheckDistance()
    {

        Vector3 offset =new Vector3(0,(float)0.5, 0);
        // Set up a raycast hit for knowing what we hit
        RaycastHit2D hit;

        // Set up out 5 feelers for undertanding the world
        Vector3[] feeler = new Vector3[]
        {     
            // 0 = L
            transform.TransformDirection(Vector2.left),
            // 1 - FL
            transform.TransformDirection(Vector2.left+Vector2.up),
            // 2 - F
            transform.TransformDirection(Vector2.up),
            // 3 = FR
            transform.TransformDirection(Vector2.right + Vector2.up),
            // 4 = R
            transform.TransformDirection(Vector2.right),
        };

        // Use this to collect all feeler distances, then well pass them through our NN for an output
        inp = new float[feeler.Length];

        // Loop through all feelers
        for (int i = 0; i < feeler.Length; i++)
        {
            // See what all feelers feel
            if (hit=Physics2D.Raycast(transform.position+offset, feeler[i]))
            {
                // If feelers feel something other than  nothing
                if (hit.collider != null && hit.collider != col)
                {
                    // Set the input[i] to be the distance of feeler[i]
                    inp[i] = hit.distance;
                    
                }

            }
            
            // Draw the feelers in the Scene mode
            Debug.DrawRay(transform.position+offset, feeler[i] * 2, Color.red);           
        }
        SetUpInputsDisplay();
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
        if (collision.gameObject.name == "FinishLine")
        {
            SceneManager.LoadScene("Success");
        }
        //Destroy(collision.gameObject);
    }


    private void SetUpInputsDisplay()
    {
        if (inp.Length == 0)
            inputText.text = "L= 0 LF= 0 F= 0 RF= 0 R= 0";
        else {
            if(inp.Length==5)
            inputText.text = "L= " + inp[0].ToString() + "\nLF= " + inp[1].ToString() + "\nF= " + inp[2].ToString() + "\nRF= " + inp[3].ToString() + "\nR= " + inp[4].ToString();
        }
    }



}
