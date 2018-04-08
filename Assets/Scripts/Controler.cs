﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Controler : MonoBehaviour {

    public GameObject blockPrefab;
    public GameObject prefab;
    public GameObject child;
    public Transform startPoint;
    public Transform finishLine;
    public int generationCount; //number of generations pass
    public int triesCount;
    public int triesNumber;
    public float mutationRate;

    float[][] dna;//saved DNA for one day; 
    float[] fitnesses;//fitnesses for DNA;
    bool alive = false;

	// Use this for initialization
	void Start () {
        generationCount = 0;
        triesCount = 0;
        dna = new float[triesNumber][];
        fitnesses = new float[triesNumber];
        alive = true;
	    LoadSettings();
	    Load();
	}

    void LoadSettings()
    {
        // Load all options from static class Settings.
        triesNumber = Settings.AttemptNum;
        mutationRate = Settings.MutationRate;
    }
    List<BlockData> blocksData;
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
            block = Instantiate(blockPrefab, new Vector3(data.X, data.Y, 0), Quaternion.identity);
            block.tag = data.tag;
            if (block.tag == "Finish")
            {
                block.GetComponent<SpriteRenderer>().color = Color.green;
            }


        }

    }
    // Update is called once per frame
    void Update () {

        GameObject p = GameObject.FindGameObjectWithTag("Dead");

        //if dead player was found
        if (p != null && alive) {

            dna[triesCount] = p.GetComponent<Player>().GetNeuralNetwork().GetDNA();
            fitnesses[triesCount] = p.GetComponent<Player>().GetNeuralNetwork().GetFitness();
            CalculateFinalFitness(p.GetComponent<Transform>().position);
            child=Instantiate(prefab);
            Destroy(p);
            Debug.Log("Dead!!");

            if (child != null)
            {
                child.GetComponent<Player>().InitializeNeuralNetwork();
                child.GetComponent<Player>().GetNeuralNetwork().SetDNA(dna[triesCount]);

            }
            triesCount++;
            Debug.Log("Dead!!");

            //alive = false;
            //if generation has ended
            if (triesCount >= triesNumber)
            {
                MutateDNA();
                SaveDNA();
                triesCount = 0;
            }
        }

       
       

	}

    private void CalculateFinalFitness(Vector3 position)
    {
        fitnesses[triesCount] -= Vector3.Distance(position, finishLine.position);
    }

    

    void MutateDNA() {

        float mutate;
        for(int i=0; i<dna.Length; i++)
        {
            for(int j=0; j < dna[i].Length; j++)
            {
                mutate = UnityEngine.Random.Range(0f, 1f);
                if (mutate <= mutationRate)
                    dna[i][j] = UnityEngine.Random.Range(-4f, 4f);
            }
        }

    }


    //saving DNA in text file
    void SaveDNA()
    {
        string output="";
        for (int i = 0; i < dna.Length; i++) {
            for (int j = 0; j < dna[i].Length; j++) {
                output += dna[i][j] + " ";              
            }
            output += " fitness= "+fitnesses[i]+"\r\n";
        }
        

        File.WriteAllText("Saves\\generation" + generationCount + ".txt", output);

        generationCount++;

    }

}
