using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingText : MonoBehaviour {

    float startTime;
    bool run;
    float runTime = 3f;

    void OnEnable()
    {
        startTime = Time.time;
        gameObject.GetComponent<UnityEngine.UI.Text>().text = "Loading";
        run = true;
    }

    // Update is called once per frame
    void Update () {
		
        if (run)
        {
            if (Time.time-startTime < runTime)
            {
                if (Time.time - startTime > runTime*0.25f)
                {
                    gameObject.GetComponent<UnityEngine.UI.Text>().text = "Loading.";
                }
                if (Time.time - startTime > runTime*0.50f)
                {
                    gameObject.GetComponent<UnityEngine.UI.Text>().text = "Loading..";
                }
                if (Time.time - startTime > runTime * 0.75f)
                {
                    gameObject.GetComponent<UnityEngine.UI.Text>().text = "Loading...";
                }
            }
            else
            {
                run = false;
                gameObject.SetActive(false);
            }
        }

	}
}
