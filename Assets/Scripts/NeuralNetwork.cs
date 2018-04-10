using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NN
{

    public class NeuralNetwork
    {
        int inputNumber;              //number of inputs, const for every hidden layer

        float[] outputWeights;       // waights of output
        float outputNodeWeight;
        float _output = 0;

        int hiddenLayerNumber;
        int hiddenLayerNodeNumber;
        float[][][] arrowWeights;    //waights of arrows connecting nodes
        float[][] nodeWeights;       // node waights

        float randNumPlus = 4;
        float randNumMinus = -4;

        float fitness;

        //creates NN
        public NeuralNetwork(int input, int hLN)
        {
            inputNumber = input;
            hiddenLayerNumber = hLN;
            hiddenLayerNodeNumber = input;

            //creating 3d table for arrow waights
            arrowWeights = new float[hiddenLayerNumber][][];
            for (int i = 0; i < hiddenLayerNumber; i++)
            {
                arrowWeights[i] = new float[hiddenLayerNodeNumber][];
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                {
                    arrowWeights[i][j] = new float[inputNumber];
                }
            }

            //creating 2d table for node waights
            nodeWeights = new float[hiddenLayerNumber][];
            for (int i = 0; i < hiddenLayerNumber; i++)
            {
                nodeWeights[i] = new float[hiddenLayerNodeNumber];
            }

            outputWeights = new float[inputNumber];


        }
        //set random values for neural network;
        public void SetRandomDNA()
        {

            for (int i = 0; i < hiddenLayerNumber; i++)
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                    for (int k = 0; k < inputNumber; k++)
                    {
                        arrowWeights[i][j][k] = GetRandomNumber(randNumMinus, randNumPlus);
                    }

            for (int i = 0; i < hiddenLayerNumber; i++)
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                {
                    nodeWeights[i][j] = GetRandomNumber(randNumMinus, randNumPlus);
                }

            for (int i = 0; i < inputNumber; i++)
                outputWeights[i] = GetRandomNumber(randNumMinus, randNumPlus);

            outputNodeWeight = GetRandomNumber(randNumMinus, randNumPlus);

        }


        public void SetDNA(float[] dna) {

            int counter = 0;
            //checking for dna length
            if (dna.Length == GetDNASize())
            {
                for (int i = 0; i < hiddenLayerNumber; i++)
                    for (int j = 0; j < hiddenLayerNodeNumber; j++)
                        for (int k = 0; k < inputNumber; k++)
                        {
                            arrowWeights[i][j][k] = dna[counter];
                            counter++;
                        }

                for (int i = 0; i < hiddenLayerNumber; i++)
                    for (int j = 0; j < hiddenLayerNodeNumber; j++)
                    {
                        nodeWeights[i][j] = dna[counter];
                        counter++;
                    }

                for (int i = 0; i < inputNumber; i++)
                {
                    outputWeights[i] = dna[counter];
                    counter++;
                }

                outputNodeWeight = dna[counter];
               
            }
            //if DNA isn't valid
            else {
                Debug.Log("Error seting DNA! Randomizing "+ dna.Length+" "+GetDNA());
                SetRandomDNA();
            }


        }


        //return false if input.Lenght!=inputNumber
        //saves result in _output
        public bool CalculateOutput(float[] input)
        {

            if (input.Length != inputNumber)
                return false;

            float[][] tmpSums = new float[hiddenLayerNumber][];

            for (int i = 0; i < hiddenLayerNumber; i++)
            {
                tmpSums[i] = new float[hiddenLayerNodeNumber];
            }
            //first hidden layer
            for (int i = 0; i < hiddenLayerNodeNumber; i++)
            {
                tmpSums[0][i] = GetWaightedSum(i, 0, input)/inputNumber;
                tmpSums[0][i] = tmpSums[0][i] * nodeWeights[0][i];
                tmpSums[0][i] = (float)Math.Tanh(tmpSums[0][i]);
            }

            //rest of hidden layers
            if (hiddenLayerNumber > 1)
            {
                for (int j = 1; j < hiddenLayerNumber; j++)
                    for (int i = 0; i < hiddenLayerNodeNumber; i++)
                    {
                        tmpSums[j][i] = GetWaightedSum(i, j, tmpSums[j - 1])/inputNumber;
                        tmpSums[j][i] = tmpSums[j][i] * nodeWeights[j][i];
                        tmpSums[j][i] = (float)Math.Tanh(tmpSums[j][i]);
                    }

            }
            //output
            CalculateOutputSum(tmpSums[hiddenLayerNumber - 1]);


            return true;
        }

        //last step of output calculation
        private void CalculateOutputSum(float[] lastHiddenLayer)
        {
            _output = 0;
            for (int i = 0; i < inputNumber; i++)
            {
                _output = outputWeights[i] * lastHiddenLayer[i];
            }
            _output *= outputNodeWeight/inputNumber;
            _output = (float)Math.Tanh(_output)/3;
        }

        //calculating node sums
        private float GetWaightedSum(int nodeID, int nodeHLayerID, float[] input)
        {
            float sum = 0;

            for (int i = 0; i < input.Length; i++)
            {
                sum += arrowWeights[nodeHLayerID][nodeID][i] * input[i];
            }

            return sum;

        }

        public float GetOutput()
        {
            return _output;
        }



        public float GetRandomNumber(float minimum, float maximum)
        {
            //return UnityEngine.Random.Range(-4f,4f); 
            return UnityEngine.Random.Range(minimum, maximum);
        }

        public int GetDNASize() {

            int DNASize = inputNumber * hiddenLayerNumber * hiddenLayerNodeNumber;//number of arrows in hidden layers
            DNASize += inputNumber * hiddenLayerNumber;//number of hidden layers waght
            DNASize += inputNumber;//number of output weights
            DNASize += 1;//output weight

            return DNASize;
            
        }

        public float[] GetDNA() {
            int size = GetDNASize();
            float[] DNA = new float[size];
            int c = 0; //counter;

            for (int i = 0; i < hiddenLayerNumber; i++)
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                    for (int k = 0; k < inputNumber; k++)
                    {
                        DNA[c] = arrowWeights[i][j][k];
                        c++;
                    }

            for (int i = 0; i < hiddenLayerNumber; i++)
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                {
                    DNA[c] = nodeWeights[i][j];
                    c++;
                }

            for (int i = 0; i < inputNumber; i++)
            {
                DNA[c] = outputWeights[i];
                c++;
            }

            DNA[c] = outputNodeWeight;

            return DNA;


        }

        public void IncrementFitness() {
            fitness+=0.15f;
        }

        public float GetFitness() {

            return fitness;

        }


    }

}