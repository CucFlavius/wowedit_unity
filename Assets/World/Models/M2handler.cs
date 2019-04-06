using Assets.Data.WoW_Format_Parsers.WMO;
using Assets.World.Terrain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using static Assets.Data.WoW_Format_Parsers.M2.M2_Data;

namespace Assets.World.Models
{
    public class M2handler : MonoBehaviour
    {
        public TerrainHandler terrainHandler;
        public bool busy;
        public Queue<M2QueueItem> M2ThreadQueue = new Queue<M2QueueItem>();
        public static Thread M2Thread;
        public Material defaultMaterial;

        private string currentM2datapath;
        private int currentM2uniqueID;
        private Vector3 currentM2position;
        private Quaternion currentM2rotation;
        private Vector3 currentM2scale;
        private Dictionary<string, Texture2D> LoadedM2Textures = new Dictionary<string, Texture2D>();
        private List<M2QueueItem> M2Clones = new List<M2QueueItem>();

        public class M2QueueItem
        {
            public string objectDataPath;
            public int uniqueID;
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
        }

        public void AddToQueue(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            M2QueueItem item = new M2QueueItem();
            item.objectDataPath = objectDataPath;
            item.uniqueID = uniqueID;
            item.Position = position;
            item.Rotation = rotation;
            item.Scale = scale;
            M2ThreadQueue.Enqueue(item);
        }

        public void M2ThreadRun(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            currentM2datapath = objectDataPath;
            currentM2uniqueID = uniqueID;
            currentM2position = position;
            currentM2rotation = rotation;
            currentM2scale = scale;

            if (!terrainHandler.LoadedM2s.ContainsKey(objectDataPath))
            {
                //ParseM2Block(); //nonthreaded - for testing purposes
                terrainHandler.LoadedM2s.Add(objectDataPath, null);
                M2Thread = new Thread(ParseM2Block);
                M2Thread.IsBackground = true;
                M2Thread.Priority = System.Threading.ThreadPriority.AboveNormal;
                M2Thread.Start();
            }
            else
            {
                CloneM2(objectDataPath, uniqueID, position, rotation, scale);
            }
        }

        void Start()
        {
            M2.ThreadWorking = false;
            M2ThreadQueue = new Queue<M2QueueItem>();
        }

        void Update()
        {
            if (M2ThreadQueue.Count > 0)
            {
                M2QueueItem queueItem = M2ThreadQueue.Dequeue();
                M2ThreadRun(queueItem.objectDataPath, queueItem.uniqueID, queueItem.Position, queueItem.Rotation, queueItem.Scale);
            }
            else if (M2ThreadQueue.Count == 0)
            {
                busy = false;
            }

            if (AllM2Data.Count > 0)
            {
                if (M2Thread != null)
                {
                    if (!M2Thread.IsAlive)
                    {
                        terrainHandler.frameBusy = true;
                        CreateM2Object(AllM2Data.Dequeue());
                    }
                }
                else
                {
                    CreateM2Object(AllM2Data.Dequeue());
                }
            }

            if (M2Clones.Count > 0)
            {
                List<M2QueueItem> RemoveElements = new List<M2QueueItem>();
                // Check if copies are Required //
                foreach (M2QueueItem item in M2Clones)
                {
                    if (terrainHandler.LoadedM2s.ContainsKey(item.objectDataPath))
                    {
                        if (terrainHandler.LoadedM2s[item.objectDataPath] != null)
                        {
                            M2QueueItem clone = item;
                            RemoveElements.Add(item);
                            GameObject instance = Instantiate(terrainHandler.LoadedM2s[item.objectDataPath]);
                            instance.transform.position = clone.Position;
                            instance.transform.rotation = clone.Rotation;
                            instance.transform.localScale = Vector3.one;
                            instance.transform.SetParent(terrainHandler.ADTBlockM2Parents[item.uniqueID].transform);
                        }
                    }
                }

                // Remove
                foreach (M2QueueItem removeItem in RemoveElements)
                {
                    M2Clones.Remove(removeItem);
                }
                RemoveElements.Clear();
            }
        }

        public void CloneM2(string objectDataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            M2QueueItem item = new M2QueueItem();
            item.objectDataPath = objectDataPath;
            item.uniqueID = uniqueID;
            item.Position = position;
            item.Rotation = rotation;
            item.Scale = scale;
            M2Clones.Add(item);
        }

        public void ParseM2Block()
        {
            M2.Load(currentM2datapath, currentM2uniqueID, currentM2position, currentM2rotation, currentM2scale);
        }

        public void CreateM2Object(M2Data data)
        {
            // M2 Object //
            GameObject M2Instance = new GameObject();
            terrainHandler.LoadedM2s[data.dataPath] = M2Instance;
            terrainHandler.LoadedM2s[data.dataPath].name = data.name;

            // LoD Group //
            LODGroup Lodgroup = terrainHandler.LoadedM2s[data.dataPath].AddComponent<LODGroup>();
            LOD[] lods = new LOD[1];
            Renderer[] renderers = new Renderer[data.submeshData.Count];

            // Bones //
            GameObject BonesRoot = new GameObject();
            BonesRoot.name = "Bones";
            BonesRoot.transform.position = Vector3.zero;
            BonesRoot.transform.rotation = Quaternion.identity;
            BonesRoot.transform.SetParent(M2Instance.transform);

            Transform[] bones = new Transform[data.m2CompBone.Count];
            Dictionary<int, GameObject> hierarchyList = new Dictionary<int, GameObject>();

            for (int boneN = 0; boneN < data.m2CompBone.Count; boneN++)
            {
                // name //
                string name = "bone_" + boneN;
                int key_bone_id = data.m2CompBone[boneN].key_bone_id;
                if (key_bone_id != -1)
                {
                    if (data.key_bone_lookup[key_bone_id] < (KeyBoneLookupList.Length - 1))
                        name = "bone_" + KeyBoneLookupList[key_bone_id];
                }

                bones[boneN] = new GameObject(name).transform;
                if (data.m2CompBone[boneN].parent_bone == -1)
                {
                    bones[boneN].SetParent(BonesRoot.transform);
                }
                else
                    bones[boneN].SetParent(bones[data.m2CompBone[boneN].parent_bone]);

                bones[boneN].transform.position = data.m2CompBone[boneN].pivot;
            }

            // Mesh //
            GameObject MesheObject = new GameObject();
            MesheObject.name = "Mesh";
            MesheObject.transform.position = Vector3.zero;
            MesheObject.transform.rotation = Quaternion.identity;
            MesheObject.transform.SetParent(M2Instance.transform);
            Mesh m = new Mesh();
            m.vertices = data.meshData.pos.ToArray();
            m.normals = data.meshData.normal.ToArray();
            m.uv = data.meshData.tex_coords.ToArray();
            m.uv2 = data.meshData.tex_coords2.ToArray();
            m.subMeshCount = data.submeshData.Count;

            // Submeshes //
            for (int batch = 0; batch < data.submeshData.Count; batch++)
            {
                m.SetTriangles(data.submeshData[batch].triList, batch, true);
            }

            // Skinned Mesh Renderer //
            SkinnedMeshRenderer rend = MesheObject.AddComponent<SkinnedMeshRenderer>();
            rend.sharedMesh = m;
            rend.bones = bones;
            rend.rootBone = BonesRoot.transform.GetChild(0);

            // Bounds //
            Bounds meshBounds = new Bounds();
            meshBounds.min = data.bounding_box.min;
            meshBounds.max = data.bounding_box.max;
            m.bounds = meshBounds;
            rend.localBounds = meshBounds;

            // Bone Weights //
            BoneWeight[] weights = new BoneWeight[m.vertices.Length];
            for (int bw = 0; bw < m.vertices.Length; bw++)
            {
                weights[bw].boneIndex0 = data.meshData.bone_indices[bw][0];
                weights[bw].weight0 = data.meshData.bone_weights[bw][0];
                weights[bw].boneIndex1 = data.meshData.bone_indices[bw][1];
                weights[bw].weight1 = data.meshData.bone_weights[bw][1];
                weights[bw].boneIndex2 = data.meshData.bone_indices[bw][2];
                weights[bw].weight2 = data.meshData.bone_weights[bw][2];
                weights[bw].boneIndex3 = data.meshData.bone_indices[bw][3];
                weights[bw].weight3 = data.meshData.bone_weights[bw][3];
            }
            m.boneWeights = weights;

            // Bind Poses //
            Matrix4x4[] bindPoses = new Matrix4x4[bones.Length];
            for (int bp = 0; bp < bindPoses.Length; bp++)
            {
                bindPoses[bp] = bones[bp].worldToLocalMatrix * transform.localToWorldMatrix;
            }
            m.bindposes = bindPoses;

            // Animations //
            Animation anim = M2Instance.AddComponent<Animation>();

            AnimationClip[] clips = new AnimationClip[data.numberOfAnimations];

            /*
            for (int a = 0; a < data.numberOfAnimations; a++)
            {
                AnimationClip clip = new AnimationClip();
                for (int p = 0; p < data.position_animations.Count; p++)
                {
                    AnimationCurve position_x_curve = new AnimationCurve();
                    AnimationCurve position_y_curve = new AnimationCurve();
                    AnimationCurve position_z_curve = new AnimationCurve();
                    Keyframe[] position_x_keyframes = new Keyframe[data.position_animations[p][a].timeStamps.Count];
                    Keyframe[] position_y_keyframes = new Keyframe[data.position_animations[p][a].timeStamps.Count];
                    Keyframe[] position_z_keyframes = new Keyframe[data.position_animations[p][a].timeStamps.Count];
                    for (int t = 0; t < position_x_keyframes.Length; t++)
                    {
                        position_x_keyframes[t] = new Keyframe(data.position_animations[p][a].timeStamps[t], data.position_animations[p][a].values[t].x);
                        position_y_keyframes[t] = new Keyframe(data.position_animations[p][a].timeStamps[t], data.position_animations[p][a].values[t].y);
                        position_z_keyframes[t] = new Keyframe(data.position_animations[p][a].timeStamps[t], data.position_animations[p][a].values[t].z);
                    }
                    position_x_curve.keys = position_x_keyframes;
                    position_y_curve.keys = position_y_keyframes;
                    position_z_curve.keys = position_z_keyframes;
                    clip.SetCurve(bones[p].name, typeof(Transform), "m_LocalPosition.x", position_x_curve);
                    clip.SetCurve(bones[p].name, typeof(Transform), "m_LocalPosition.y", position_y_curve);
                    clip.SetCurve(bones[p].name, typeof(Transform), "m_LocalPosition.z", position_z_curve);
                }

                clip.legacy = true;
                clip.wrapMode = WrapMode.Loop;
                anim.AddClip(clip
                    , "anim_" + a);
            }
            anim.Play("anim_0");
            */

            // Materials //
            Material[] materials = new Material[data.submeshData.Count];
            for (int matD = 0; matD < materials.Length; matD++) { materials[matD] = new Material(Shader.Find("WoWEdit/WMO/S_Diffuse")); ; }  // fill with default material
            rend.materials = materials;

            if (M2.HasTextures)
            {
                // Textures //
                for (int tex = 0; tex < data.submeshData.Count; tex++)
                {
                    string textureName = data.m2Tex[data.textureLookupTable[data.m2BatchIndices[data.m2BatchIndices[tex].M2Batch_submesh_index].M2Batch_texture]].filename;
                    Texture2Ddata tdata = data.m2Tex[data.textureLookupTable[data.m2BatchIndices[data.m2BatchIndices[tex].M2Batch_submesh_index].M2Batch_texture]].texture2Ddata;
                    if (textureName != null && textureName != "" && tdata.TextureData != null)
                    {
                        if (LoadedM2Textures.ContainsKey(textureName))
                        {
                            materials[tex].SetTexture("_MainTex", LoadedM2Textures[textureName]);
                        }
                        else
                        {
                            try
                            {
                                Texture2D texture = new Texture2D(tdata.width, tdata.height, tdata.textureFormat, tdata.hasMipmaps);
                                texture.LoadRawTextureData(tdata.TextureData);
                                texture.Apply();
                                LoadedM2Textures[textureName] = texture;
                                materials[tex].SetTexture("_MainTex", texture);
                            }
                            catch
                            {
                                Debug.Log("Error: Loading RawTextureData @ M2handler");
                            }
                        }
                    }
                }
            }

            // DEBUG - Draw Bones //
            BonesRoot.AddComponent<DrawBones>();

            // Object Transforms //
            terrainHandler.LoadedM2s[data.dataPath].transform.position = data.position;
            terrainHandler.LoadedM2s[data.dataPath].transform.rotation = data.rotation;
            terrainHandler.LoadedM2s[data.dataPath].transform.localScale = data.scale;
            if (data.uniqueID != -1)
            {
                if (terrainHandler.ADTBlockM2Parents[data.uniqueID].transform != null)
                    terrainHandler.LoadedM2s[data.dataPath].transform.SetParent(terrainHandler.ADTBlockM2Parents[data.uniqueID].transform);
                else
                    Destroy(terrainHandler.LoadedM2s[data.dataPath]);
            }

            terrainHandler.frameBusy = false;
        }

        public void StopLoading()
        {
            AllM2Data.Clear();
            M2ThreadQueue.Clear();
        }
    }
}