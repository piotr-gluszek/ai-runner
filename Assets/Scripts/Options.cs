using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Options : MonoBehaviour
{




    // Game object which will be set active.
    public Player player;
    GameObject buttons;
    bool isActive;

    void Start()
    {
        buttons = transform.Find("Buttons").gameObject;
        isActive = false;

    }


    void Update()
    {

        // Show Options when ESC pressed.
        // When ESC pressed while Options are visible set to invisible (inactive).
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActive)
            {
                buttons.SetActive(true);
                player.Freeze();

            }
            else
            {
                buttons.SetActive(false);
                player.Move();
            }


            isActive = !isActive;


        }

    }

    List<GameObject> blocks;
    List<float> blockCoordinates;
    public GameObject blockPrefab;
    float savedBlocksNum = 0;
    void FindBlocks()
    {
        List<GameObject> objects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(objects);

        blocks = new List<GameObject>();
        // iterate root objects and do something
        foreach (GameObject sceneObject in objects)
        {
            if (sceneObject.name == "Block(Clone)")
            {
                blocks.Add(sceneObject);
                savedBlocksNum++;
            }

        }


    }
    void GetBlockCoordinates()
    {
        blockCoordinates = new List<float>();
        float X, Y;
        foreach (GameObject block in blocks)
        {
            X = block.GetComponent<Transform>().position.x;
            Y = block.GetComponent<Transform>().position.y;
            blockCoordinates.Add(X);
            blockCoordinates.Add(Y);

        }

    }

    void Serialize()
    {
        FileStream fs = new FileStream("SavedScene23022018.dat", FileMode.Create);

        // Construct a BinaryFormatter and use it to serialize the data to the stream.
        BinaryFormatter formatter = new BinaryFormatter();
        blockCoordinates.Insert(0, (float)savedBlocksNum);
        formatter.Serialize(fs, blockCoordinates);
        fs.Close();
    }
    public void Save()
    {
        FindBlocks();
        GetBlockCoordinates();
        Serialize();

    }
    void Deserialize()
    {
        FileStream fs = new FileStream("SavedScene23022018.dat", FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();

        // Deserialize the hashtable from the file and 
        // assign the reference to the local variable.
        blockCoordinates = (List<float>)formatter.Deserialize(fs);
        fs.Close();
    }
    public void Load()
    {
        DestroyBlocks();
        Deserialize();
        SetBlocksUp();
    }
    void DestroyBlocks()
    {
        List<GameObject> objects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(objects);

        // iterate root objects and do something
        foreach (GameObject sceneObject in objects)
        {
            if (sceneObject.name == "Block(Clone)")
                Destroy(sceneObject);
        }
    }


    void SetBlocksUp()
    {
        float X, Y;
        savedBlocksNum = (int)blockCoordinates[0];

        for (int index = 1; index < (savedBlocksNum*2+1); index += 2)
        {
            X = blockCoordinates[index];
            Y = blockCoordinates[index + 1];
            Instantiate(blockPrefab, new Vector3(X, Y, 0), Quaternion.identity);
        }

    }
}
