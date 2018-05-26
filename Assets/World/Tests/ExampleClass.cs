﻿using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<Animation>();
        gameObject.AddComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer rend = GetComponent<SkinnedMeshRenderer>();
        Animation anim = GetComponent<Animation>();

        // Build basic mesh
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(-1, 5, 0), new Vector3(1, 5, 0) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
        mesh.RecalculateNormals();

        // Assign mesh to mesh filter & renderer
        rend.material = new Material(Shader.Find("Diffuse"));

        // Assign bone weights to mesh
        // We use 2 bones. One for the lower vertices, one for the upper vertices.
        BoneWeight[] weights = new BoneWeight[4];

        weights[0].boneIndex0 = 0;
        weights[0].weight0 = 1;

        weights[1].boneIndex0 = 0;
        weights[1].weight0 = 1;

        weights[2].boneIndex0 = 1;
        weights[2].weight0 = .1f;
        weights[2].boneIndex1 = 0;
        weights[2].weight1 = .1f;

        weights[3].boneIndex0 = 1;
        weights[3].weight0 = 1f;

        // A BoneWeights array (weights) was just created and the boneIndex and weight assigned.
        // The weights array will now be assigned to the boneWeights array in the Mesh.
        mesh.boneWeights = weights;

        // Create Bone Transforms and Bind poses
        // One bone at the bottom and one at the top
        Transform[] bones = new Transform[2];
        Matrix4x4[] bindPoses = new Matrix4x4[2];

        bones[0] = new GameObject("Lower").transform;
        bones[0].parent = transform;
        // Set the position relative to the parent
        bones[0].localRotation = Quaternion.identity;
        bones[0].localPosition = Vector3.zero;

        // The bind pose is bone's inverse transformation matrix
        // In this case the matrix we also make this matrix relative to the root
        // So that we can move the root game object around freely
        bindPoses[0] = bones[0].worldToLocalMatrix * transform.localToWorldMatrix;

        bones[1] = new GameObject("Upper").transform;
        bones[1].parent = transform;
        // Set the position relative to the parent
        bones[1].localRotation = Quaternion.identity;
        bones[1].localPosition = new Vector3(0, 5, 0);
        // The bind pose is bone's inverse transformation matrix
        // In this case the matrix we also make this matrix relative to the root
        // So that we can move the root game object around freely
        bindPoses[1] = bones[1].worldToLocalMatrix * transform.localToWorldMatrix;

        // assign the bindPoses array to the bindposes array which is part of the mesh.
        mesh.bindposes = bindPoses;

        // Assign bones and bind poses
        rend.bones = bones;
        rend.sharedMesh = mesh;

        // Assign a simple waving animation to the bottom bone
        AnimationCurve curve = new AnimationCurve();
        curve.keys = new Keyframe[] { new Keyframe(0, 0, 0, 0), new Keyframe(1, 3, 0, 0), new Keyframe(2, 0.0F, 0, 0) };

        // Create the clip with the curve
        AnimationClip clip = new AnimationClip();
        clip.SetCurve("Lower", typeof(Transform), "m_LocalPosition.z", curve);
        clip.legacy = true;
        clip.wrapMode = WrapMode.Loop;
        // Add and play the clip
        anim.AddClip(clip, "a");
        anim.Play("a");
    }
}