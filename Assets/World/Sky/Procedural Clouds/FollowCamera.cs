using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.World.Sky.Procedural_Clouds
{
    public class FollowCamera : MonoBehaviour
    {
        public GameObject cam;

        void LateUpdate()
        {
            transform.position = cam.transform.position;
        }
    }
}
