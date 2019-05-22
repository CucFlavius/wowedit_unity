using Assets.WoWEditSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.World.Terrain
{
    public class ADTBlock : MonoBehaviour
    {

        Vector3 blockCenter;
        Vector3[] blockCorners;
        int materialLoDState;
        public Material GLmat;
        public bool reCheck;
        Camera cameraMain;
        public Vector2 coords;
        public string mapName;

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

        public void UnloadAsset()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                //Destroy(transform.GetChild(i).GetComponent<MeshFilter>().mesh);
                transform.GetChild(i).gameObject.SetActive(false);
                Destroy(transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh);
                Destroy(transform.GetChild(i).GetComponent<ADTChunk>().mesh);
                for (int ln = 1; ln < 4; ln++)
                {
                    try
                    {
                        if (transform.GetChild(i).GetComponent<ADTChunk>().high != null)
                            Destroy(transform.GetChild(i).GetComponent<ADTChunk>().high.GetTexture("_blend" + ln));
                    }
                    catch
                    {
                        Debug.Log("Memory Cleaner - Error: Couldn't find " + "_blend" + ln);
                    }
                }
                if (transform.GetChild(i).GetComponent<ADTChunk>().low != null)
                {
                    Destroy(transform.GetChild(i).GetComponent<ADTChunk>().low.GetTexture("_MainTex2"));
                    Destroy(transform.GetChild(i).GetComponent<ADTChunk>().low);
                }
                if (transform.GetChild(i).GetComponent<ADTChunk>().high != null)
                    Destroy(transform.GetChild(i).GetComponent<ADTChunk>().high);
                //if (transform.GetChild(i).GetComponent<Renderer>().sharedMaterial != null)
                //DestroyImmediate(transform.GetChild(i).GetComponent<Renderer>().sharedMaterial, true);
            }
            Destroy(gameObject);
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

        public void UpdatePosition()
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
            if (distance < Settings.terrainMaterialDistance / Settings.WorldScale)
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
}