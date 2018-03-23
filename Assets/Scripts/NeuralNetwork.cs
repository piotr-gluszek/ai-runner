using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NN
{

    public class NeuralNetwork
    {
        int inputNumber;              //number of inputs, const for every hidden layer

        float[] outputWaights;       // waights of output
        float outputNodeWaight;
        float _output = 0;

        int hiddenLayerNumber;
        int hiddenLayerNodeNumber;
        float[][][] arrowWaights;    //waights of arrows connecting nodes
        float[][] nodeWaights;       // node waights

        float randNumSeedPlus = 1;
        float randNumSeedMinus = -1;

        //creates NN
        public NeuralNetwork(int input, int hLN)
        {
            inputNumber = input;
            hiddenLayerNumber = hLN;
            hiddenLayerNodeNumber = input;

            //creating 3d table for arrow waights
            arrowWaights = new float[hiddenLayerNumber][][];
            for (int i = 0; i < hiddenLayerNumber; i++)
            {
                arrowWaights[i] = new float[hiddenLayerNodeNumber][];
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                {
                    arrowWaights[i][j] = new float[inputNumber];
                }
            }

            //creating 2d table for node waights
            nodeWaights = new float[hiddenLayerNumber][];
            for (int i = 0; i < hiddenLayerNumber; i++)
            {
                nodeWaights[i] = new float[hiddenLayerNodeNumber];
            }

            outputWaights = new float[inputNumber];


        }
        //set random values for neural network;
        public void SetRandomDNA()
        {

            for (int i = 0; i < hiddenLayerNumber; i++)
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                    for (int k = 0; k < inputNumber; k++)
                    {
                        arrowWaights[i][j][k] = GetRandomNumber(-randNumSeedMinus, randNumSeedPlus);
                    }

            for (int i = 0; i < hiddenLayerNumber; i++)
                for (int j = 0; j < hiddenLayerNodeNumber; j++)
                {
                    nodeWaights[i][j] = GetRandomNumber(-randNumSeedMinus, randNumSeedPlus);
                }

            for (int i = 0; i < inputNumber; i++)
                outputWaights[i] = GetRandomNumber(-randNumSeedMinus, randNumSeedPlus);

            outputNodeWaight = GetRandomNumber(-randNumSeedMinus, randNumSeedPlus);

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
                tmpSums[0][i] = tmpSums[0][i] * nodeWaights[0][i];
                tmpSums[0][i] = (float)Math.Tanh(tmpSums[0][i]);
            }

            //rest of hidden layers
            if (hiddenLayerNumber > 1)
            {
                for (int j = 1; j < hiddenLayerNumber; j++)
                    for (int i = 0; i < hiddenLayerNodeNumber; i++)
                    {
                        tmpSums[j][i] = GetWaightedSum(i, j, tmpSums[j - 1])/inputNumber;
                        tmpSums[j][i] = tmpSums[j][i] * nodeWaights[j][i];
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
                _output = outputWaights[i] * lastHiddenLayer[i];
            }
            _output *= outputNodeWaight/inputNumber;
            _output = (float)Math.Tanh(_output)/3;
        }

        //calculating node sums
        private float GetWaightedSum(int nodeID, int nodeHLayerID, float[] input)
        {
            float sum = 0;

            for (int i = 0; i < input.Length; i++)
            {
                sum += arrowWaights[nodeHLayerID][nodeID][i] * input[i];
            }

            return sum;

        }

        public float GetOutput()
        {
            return _output;
        }



        public float GetRandomNumber(float minimum, float maximum)
        {
            
            //Random random = new Random();
            return UnityEngine.Random.Range(-4f,4f);
            //return (float)(random.NextDouble() * (maximum - minimum) + minimum);
            //return (float)random.NextDouble();
        }

    }

}