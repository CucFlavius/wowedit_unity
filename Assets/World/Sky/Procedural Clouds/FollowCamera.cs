using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject cam;

	void LateUpdate ()
    {
        transform.position = cam.transform.position;
	}
}
