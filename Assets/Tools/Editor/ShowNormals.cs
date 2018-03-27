using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShowNormals : MonoBehaviour {

    private Vector3[] vertices;
    private Vector3[] normals;

    void Start()
    {
        vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
        normals = transform.GetComponent<MeshFilter>().mesh.normals;
        /*
        string[] normalsTXT = new string[normals.Length];

        for (int i = 0; i < normals.Length; i++)
        {
            string line = normals[i].x.ToString() + " " + normals[i].y.ToString() + " " + normals[i].z.ToString();
            normalsTXT[i] = line;
        }

        File.WriteAllLines(@"d:\normals.txt", normalsTXT);
        */
    }

    private void OnDrawGizmos()
    {
        print(normals.Length);
        float scale = 0.01f ;
        //if (showNormals && vertices != null)
        //{
            Gizmos.color = Color.yellow;
            for (int v = 0; v < vertices.Length; v++)
            {
                Gizmos.DrawRay(transform.position + vertices[v], (transform.position + vertices[v] + normals[v]) * scale);
            }
        //}
    }
}
