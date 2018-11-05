using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public float avgFrameRate;
    public Text display_Text;
    private float deltaTime = 0.0f;

    public void Start()
    {
        InvokeRepeating("RecalculateFPS", 2.0f, 1.0f);
    }

    public void RecalculateFPS()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        avgFrameRate = 1.0f / deltaTime;
    }

    public void OnGUI()
    {
        display_Text.text = string.Format("FPS: {0:0.0}", avgFrameRate);
    }
}