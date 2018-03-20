using UnityEngine;
using System.Collections;
using DentedPixelPerformance;

public class FPSExamplePerformanceBreakdown : MonoBehaviour {

	private int animateCount = 6000;

	void Awake(){
		StartCoroutine(showGraph(4.0f)); 
	}

	// Use this for initialization
	void Start () {
		int iter = 0;
		int side = System.Convert.ToInt32( Mathf.Pow( animateCount*1.0f, 1.0f/3.0f ) );
		Vector3 p;
		for(int i = 0; i < side; i++){
			for(int j = 0; j < side; j++){
				for( int k = 0; k < side; k++){
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Destroy( cube.GetComponent( typeof(BoxCollider) ) as Component );
					p = new Vector3(i*3,j*3,k*3);
					cube.transform.position = p;
					cube.name = "cube"+iter;
					iter++;
				}
			}
		}
	}

	IEnumerator showGraph(float delayTime){
		yield return new WaitForSeconds(delayTime);
		FPSGraphC fpsGraph = gameObject.GetComponent(typeof(FPSGraphC)) as FPSGraphC;
		fpsGraph.showPerformance();
	}
}
