using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.WoWEditSettings;

namespace Assets.World.Tests
{
    // Put this script on a Camera
    public class DrawLines : MonoBehaviour
    {

        // Fill/drag these in from the editor

        // Choose the Unlit/Color shader in the Material Settings
        // You can change that color, to change the color of the connecting lines
        public Material lineMat;

        public GameObject mainPoint;
        public List<GameObject> points;
        public Color col;

        // Connect all of the `points` to the `mainPoint`
        void DrawConnectingLines()
        {
            if (mainPoint && points.Count > 0 && points[0] != null)
            {
                // Loop through each point to connect to the mainPoint
                foreach (GameObject point in points)
                {
                    Vector3 mainPointPos = mainPoint.transform.position - new Vector3(0, 2, 0);
                    Vector3 pointPos = point.transform.position;
                    float dist = Vector3.Distance(mainPoint.transform.position, pointPos);
                    if (dist < (Settings.terrainMaterialDistance) / Settings.WORLD_SCALE)
                        col = Color.red;
                    else
                        col = Color.yellow;
                    GL.Begin(GL.LINES);
                    lineMat.SetPass(0);
                    //GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
                    GL.Color(col);
                    GL.Vertex3(mainPointPos.x, mainPointPos.y, mainPointPos.z);
                    GL.Vertex3(pointPos.x, pointPos.y, pointPos.z);
                    GL.End();
                }
            }
        }

        // To show the lines in the game window whne it is running
        void OnPostRender()
        {
            DrawConnectingLines();
        }

        // To show the lines in the editor
        void OnDrawGizmos()
        {
            DrawConnectingLines();
        }
    }
}