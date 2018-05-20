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
    }

    private void OnDrawGizmos()
    {
        float scale = 0.01f ;
        Gizmos.color = Color.yellow;
        for (int v = 0; v < vertices.Length; v++)
        {
            Gizmos.DrawRay(transform.position + vertices[v], (transform.position + vertices[v] + normals[v]) * scale);
        }
    }
}
