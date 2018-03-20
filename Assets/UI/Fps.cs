using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour
{

    string label = "";

    int frameCount = 0;
    double dt = 0.0f;
    double fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.

    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            fps = frameCount / dt;
            label = fps.ToString();
            frameCount = 0;
            dt -= 1.0 / updateRate;
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUI.Label(new Rect(w - 200, 20, w, h * 2 / 100), label);
    }
}