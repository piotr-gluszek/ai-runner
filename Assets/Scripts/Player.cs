using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NN;
using System.Runtime.InteropServices;
using TMPro;

enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public class Player : MonoBehaviour
{

    const float MOVEMENT_SPEED = 2;
    const float ANIMATION_SPEED = 1;
    public bool moving = false;

    // Movement speed of Player.
    public float movementSpeed;
    float rotationSpeed;
    public float[] inputs;
    public TMP_Text inputText;
    NeuralNetwork brain;
    public int lifeTime;
    // Components.
    BoxCollider2D collider;
    Animator animator;

    //public GameObject options;

    [DllImport("kernel32")]
    extern static UInt64 GetTickCount64();



    void Start()
    {
        tag = "Alive";
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        if (!moving)
            Freeze();
        else Move();

        if (brain == null)
            InitializeNeuralNetwork();

        inputText = GameObject.Find("Text").GetComponent<TMP_Text>();
        

    }



    // Update is called once per frame
    void Update()
    {
        if (moving)
            CheckDistance();

        UpdateInputs();
        NeuralNetworkMove();
        NeuralNetworkIncrementFitness();
        lifeTime++;
        if (lifeTime > 20000)
        {
            Freeze();
            tag = "Dead";
            brain.AddToFitness(-200000000);
        }

    }



    private void CheckDistance()
    {

        Vector2 offsetY = collider.size.y * transform.localScale / 2;
        Vector3 offset = new Vector3(0, (float)offsetY.y, 0);
        offset = transform.rotation * offset;
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
        inputs = new float[feeler.Length];

        // Loop through all feelers
        for (int i = 0; i < feeler.Length; i++)
        {
            // See what all feelers feel
            if (hit = Physics2D.Raycast(transform.position + offset, feeler[i]))
            {
                // If feelers feel something other than  nothing
                if (hit.collider != null && hit.collider != collider)
                {
                    if (hit.collider.tag == "Finish")
                        inputs[i] = 10;
                    else
                        inputs[i] = hit.distance;

                }

            }

            // Draw the feelers in the Scene mode
            Debug.DrawRay(transform.position + offset, feeler[i] * 2, Color.red);
        }

    }

    void DetectMovement()
    {



        transform.Translate(0, movementSpeed / 2 * Time.deltaTime, 0);
        animator.SetInteger("direction", (int)Direction.Up);
        // Starting movement after pressing Spacebar.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.speed = ANIMATION_SPEED;
            movementSpeed = MOVEMENT_SPEED;
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


    void NeuralNetworkMove()
    {

        // Disable movement while Options are displayed.
        // Start moving after pressing Spacebar.
        //if (!options.activeInHierarchy)
        //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Move();
        }
        //}

        if (moving)
        {
            transform.Translate(0, movementSpeed / 2 * Time.deltaTime, 0);
            animator.SetInteger("direction", (int)Direction.Up);

            brain.CalculateOutput(inputs);
            rotationSpeed = brain.GetOutput();

            transform.Rotate(0, 0, 300 * rotationSpeed * Time.deltaTime);
        }

    }

    //incrementing fitness every frame
    private void NeuralNetworkIncrementFitness()
    {
        if (moving)
            brain.IncrementFitness();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Wall")
        {
            Freeze();
            tag = "Dead";
        }
        if (collision.gameObject.tag == "Finish")
        {
            brain.AddToFitness(1000);
            tag = "Dead";
        }

    }


    private void UpdateInputs()
    {
        if (inputText != null)
            if (inputs.Length == 0)
                inputText.text = "L= 0\nLF= 0\nF= 0\nRF= 0\nR= 0";
            else
            {
                if (inputs.Length == 5)
                    inputText.text = "L= " + inputs[0].ToString() + "\nLF= " + inputs[1].ToString() + "\nF= " + inputs[2].ToString() + "\nRF= " + inputs[3].ToString() + "\nR= " + inputs[4].ToString();
                inputText.text += "\nOutput= " + rotationSpeed;
            }
    }


    public static int GetUpTime()
    {
        return (int)GetTickCount64();
    }

    bool isMoving()
    {
        return moving;
    }

    // Stop animation, stop movement and set flag. Disable collider.
    public void Freeze()
    {
        movementSpeed = 0;
        animator.speed = 0;
        collider.enabled = false;
        moving = false;
    }

    // Start animation, start movement and set flag. Enable collider.
    public void Move()
    {

        movementSpeed = MOVEMENT_SPEED;
        animator = GetComponent<Animator>();
        animator.speed = ANIMATION_SPEED;
        collider.enabled = true;
        moving = true;
    }

    public void InitializeNeuralNetwork()
    {
        UnityEngine.Random.InitState(GetUpTime());
        brain = new NeuralNetwork(5, 2);
        brain.SetRandomDNA();
    }

    public NeuralNetwork GetNeuralNetwork()
    {

        return brain;
    }



}
