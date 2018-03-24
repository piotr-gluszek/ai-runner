using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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


public class Options : MonoBehaviour
{




    // Game object which will be set active.
    public Player player;
    GameObject buttons;
    bool isActive;

    public bool isMenuActive()
    {
        return isActive;
    }

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
    List<BlockData> blocksData;
    void GetBlocksData()
    {
        blocksData= new List<BlockData>();
        float X, Y;
        string tag;
        foreach (GameObject block in blocks)
        {
            X = block.GetComponent<Transform>().position.x;
            Y = block.GetComponent<Transform>().position.y;
            tag = block.tag;
            blocksData.Add(new BlockData(X,Y,tag));
            

        }

    }

    void Serialize()
    {
        string path = EditorUtility.SaveFilePanel("Saving stage...", "", "", "dat");
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
    void Deserialize()
    {
        string path = EditorUtility.OpenFilePanel("Saved stages", "", "dat");
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();

        // Deserialize the hashtable from the file and 
        // assign the reference to the local variable.
        blocksData = (List<BlockData>)formatter.Deserialize(fs);
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
       
        GameObject block;
        foreach (BlockData data in blocksData)
        {
           block=Instantiate(blockPrefab, new Vector3(data.X, data.Y, 0), Quaternion.identity);
           block.tag = data.tag;
            if (block.tag == "Finish")
            {
                block.GetComponent<SpriteRenderer>().color = Color.green;
            }


        }

    }
}
