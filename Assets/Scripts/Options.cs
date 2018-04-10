using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEditor;




[System.Serializable]
public class BlockData
{
    public float X;
    public float Y;
    public string tag;
    public BlockData() { }
    public BlockData(float X, float Y, string tag)
    {
        this.X = X;
        this.Y = Y;
        this.tag = tag;

    }
}


public class Options : MonoBehaviour, ICommunicutable
{

    private GameObject _mapNameInput;
    private string _mapName;



    // Game object which will be set active.
    GameObject buttons;
    bool isActive;

    public bool isMenuActive()
    {
        return isActive;
    }

    void Start()
    {
        _mapNameInput = transform.Find("Canvas/MapNameInput").gameObject;
        buttons = transform.Find("Canvas/Buttons").gameObject;
        isActive = false;

    }

    public void ShowMapNameInput()
    {
        _mapNameInput.SetActive(true);

    }

    public void SubmitMapName()
    {
        _mapName = _mapNameInput.transform.Find("Input").gameObject.GetComponent<TMP_InputField>().text;
        _mapNameInput.SetActive(false);

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


            }
            else
            {
                buttons.SetActive(false);

            }


            isActive = !isActive;


        }

    }
    public void ProceedData(GameObject gameObj)
    {
        if (!isActive)
        {
            if (Input.GetMouseButton(0))
            {
                Destroy(gameObj);
                Debug.Log("OnMouseOver: destory.");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                gameObj.tag = "Finish";
                gameObj.GetComponent<SpriteRenderer>().color = Color.green;
                Debug.Log("OnMouseOver: tag.");
            }
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
    List<BlockData> blocksData;
    void GetBlocksData()
    {
        blocksData = new List<BlockData>();
        float X, Y;
        string tag;
        foreach (GameObject block in blocks)
        {
            X = block.GetComponent<Transform>().position.x;
            Y = block.GetComponent<Transform>().position.y;
            tag = block.tag;
            blocksData.Add(new BlockData(X, Y, tag));


        }

    }

    void Serialize()
    {
        string path = @"Maps\" + _mapName + ".dat";
        FileStream fs = new FileStream(path, FileMode.Create);

        // Construct a BinaryFormatter and use it to serialize the data to the stream.
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, blocksData);

        fs.Close();
    }
    public void Save()
    {

        FindBlocks();
        GetBlocksData();
        Serialize();

    }

    public void Resume()
    {

        buttons.SetActive(false);
        isActive = false;
    }
}
