using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADTBlock : MonoBehaviour {

    Vector3 blockCenter;
    Vector3[] blockCorners;
    int materialLoDState;
    public Material GLmat;
    public bool reCheck;
    Camera cameraMain;

    private void Start()
    {
        cameraMain = Camera.main;
        blockCorners = new Vector3[4];
        blockCorners[0] = transform.GetChild(0).transform.position;
        blockCorners[1] = transform.GetChild(15).transform.position;
        blockCorners[2] = transform.GetChild(240).transform.position;
        blockCorners[3] = transform.GetChild(255).transform.position;
        materialLoDState = 1;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void Low()
    {
        for (int i = 1; i <= 4; i++)
        {
            for (int j = (256 / 4) * (i - 1); j < (256 / 4) * i; j++)
            {
                transform.GetChild(j).GetComponent<ADTChunk>().UpdateDistance(1);
            }
        }
    }

    private void High()
    {
        for (int i = 1; i <= 4; i++)
        {
            for (int j = (256 / 4) * (i - 1); j < (256 / 4) * i; j++)
            {
                transform.GetChild(j).GetComponent<ADTChunk>().UpdateDistance(0);
            }
        }
    }

    public void UpdatePosition ()
    {
        // find minimum corner distance //
        float distance = 10000;
        for (int i = 0; i < 4; i++)
        {
            Vector3 heading = blockCorners[i] - cameraMain.transform.position;
            float currentDistance = Vector3.Dot(heading, cameraMain.transform.forward);
            if (currentDistance < distance)
            {
                distance = currentDistance;
            }
        }
        if (distance < Settings.terrainMaterialDistance / Settings.worldScale)
        {
            if (materialLoDState == 1 || reCheck)
            {
                materialLoDState = 0;
                reCheck = false;
                High();
            }
        }
        else
        {
            if (materialLoDState == 0 || reCheck)
            {
                materialLoDState = 1;
                reCheck = false;
                Low();
            }
        }
    }
}
