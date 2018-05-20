using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static partial class M2
{
    public static Queue<M2Data> AllM2Data = new Queue<M2Data>();
    public static M2Data m2Data = new M2Data();

    public struct M2Data
    {
        // Object //
        
    }

    public struct M2Array
    {
        public int size;
        public int offset;
    }

    public struct M2Bounds
    {
        public BoundingBox extent;
        public float radius;
    }

    public struct M2TrackBase
    {
        public UInt16 trackType;
        public UInt16 loopIndex;
        M2Array sequenceTimes;
    }
    
    public struct M2Vertex 
    { 
        public C3Vector pos; 
        public uint8 bone_weights[4]; 
        public uint8 bone_indices[4]; 
        public C3Vector normal; 
        public C2Vector tex_coords[2]; // two textures, depending on shader used
    }
}
