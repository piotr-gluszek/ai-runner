using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillWithBlocks : MonoBehaviour
{
    // Prefab used to fill the scene with.
    public GameObject block;

    // Calculation of camera size.
    Vector2 CalculateScreenSizeInWorldCoords()
    {
        var cam = Camera.main;
        var p1 = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        var p2 = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        var p3 = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        var width = (p2 - p1).magnitude;
        var height = (p3 - p2).magnitude;

        var dimensions = new Vector2(width, height);

        return dimensions;
    }

    // Scene initialization.
    void Start()
    {
        // 2D prefab size.
        float sizeX = block.GetComponent<SpriteRenderer>().bounds.size.x;
        float sizeY = block.GetComponent<SpriteRenderer>().bounds.size.y;

        // Camera size.
        float camX = CalculateScreenSizeInWorldCoords().x;
        float camY = CalculateScreenSizeInWorldCoords().y;

        for (int y = 0; y < camY/sizeY/2; y++)
        {
            for (int x = 0; x < camX/sizeX/2; x++)
            {
                Instantiate(block, new Vector3(x * sizeX+sizeX/2 , y * sizeY+sizeY/2, 0), Quaternion.identity);
                

            }
        }

        for (int y = 0; y < camY / sizeY / 2; y++)
        {
            for (int x = 0; x < camX / sizeX / 2; x++)
            {
                Instantiate(block, new Vector3(-(x * sizeX + sizeX / 2),-( y * sizeY + sizeY / 2), 0), Quaternion.identity);


            }
        }
        for (int y = 0; y < camY / sizeY / 2; y++)
        {
            for (int x = 0; x < camX / sizeX / 2; x++)
            {
                Instantiate(block, new Vector3(x * sizeX + sizeX / 2, -(y * sizeY + sizeY / 2), 0), Quaternion.identity);


            }
        }
        for (int y = 0; y < camY / sizeY / 2; y++)
        {
            for (int x = 0; x < camX / sizeX / 2; x++)
            {
                Instantiate(block, new Vector3(-(x * sizeX + sizeX / 2), y * sizeY + sizeY / 2, 0), Quaternion.identity);


            }
        }
        Debug.Log(camX+" "+camY+" "+sizeX+" "+sizeY);
    }

    private void Update()
    {
        
    }
}


