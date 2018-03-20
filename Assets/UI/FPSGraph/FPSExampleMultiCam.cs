using UnityEngine;
using System.Collections;

public class FPSExampleMultiCam : MonoBehaviour {
	public Camera[] cameras;

	private int cameraIter = 0;
	private float cameraSwitchIter;
	

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		cameras[1].enabled = false;
		cameras[2].enabled = false;
		cameras[1].GetComponent<AudioListener>().enabled = false;
		cameras[2].GetComponent<AudioListener>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(cameraSwitchIter>5f){
			cameraSwitchIter = 0f;

			cameras[ cameraIter ].enabled = false;
			cameras[ cameraIter ].GetComponent<AudioListener>().enabled = false;
			//Destroy( cameras[ cameraIter ].gameObject.GetComponent<AudioListener>() );

			cameraIter++;
			if(cameraIter>2){
				cameraIter = 0;
			}
			Debug.Log("Switching to Camera"+cameraIter);
			
			cameras[ cameraIter ].enabled = true;
			cameras[ cameraIter ].GetComponent<AudioListener>().enabled = true;
			//cameras[ cameraIter ].gameObject.AddComponent( typeof(AudioListener) );
		}
	
		cameraSwitchIter += Time.deltaTime;
	}
}
