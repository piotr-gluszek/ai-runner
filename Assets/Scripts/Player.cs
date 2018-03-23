using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NN;
using System.Runtime.InteropServices;

enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public class Player : MonoBehaviour
{
    
    // Movement speed of Player.
    public float speed;
    Animator animator;
    public float[] inp;
    public Text inputText;
    BoxCollider2D col;
    float movment;
    NeuralNetwork brain;

    [DllImport("kernel32")]
    extern static UInt64 GetTickCount64();



    void Start()
    {


        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        animator.speed = 0;

        UnityEngine.Random.InitState(GetUpTime());
        brain = new NeuralNetwork(5, 2);
        brain.SetRandomDNA();
        //SetUpInputsDisplay();
    }



    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        //DetectMovement();
        NeuralNetworkMove();
    }

    private void CheckDistance()
    {

        Vector2 offsetY = col.size.y * transform.localScale/2;
        Vector3 offset =new Vector3(0,(float)offsetY.y, 0);
        offset = transform.rotation * offset ;
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
       


        transform.Translate(0, speed / 2 * Time.deltaTime, 0);
        animator.SetInteger("direction", (int)Direction.Up);
        // Starting movement after pressing Spacebar.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.speed = 1;
            speed = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(0, 0, 30 * 3 * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, 30 * -3 * Time.deltaTime);

        }
        

    }
    void NeuralNetworkMove() {
        transform.Translate(0, speed / 2 * Time.deltaTime, 0);
        animator.SetInteger("direction", (int)Direction.Up);

        brain.CalculateOutput(inp);
        movment = brain.GetOutput();

        transform.Rotate(0, 0, 30 * 3* movment * Time.deltaTime);


    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("Success");
        }
       
    }


    private void SetUpInputsDisplay()
    {
        if (inp.Length == 0)
            inputText.text = "L= 0 LF= 0 F= 0 RF= 0 R= 0";
        else
        {
            if (inp.Length == 5)
                inputText.text = "L= " + inp[0].ToString() + "\nLF= " + inp[1].ToString() + "\nF= " + inp[2].ToString() + "\nRF= " + inp[3].ToString() + "\nR= " + inp[4].ToString();
            inputText.text += "\n output= " + movment;
        }
    }


    public static int GetUpTime()
    {
        return (int)GetTickCount64();
    }


}
