using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DentedPixelPerformance {

// FPS Graph - Performance Analyzer Tool - Version 0.975
//
// To use FPS Graph simply add this script to your main camera. 
//    detailed explanation: Select the camera used in your scene then go to the inspector window click on the component menu and down to scripts you should find FPSGraph 
// Options:
//    Audio Feedback: This allows you to audibly "visualize" the perforamnce of the scene
//    Audio Feedback Volume: This specifies how loud the audio feedback is, from 0.0-1.0
//    Graph Multiply: This specifies how zoomed in the graph is, the default is the graph is multiplyed by 2x, meaning every pixel is doubled.
//    Graph Position: This specifies where the graph sits on the screen examples: x:0.0, y:0.0 (top-left) x:1.0, y:0.0 (top-right) x:0.0, y:1.0 (bottom-left) x:1.0 y:1.0 (bottom-right)
//    Frame History Length: This is the length of FPS that is displayed (Set this to a low amount if you are targeting older mobile devices)
public class FPSGraphC : MonoBehaviour
{
	public enum GraphPositioning{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}


	Material mat;

	public void CreateLineMaterial() {
	    mat = new Material(Shader.Find("GUI/Text Shader") );
	}

	public bool switchToMainCamera = true;
	public bool showPerformanceOnClick = true;
	public bool showFPSNumber = false;
	public bool audioFeedback = false;
	public float audioFeedbackVolume = 0.5f;
	public int graphMultiply = 2;
	public GraphPositioning graphPositioning;
	public Vector2 graphPosition = new Vector2(0.0f, 0.0f);
	public int frameHistoryLength = 120;

	public void copyFrom( FPSGraphC from ){
		this.switchToMainCamera = from.switchToMainCamera;
		this.showPerformanceOnClick = from.showPerformanceOnClick;
		this.showFPSNumber = from.showFPSNumber;
		this.audioFeedback = from.audioFeedback;
		this.audioFeedbackVolume = from.audioFeedbackVolume;
		this.graphMultiply = from.graphMultiply;
		this.graphPositioning = from.graphPositioning;
		this.graphPosition = from.graphPosition;
		this.frameHistoryLength = from.frameHistoryLength;
		this.CpuColor = from.CpuColor;
		this.RenderColor = from.RenderColor;
		this.OtherColor = from.OtherColor;
		this.useMinFPS = from.useMinFPS;
		this.minFPS = from.minFPS;
	}

	public Color CpuColor = new Color( 53.0f/ 255.0f, 136.0f / 255.0f, 167.0f / 255.0f , 1.0f );
	public Color RenderColor = new Color( 112.0f/ 255.0f, 156.0f / 255.0f, 6.0f / 255.0f, 1.0f );
	public Color OtherColor = new Color( 193.0f/ 255.0f, 108.0f / 255.0f, 1.0f / 255.0f, 1.0f );

	public bool useMinFPS = false;
	public int minFPS = 20;

	// Re-created Assets
	private static readonly int[] numberBits = new[] {1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0,1,0,1,1,1,0,1,1,1,0,0,1,0,0,1,1,1,0,0,0,1,1,0,1,0,0,1,0,0,0,1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,0,1,0,0,1,0,1,0,0,0,1,1,0,1,0,0,1,0,0,0,0,1,0,0,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0,1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,0,0,1,0,1,0,0,0,1,0,1,0,1,0,1,0,0,0,1,0,0,0,0,0,1,0,1,0,1,0,1,0,1,1,1,1,0,0,1,0,0,0,1,0,0,1,1,1,0,0,0,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1};
	private static readonly int[] fpsBits = new[] {1,0,0,0,1,0,0,0,1,1,1,1,1,0,0,1,1,1,0,0,1,1,1,0,0,0,1,0,1,0,1,1,0,0,1,1,0,1,1,1,0,1,1,1};
	private static readonly int[] mbBits = new[] {1,0,1,0,1,0,1,1,1,1,0,1,0,1,0,1,0,1,1,1,1,1,1,0,1,1,1,0,0,0,0,0,0,1,0,0};
	private static readonly Color[] graphKeys = new[] {new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f)};

	private static AudioClip audioClip;
	private static AudioSource audioSource;

	private static Texture2D graphTexture;
	private static int graphHeight = 100;

	private static int m_FpsAccumulator = 0;
	private static float r_lastRealTime = 0;
    private static float m_FpsNextPeriod = 0;
    private static int m_CurrentFps;
    const float fpsMeasurePeriod = 0.5f;

	private static int[,] textOverlayMask;

	private static float[,] dtHistory;
	private static int[] gcHistory;
	private static int i,j,x,y;
	private static float val;
	private static Color color;
	private static Color32 color32;
	private static float maxFrame = 0.0f;
	private static float yMulti;
	private static float beforeRender;
	private static float[] fpsVals = new float[3];
	private static float x1;
	private static float x2;
	private static float y1;
	private static float y2;
	private static float xOff;
	private static float yOff;
	private static float yGUI;
	private static int[] lineY = new int[]{25, 50, 99};
	private static int[] lineY2 = new int[]{21, 46, 91};
	private static int[] keyOffX = new int[]{61,34,1};
	private static string[] splitMb;
	private static int first;
	private static int second;
	private static float lowestDt = 10000.0f;
	private static float highestDt;
	private static float totalDt;
	private static int totalFrames = 0;
	private static float totalGPUTime = 0.0f;
	private static float totalCPUTime = 0.0f;
	private static float totalOtherTime = 0.0f;
	private static float totalTimeElapsed = 0.0f;
	private static float totalSeconds;
	private static float renderSeconds;
	private static float lateSeconds;
	private static float dt;
	private static int frameCount;
	private static int frameIter = 1;
	private static float eTotalSeconds;
	private static int lastCollectionCount = -1;
	private static float mem;

	private static Color[] fpsColors;
	private static Color[] fpsColorsTo;
	
	private static Color lineColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	private static Color darkenedBack = new Color(0.0f,0.0f,0.0f,0.5f);
	private static Color darkenedBackWhole = new Color(0.0f,0.0f,0.0f,0.5f);

	private static Color32[] colorsWrite;

	private static Rect graphSizeGUI;

	private static System.Diagnostics.Stopwatch stopWatch;
	private static float lastElapsed;
	private static float fps;
	private static int graphSizeMin;
	enum FPSGraphViewMode{
		graphing,
		totalperformance,
		assetbreakdown
	}
	private static FPSGraphViewMode viewMode = FPSGraphViewMode.graphing;

	private void checkIfCurrentCamera(){
		if(switchToMainCamera){
			
			Camera mainCam = Camera.current!=null ? Camera.current : Camera.main;
			if(mainCam!=null && mainCam.gameObject != gameObject && mainCam.name.IndexOf("SceneCamera") < 0){
				switchCamera( mainCam );
			}
		}
	}

	private void switchCamera( Camera newCam ){
		// Debug.Log("current:"+newCam.name+" gameObject:"+gameObject.name);
		audioSource.Stop();
		audioSource.enabled = false;
		Destroy( audioSource );

		FPSGraphC newGraph = newCam.gameObject.AddComponent< FPSGraphC >();
		newGraph.initAudio();
		newGraph.copyFrom( this );

		this.audioFeedback = false;
		Destroy( this );
	}

	void Awake(){
		if(gameObject.GetComponent<Camera>()==null)
			Debug.LogWarning("FPS Graph needs to be attached to a Camera object");

		CreateLineMaterial();

		fpsColors = new[] { RenderColor, CpuColor, OtherColor };
		fpsColorsTo = new[] {fpsColors[0]*0.7f, fpsColors[1]*0.7f, fpsColors[2]*0.7f};
	}

	void Start () {
		m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
		r_lastRealTime = Time.realtimeSinceStartup;

		graphSizeMin = frameHistoryLength > 95 ? frameHistoryLength : 95;

		textOverlayMask = new int[graphHeight, graphSizeMin];
		reset();

		graphTexture = new Texture2D( graphSizeMin, 7, TextureFormat.ARGB32, false, false);
		colorsWrite = new Color32[ graphTexture.width * 7 ];
		graphTexture.filterMode = FilterMode.Point;
		graphSizeGUI = new Rect( 0f, 0f, graphTexture.width * graphMultiply, graphTexture.height * graphMultiply );

		addFPSAt(14,23);
		addFPSAt(14,48);
		addFPSAt(14,93);
		if(showFPSNumber){
			addFPSAt(14,0);
		}

		for (int x = 0; x < graphTexture.width; ++x) {
			for (int y= 0; y < 7; ++y) {
				if(x < 95 && y < 5){
					color = graphKeys[ y*95 + x ];
				}else{
					color.a = 0.0f;
				}
				graphTexture.SetPixel(x, y, color);
				colorsWrite[ (y) * graphTexture.width + x ] = color;
			}
		}
		graphTexture.Apply();

	  	if(audioFeedback)
	    	initAudio();

	    /*while (true) {
	        yield return new WaitForEndOfFrame();

	        eTotalSeconds = (float)stopWatch.Elapsed.TotalSeconds;
			dt = eTotalSeconds - lastElapsed;
	    }*/
	}

	void reset(){
		dtHistory = new float[3, frameHistoryLength];
		gcHistory = new int[frameHistoryLength];

	    stopWatch = new System.Diagnostics.Stopwatch();
	    stopWatch.Start();

	    lowestDt = 10000.0f;
		highestDt = 0f;
		totalDt = 0f;
		totalFrames = 0;
		totalGPUTime = 0f;
		totalCPUTime = 0f;
		totalOtherTime = 0f;
		totalTimeElapsed = 0f;
		frameIter = 0;
		frameCount = 1;
	}

	public void initAudio(){
		
		AnimationCurve volumeCurve = AnimationCurve.Linear( 0f, 1f, 1f, 1f);
		AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));
		audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve);

		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = audioClip;
	}

	private static int xExtern;
	private static int yExtern;
	private static int startAt;
	private static int yOffset;
	private static int xLength;

	void addFPSAt( int startX, int startY){
		yExtern = startY;
		for(int y=0; y < 4; y++){
			xExtern = startX;
			yOffset = y*11;
			for(int x=0; x < 11; x++){
				textOverlayMask[yExtern, xExtern] = fpsBits[ yOffset + x ];
				xExtern++;
			}

			yExtern++;
		}
	}

	private static int k,z;
	void addNumberAt( int startX, int startY, int num, bool isLeading ){
		if(isLeading && num==0)
			num = -1;
		startAt = num * 4;
		xLength = startAt + 3;

		yExtern = startY;
		for(z=0; z < 5; z++){
			xExtern = startX;
			yOffset = z*39;
			for(k=startAt; k < xLength; k++){
				//textOverlayMask[yExtern, xExtern] = num==-1 ? 0 : numberBits[ yOffset + x ];
				if(num!=-1 && numberBits[ yOffset + k ] == 1){
					x1 = xExtern * graphMultiply + xOff;
					y1 = yExtern * graphMultiply + yOff;
					GL.Vertex3(x1,y1,0);
					GL.Vertex3(x1,y1+1* graphMultiply,0);
					GL.Vertex3(x1+1* graphMultiply,y1+1* graphMultiply,0);
					GL.Vertex3(x1+1* graphMultiply,y1,0);
				}
				xExtern++;
			}

			yExtern++;
		}
	}

	void addPeriodAt( int startX, int startY){
	    x1 = startX*graphMultiply + xOff;
	    x2 = (startX+1)*graphMultiply + xOff;
	    y1 = startY*graphMultiply + yOff;
	    y2 = (startY-1)*graphMultiply + yOff;
	    GL.Vertex3(x1,y1,0);
	    GL.Vertex3(x1,y2,0);
	    GL.Vertex3(x2,y2,0);
	    GL.Vertex3(x2,y1,0);
	}

	void Update () {
		checkIfCurrentCamera();

		if(viewMode == FPSGraphViewMode.graphing){
			lastElapsed = (float)stopWatch.Elapsed.TotalSeconds;
			//if(frameCount>4){
				//Debug.Log("Update seconds:"+stopWatch.Elapsed.TotalSeconds);
				//Debug.Log("Update lastElapsed:"+lastElapsed);

				float dtLocal = Time.realtimeSinceStartup - r_lastRealTime;
           		float calculateFPS = (int) (1f / dtLocal);
           		//Debug.Log("dt:"+dt+" dtLocal:"+dtLocal);
           		dt = dtLocal;
				
				if(frameCount>6)
				    dtHistory[0, frameIter] = dt;

			    if(dt<lowestDt){
					lowestDt = dt;
				}else if(dt>highestDt){
					highestDt = dt;
				}

				m_FpsAccumulator++;
				if (Time.realtimeSinceStartup > m_FpsNextPeriod){
               		m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
               		
               		//Debug.Log("m_CurrentFps:"+m_CurrentFps+" calculateFPS:"+calculateFPS);
               		m_CurrentFps = (int)calculateFPS;
	                m_FpsAccumulator = 0;
	                m_FpsNextPeriod += fpsMeasurePeriod;
				}
				r_lastRealTime = Time.realtimeSinceStartup;
				totalGPUTime += dtHistory[0, frameIter]-dtHistory[1, frameIter];
				totalCPUTime += dtHistory[1, frameIter]-dtHistory[2, frameIter];
				totalOtherTime += dtHistory[2, frameIter];

				if(lastCollectionCount!=System.GC.CollectionCount(0) ){
					gcHistory[frameIter] = 1;
					lastCollectionCount = System.GC.CollectionCount(0);
				}

				totalDt += dt;
				totalFrames++;

				frameIter++;

				if(audioFeedback && frameCount>6){
					if(audioClip==null)
						initAudio();

					if(audioSource.isPlaying==false)
						audioSource.Play();
				}else if(audioSource && audioSource.isPlaying){
					audioSource.Stop();
				}

				if(audioClip){
					audioSource.pitch = Mathf.Clamp( dt * 90.0f - 0.7f, 0.1f, 50.0f );
					audioSource.volume = audioFeedbackVolume;
				}
				//Debug.Log("audioSource.pitch:"+audioSource.pitch);

				if(frameIter>=frameHistoryLength)
					frameIter = 0;

				beforeRender = (float)stopWatch.Elapsed.TotalSeconds;
			//}
			
			frameCount++;
		}

		//Debug.Log("yMulti:"+yMulti + " maxFrame:"+maxFrame);
	}

	void LateUpdate(){
		//Debug.Log("LateUpdate seconds:"+stopWatch.Elapsed.TotalSeconds);

		eTotalSeconds = (float)stopWatch.Elapsed.TotalSeconds;
		dt = (eTotalSeconds - beforeRender);
		//Debug.Log("OnPostRender time:"+dt);

		if(frameCount>6)
			dtHistory[2, frameIter] = dt;

		beforeRender = eTotalSeconds;
	}

	IEnumerator OnPostRender(){
		if(viewMode == FPSGraphViewMode.graphing)
			yield return new WaitForEndOfFrame();
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadPixelMatrix();
		GL.Begin(GL.QUADS);

		if(viewMode == FPSGraphViewMode.graphing){
			if(graphPositioning==GraphPositioning.TopLeft || graphPositioning==GraphPositioning.BottomLeft){
				xOff = graphPosition.x;
			}else{
				xOff = w - frameHistoryLength*graphMultiply - graphPosition.x;
			}
			
			if(graphPositioning==GraphPositioning.TopLeft || graphPositioning==GraphPositioning.TopRight){
				yOff = h - (100 * graphMultiply + graphPosition.y);
			}else{
				yOff = (8*graphMultiply + graphPosition.y);
			}

			// Shadow for whole graph
			GL.Color( darkenedBackWhole );
			GL.Vertex3(xOff, yOff-8*graphMultiply,0);
			GL.Vertex3(xOff,100 * graphMultiply + yOff,0);
			GL.Vertex3(graphSizeMin * graphMultiply + xOff,100.0f* graphMultiply + yOff,0);
			GL.Vertex3(graphSizeMin * graphMultiply + xOff,yOff-8*graphMultiply ,0);
		    
		    maxFrame = 0.0f;
		    for (x = 0; x < frameHistoryLength; ++x) {
				totalSeconds = dtHistory[ 0, x ];

				if(totalSeconds>maxFrame)
					maxFrame = totalSeconds;
				totalSeconds *= yMulti;
				fpsVals[0] = totalSeconds;

				renderSeconds = dtHistory[ 1, x ];
				renderSeconds *= yMulti;
				fpsVals[1] = renderSeconds;

				lateSeconds = dtHistory[ 2, x ];
				lateSeconds *= yMulti;
				fpsVals[2] = lateSeconds;

				i = x - frameIter - 1;
				if(i<0)
					i = frameHistoryLength + i;
			
				x1 = i * graphMultiply + xOff;
				x2 = (i+1) * graphMultiply + xOff;
				
				for(j = 0; j < fpsVals.Length; j++){
					y1 = j < fpsVals.Length - 1 ? fpsVals[j+1] * graphMultiply + yOff : yOff;
					y2 = fpsVals[j] * graphMultiply + yOff;

					GL.Color(fpsColorsTo[j]);
					GL.Vertex3(x1,y1,0);
					GL.Vertex3(x2,y1,0);
					GL.Color(fpsColors[j]);
					GL.Vertex3(x2,y2,0);
					GL.Vertex3(x1,y2,0);
				}

				// Garbage Collections
				if(gcHistory[x]==1){
					y1 = -0*graphMultiply+yOff;
					y2 = -2*graphMultiply+yOff;
					GL.Color(Color.red);
					GL.Vertex3(x1,y1,0);
					GL.Vertex3(x2,y1,0);
					GL.Vertex3(x2,y2,0);
					GL.Vertex3(x1,y2,0);
				}

		      //Debug.Log("x:"+(x-frameIter));
		    }

		  	// Round to nearest relevant FPS
		  	if(useMinFPS==false){
			   	if(maxFrame < 1.0f/120.0f){
			   		maxFrame = 1.0f/120.0f;
			   	}else if(maxFrame < 1.0f/60.0f){
			   		maxFrame = 1.0f/60.0f;
			   	}else if(maxFrame < 1.0f/30.0f){
			   		maxFrame = 1.0f/30.0f;
			   	}else if(maxFrame < 1.0f/15.0f){
			   		maxFrame = 1.0f/15.0f;
			   	}else if(maxFrame < 1.0f/10.0f){
			   		maxFrame = 1.0f/10.0f;
			   	}else if(maxFrame < 1.0f/5.0f){
			   		maxFrame = 1.0f/5.0f;
			   	}

			    yMulti = graphHeight / maxFrame;
			}else{
				maxFrame = 1.0f/minFPS;
				yMulti = graphHeight / maxFrame;
			}

		    
		    // Add Horiz Lines
			GL.Color( lineColor );
			x1 = 28 * graphMultiply + xOff;
			x2 = graphSizeMin*graphMultiply + xOff;
			for(i = 0; i < lineY.Length; i++){
				y1 = lineY[i] * graphMultiply + yOff;
				y2 = (lineY[i]+1)* graphMultiply + yOff;
				GL.Vertex3(x1,y1,0);
				GL.Vertex3(x1,y2,0);
				GL.Vertex3(x2,y2,0);
				GL.Vertex3(x2,y1,0);
			}

			// Add FPS Shadows
			GL.Color( darkenedBack );
			x2 = 27 * graphMultiply + xOff;
			for(i = 0; i < lineY.Length; i++){
				y1 = lineY2[i] * graphMultiply + yOff;
				y2 = (lineY2[i]+9)* graphMultiply + yOff;
				GL.Vertex3(xOff, y1,0);
				GL.Vertex3(xOff, y2,0);
				GL.Vertex3(x2, y2,0);
				GL.Vertex3(x2, y1,0);
			}

			// Add Key Boxes
		    for(i = 0; i < keyOffX.Length; i++){
		        x1 = keyOffX[i]*graphMultiply + xOff + 1*graphMultiply;
		        x2 = (keyOffX[i]+4)*graphMultiply + xOff + 1*graphMultiply;
		        y1 = (5)*graphMultiply + yOff - 9*graphMultiply;
		        y2 = (1)*graphMultiply + yOff - 9*graphMultiply;
		        GL.Color( fpsColorsTo[i] );
		        GL.Vertex3(x1,y1,0);
		        GL.Vertex3(x1,y2,0);
		        GL.Vertex3(x2,y2,0);
		        GL.Vertex3(x2,y1,0);
		    }

		    for(i = 0; i < keyOffX.Length; i++){
		        x1 = keyOffX[i]*graphMultiply + xOff;
		        x2 = (keyOffX[i]+4)*graphMultiply + xOff;
		        y1 = (5)*graphMultiply + yOff - 8*graphMultiply;
		        y2 = (1)*graphMultiply + yOff - 8*graphMultiply;
		        GL.Color( fpsColors[i] );
		        GL.Vertex3(x1,y1,0);
		        GL.Vertex3(x1,y2,0);
		        GL.Vertex3(x2,y2,0);
		        GL.Vertex3(x2,y1,0);
		    }

		    GL.Color(Color.white);
		    for (x = 0; x < graphTexture.width; ++x) {
				for (y = 0; y < graphHeight; ++y) {
					// Draw Text
					if(textOverlayMask[y,x] == 1){
						x1 = x*graphMultiply + xOff;
						x2 = x*graphMultiply + 1*graphMultiply + xOff;
						y1 = y*graphMultiply + yOff;
						y2 = y*graphMultiply + 1*graphMultiply + yOff;
						GL.Vertex3(x1,y1,0);
						GL.Vertex3(x1,y2,0);
						GL.Vertex3(x2,y2,0);
						GL.Vertex3(x2,y1,0);
					}
				}
		    }

		    // Draw Mb
		    for (x = 0; x < 9; ++x) {
		        for (y = 0; y < 4; ++y) {
		            if(mbBits[y*9 + x]==1){
		                x1 = x*graphMultiply + xOff + 111*graphMultiply;
		                x2 = x*graphMultiply + 1*graphMultiply + xOff + 111*graphMultiply;
		                y1 = y*graphMultiply + yOff + -7*graphMultiply;
		                y2 = y*graphMultiply + 1*graphMultiply + yOff + -7*graphMultiply;
		                GL.Vertex3(x1,y1,0);
		                GL.Vertex3(x1,y2,0);
		                GL.Vertex3(x2,y2,0);
		                GL.Vertex3(x2,y1,0);
		            }
		        }
		    }

		    if(maxFrame>0){
				fps = Mathf.Round(1.0f/maxFrame);
				
				if(showFPSNumber){
					// Debug.Log("m_CurrentFps:"+m_CurrentFps + " 1:"+(int)((m_CurrentFps / 100)%10));
					addNumberAt( 1, 0, (int)((m_CurrentFps / 100)%10), true );
					addNumberAt( 5, 0, (int)((m_CurrentFps / 10.0)%10), false );
					addNumberAt( 9, 0, (int)(m_CurrentFps % 10), false );
				}

				addNumberAt( 1, 93, (int)((fps / 100)%10), true );
				addNumberAt( 5, 93, (int)((fps / 10.0)%10), true );
				addNumberAt( 9, 93, (int)(fps % 10), false );

				fps *= 2;
				addNumberAt( 1, 48, (int)((fps / 100)%10), true );
				addNumberAt( 5, 48, (int)((fps / 10)%10), true );
				addNumberAt( 9, 48, (int)(fps % 10), false );

				fps *= 1.5f;
				addNumberAt( 1, 23, (int)((fps / 100)%10), true );
				addNumberAt( 5, 23, (int)((fps / 10)%10), true );
				addNumberAt( 9, 23, (int)(fps % 10), false );
				
				#if UNITY_PRO_LICENSE
				mem = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / 1000000.0f;
				#else
				mem = ( System.GC.GetTotalMemory(false) ) / 1000000.0f;
				#endif
				

				if(mem<1.0){
		            splitMb = mem.ToString("F2").Split("."[0]);

		            if(splitMb[1][0]=="0"[0]){
		                first = 0;
		                second = int.Parse( splitMb[1] );
		            }else{
		                first = int.Parse( splitMb[1] );
		                second = first%10;
		                first = (first/10)%10;
		            }
		            
		            addPeriodAt( 100, -6);
		            addNumberAt( 102, -7, first, false );
		            addNumberAt( 106, -7, second, false );
		        }else if(mem<100.0f){
		        	splitMb = mem.ToString("F1").Split("."[0]);
		            first = int.Parse( splitMb[0] );

		            if(first>=10)
		                addNumberAt( 96, -7, first%100/10, false );
		            addNumberAt( 100, -7, first%10, false );
		            addPeriodAt( 104, -6);
		            addNumberAt( 106, -7, int.Parse( splitMb[1] ), false );
		        }else{
		        	first = (int)mem;

		        	addNumberAt( 96, -7, (int)(first/100), false );
		        	addNumberAt( 100, -7, first%100/10, false );
		        	addNumberAt( 104, -7, first%10, false );
		        }
		    }

		    yGUI = ( graphPositioning==GraphPositioning.TopLeft || graphPositioning==GraphPositioning.TopRight ) ? (graphMultiply*100 + graphPosition.y) : h - (graphMultiply*8 + graphPosition.y);
			
			// Draw Key Text
			for (x = 0; x < 95; ++x) {
				for (y = 0; y < 7; ++y) {
					if(x < 95 && y < 5){
						color = graphKeys[ y*95 + x ];
					}else{
						color.a = 0.0f;
					}
					if(color.a>0.0f){
						addPeriodAt( x, y - 6 );
					}
				}
			}

		    GL.End();
		    GL.PopMatrix();

		    // GUI.DrawTexture( new Rect(xOff, yGUI, graphSizeGUI.width, graphSizeGUI.height), graphTexture );


	    }else{
	    	if(circleGraphLabels==null)
	    		circleGraphLabels = new Vector2[3];
	    	// draw background
	    	Rect sRect = new Rect(w*0.05f,h*0.05f,w*0.9f,h*0.9f);
	    	GL.Color(new Color(0f,0f,0f,0.8f));
	    	GL.Vertex3(sRect.x,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y+sRect.height,0f);
	    	GL.Vertex3(sRect.x,sRect.y+sRect.height,0f);

	    	if(viewMode == FPSGraphViewMode.totalperformance){
	    		// circle graph
		    	float totalTimes = totalCPUTime + totalGPUTime + totalOtherTime;
		    	float[] ratiosWidth = new float[]{totalGPUTime/totalTimes,totalCPUTime/totalTimes,totalOtherTime/totalTimes};
			    float[] ratios = new float[]{ratiosWidth[0],ratiosWidth[0]+ratiosWidth[1],1.0f};
			    float x;
			    float y;
			    float length = w*0.15f;
			    float angle = 0.0f;
			    float angle_stepsize = Mathf.PI/120.0f;
			    Vector2 center = new Vector2(w*0.7f,h*0.5f);
			    int colorIter = 0;
			    float colorIterRatio = 0.0f;

			    // Labels
			    for(colorIter = 0; colorIter < 3; colorIter++){
			    	float centerAngle = (ratios[colorIter] - ratiosWidth[colorIter] * 0.5f) * (2.0f * Mathf.PI);
			    	// Debug.Log("colorIter:"+colorIter+ " x:"+length * 0.5f * Mathf.Cos(centerAngle));
			    	x = length * 0.3f * Mathf.Cos(centerAngle);
			    	x = x < 0 ? x + x : x;
		    		x = center.x + x;
			        y = center.y + length * 0.5f * Mathf.Sin(centerAngle) + 0.02f*h;
		    		circleGraphLabels[colorIter] = new Vector2(x,Screen.height-y);
			    }

			    colorIter = 0;
			    while (angle < 2.0f * Mathf.PI){
			    	float ratio = angle / (2.0f * Mathf.PI);
			    	if(ratio > ratios[colorIter]){
			    		colorIter++;
			    		colorIterRatio = 0.0f;
			    	}else{
			    		colorIterRatio += angle_stepsize / (2.0f*Mathf.PI);
			    	}
			    	Color diff = (fpsColors[colorIter]-fpsColors[colorIter]*0.4f);
			    	float colorRatio = colorIterRatio/ratiosWidth[colorIter];
			    	GL.Color(fpsColors[colorIter]*0.85f+diff*colorRatio);
			    	
			        x = center.x + length * Mathf.Cos(angle);
			        y = center.y + length * Mathf.Sin(angle);
			        angle += angle_stepsize;

			        GL.Vertex3(center.x,center.y,0f);
			        GL.Vertex3(x,y,0f);
			        x = center.x + length * Mathf.Cos(angle);
			        y = center.y + length * Mathf.Sin(angle);
			    	GL.Vertex3(x,y,0f);
					GL.Vertex3(center.x,center.y,0f);
			    }
			}

		    // Dismiss Box
		    sRect = new Rect(w*0.375f,h*0.08f,w*0.25f,h*0.11f);
		   	GL.Color(fpsColorsTo[1]);
		    GL.Vertex3(sRect.x,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y,0f);
	    	
	    	GL.Color(fpsColors[1]);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y+sRect.height,0f);
	    	GL.Vertex3(sRect.x,sRect.y+sRect.height,0f);

	    	// Top Tabs
	    	float xStart = viewMode == FPSGraphViewMode.assetbreakdown ? w*0.05f : 0.5f*w;
	    	sRect = new Rect(xStart,h*0.84f,w*0.45f,h*0.11f);
		   	GL.Color(fpsColorsTo[1]);
		    GL.Vertex3(sRect.x,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y,0f);
	    	
	    	GL.Color(fpsColors[1]);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y+sRect.height,0f);
	    	GL.Vertex3(sRect.x,sRect.y+sRect.height,0f);

	    	GL.End();
		    GL.PopMatrix();

		    
	    }

	    dt = ((float)stopWatch.Elapsed.TotalSeconds - beforeRender);
		// Debug.Log("OnPostRender time:"+dt);

		if(frameCount>6)
			dtHistory[1, frameIter] = dt;

		eTotalSeconds = (float)stopWatch.Elapsed.TotalSeconds;

		dt = (eTotalSeconds - lastElapsed);
		
		//Debug.Log("Update time:"+dt*1000 + " Time.delta:"+Time.deltaTime*1000);
	}

	IEnumerator OnEndOfFrame() {
        yield return new WaitForEndOfFrame();
        dt = ((float)stopWatch.Elapsed.TotalSeconds - beforeRender);
		// Debug.Log("OnPostRender time:"+dt);

		dtHistory[1, frameIter] = dt;

		eTotalSeconds = (float)stopWatch.Elapsed.TotalSeconds;

		dt = (eTotalSeconds - lastElapsed);
    }

	bool hasFormated;
	Rect wRect;
	GUIStyle backupLabel;
	GUIStyle backupButton;
	GUIStyle h1;
	GUIStyle h2;
	GUIStyle h3;
	GUIStyle guiButton;
	GUIStyle graphTitles;
	Vector2[] circleGraphLabels;

	void format(){
		if(hasFormated==false){
			hasFormated = true;

			h1 = GUI.skin.GetStyle( "Label" );
			backupLabel = new GUIStyle(h1);
			backupButton = new GUIStyle( GUI.skin.GetStyle("Button") );
			h1.alignment = TextAnchor.UpperLeft;
			h1.fontSize = (int)(Screen.height*0.08f);
			h2 = new GUIStyle( h1 );
			h2.fontSize = (int)(Screen.height*0.05f);
			h3 = new GUIStyle( h1 );
			h3.fontSize = (int)(Screen.height*0.037f);
			graphTitles = new GUIStyle( h1 );
			graphTitles.fontSize = (int)(Screen.height*0.037f);
			// graphTitles.alignment = TextAnchor.LowerCenter;
			guiButton = new GUIStyle(h1);
			guiButton.normal.background = null;
		}
	}

	float w;
	float h;

	void OnGUI(){
	    //Debug.Log("OnGUI time:"+stopWatch.Elapsed);
	    w = Screen.width;
	    h = Screen.height;
	    
	    if(viewMode != FPSGraphViewMode.graphing){
	    	Time.timeScale = 0.0f;
	    	
	    	GUI.skin.button = guiButton;
			if(GUI.Button(new Rect(w*0.05f,h*0.05f,w*0.45f,h*0.15f),"")){
				viewMode = FPSGraphViewMode.totalperformance;
			}
			if(GUI.Button(new Rect(w*0.5f,h*0.05f,w*0.45f,h*0.15f),"")){
				viewMode = FPSGraphViewMode.assetbreakdown;
			}
			if(GUI.Button(new Rect(w*0.3f,h*0.8f,w*0.4f,h*0.15f),"")){
				reset();
				viewMode = FPSGraphViewMode.graphing;
				Time.timeScale = 1.0f;
			}

	    	format();
	    	Color backupColor = GUI.color;
	    	GUI.color = Color.black;

	    	Rect sRect = new Rect(w*0.05f,h*0.05f,w*0.9f,h*0.9f);
	    	GUI.color = Color.black;

	    	GUI.color = Color.white;
	    	GUI.skin.label = h2;
			GUI.Label(new Rect(w*0.1f,h*0.07f,w,h*0.2f), "Performance Results");
			GUI.Label(new Rect(w*0.62f,h*0.07f,w,h*0.2f), "Assets Used");

			if(viewMode == FPSGraphViewMode.totalperformance){
				GUI.skin.label = h2;
				GUI.Label(new Rect(w*0.1f,h*0.2f,w,h*0.2f), "Score:");
				GUI.skin.label = h1;
				GUI.Label(new Rect(w*0.1f,h*0.27f,w,h*0.2f), (totalDt*1000.0f).ToString("n0")+"ms");

				GUI.skin.label = h2;
				GUI.Label(new Rect(w*0.1f,h*0.38f,w,h*0.2f), "Time Elapsed:");
				GUI.skin.label = h1;
				GUI.Label(new Rect(w*0.1f,h*0.43f,w,h*0.2f), totalTimeElapsed.ToString("F1")+"s");

				GUI.skin.label = h3;
				float avgFrameRate = totalDt / totalFrames;
				string[] arr = new string[]{"lowest: "+(1.0f/highestDt).ToString("n0")+"fps","highest: "+(1.0f/lowestDt).ToString("n0")+"fps", "avg: "+(1.0f/avgFrameRate).ToString("n0")+"fps"};
				for(int i = 0; i < arr.Length; i++){
					GUI.Label(new Rect(w*0.1f,h*0.57f + w*0.04f*i,w,h*0.2f), arr[i]);
				}

				GUI.color = Color.black;
				GUI.skin.label = graphTitles;
				arr = new string[]{"Render","CPU","Other"};
				float[] arrW = new float[]{0.12f,0.12f,0.12f};
				float sh = 0.0023f*w;
				for(int i = 0; circleGraphLabels!=null && i<circleGraphLabels.Length; i++){
					GUI.color = Color.black;
					GUI.Label(new Rect(circleGraphLabels[i].x+sh,circleGraphLabels[i].y+sh,w*arrW[i],h*0.047f), arr[i]);
					GUI.color = Color.white;
					GUI.Label(new Rect(circleGraphLabels[i].x,circleGraphLabels[i].y,w*arrW[i],h*0.047f), arr[i]);
				}
			}else{
				GUI.skin.label = h2;
				System.Type[] types = new System.Type[]{ typeof(UnityEngine.Object), typeof(Texture2D),typeof(AudioClip),typeof(Mesh),typeof(GameObject),typeof(Component)};
				string[] typeNames = new string[]{"Objects","Textures","Meshes","Materials","GameObjects","Components"};
				for(int i = 0; i < types.Length; i++){
					GUI.Label(new Rect(w*0.1f,h*0.2f + w*0.04f*i,w,h*0.2f), typeNames[i] + ": " + Resources.FindObjectsOfTypeAll( types[i] ).Length.ToString("n0"));
				}
			}

			GUI.skin.button = guiButton;
			
			sRect = new Rect(w*0.435f,h*0.83f,w*0.25f,h*0.11f);
			GUI.skin.label = h2;
			GUI.Label(sRect, "Dismiss");

			GUI.skin.label = backupLabel;
			GUI.skin.button = backupButton;
			GUI.color = backupColor;
		}

	    if(showPerformanceOnClick && didPressOnGraph() && highestDt>0.0f)
	    	showPerformance();
	}

	public void showPerformance(){
		if(viewMode!=FPSGraphViewMode.totalperformance){
			totalTimeElapsed = Time.time;
			viewMode = FPSGraphViewMode.totalperformance;
			if(audioSource)
		    	audioSource.Stop();
		}
	}

	public bool didPressOnGraph(){
		if(Input.touchCount>0||Input.GetMouseButtonDown(0)){
			//Rect graphRect = new Rect(graphPosition.x*(w-graphMultiply*frameHistoryLength),graphPosition.y,graphSizeGUI.width,107*graphMultiply);
			
			Rect graphRect = new Rect(xOff, yGUI-100*graphMultiply, graphSizeGUI.width, 107*graphMultiply);
			// Debug.Log("Input.mousePosition:"+Input.mousePosition+" rect:"+graphRect);
			if(Input.touchCount>0){
				for(int i=0; i < Input.touchCount; i++){
					if(Input.touches[i].phase == TouchPhase.Ended && checkWithinRect( Input.touches[i].position, graphRect ))
						return true;
				}
			}else if(Input.GetMouseButtonDown(0) && checkWithinRect( Input.mousePosition, graphRect )){
				return true;
			}
		}

		return false;
	}

	public static bool checkWithinRect(Vector2 vec2, Rect rect){
		vec2.y = Screen.height-vec2.y;
		//Debug.Log("vec2:"+vec2+" rect:"+rect);
		return (vec2.x > rect.x && vec2.x < rect.x + rect.width && vec2.y > rect.y && vec2.y < rect.y + rect.height);
	}

}


/**
* Create Audio dynamically and easily playback
*
* @class LeanAudio
* @constructor
*/
public class LeanAudio : MonoBehaviour {

	public static float MIN_FREQEUNCY_PERIOD = 0.00001f;
	public static int PROCESSING_ITERATIONS_MAX = 50000;
	public static List<float> generatedWaveDistances;

	public static LeanAudioOptions options(){
		return new LeanAudioOptions();
	}

	/**
	* Create dynamic audio from a set of Animation Curves and other options.
	* 
	* @method createAudio
	* @param {AnimationCurve} volumeCurve:AnimationCurve describing the shape of the audios volume (from 0-1). The length of the audio is dicated by the end value here.
	* @param {AnimationCurve} frequencyCurve:AnimationCurve describing the width of the oscillations between the sound waves in seconds. Large numbers mean a lower note, while higher numbers mean a tighter frequency and therefor a higher note. Values are usually between 0.01 and 0.000001 (or smaller)
	* @param {LeanAudioOptions} options:LeanAudioOptions You can pass any other values in here like vibrato or the frequency you would like the sound to be encoded at. See <a href="LeanAudioOptions.html">LeanAudioOptions</a> for more details.
	* @return {AudioClip} AudioClip of the procedurally generated audio
	* @example
	* AnimationCurve volumeCurve = new AnimationCurve( new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));<br>
	* AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));<br>
	* AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato( new Vector3[]{ new Vector3(0.32f,0f,0f)} ));<br>
	*/
	public static AudioClip createAudio( AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null ){
		if(options==null)
			options = new LeanAudioOptions();
		
		float[] generatedWavePts = createAudioWave( volume, frequency, options);
		return createAudioFromWave( generatedWavePts, options );
	}

	private static float[] createAudioWave( AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options ){
		float time = volume[ volume.length - 1 ].time;
		List<float> list = new List<float>();
		generatedWaveDistances = new List<float>();
		// float[] vibratoValues = new float[ vibrato.Length ];
		float passed = 0f;
		for(int i = 0; i < PROCESSING_ITERATIONS_MAX; i++){
			float f = frequency.Evaluate(passed);
			if(f<MIN_FREQEUNCY_PERIOD)
				f = MIN_FREQEUNCY_PERIOD;
			float height = volume.Evaluate(passed + 0.5f*f);
			if(options.vibrato!=null){
				for(int j=0; j<options.vibrato.Length; j++){
					float peakMulti = Mathf.Abs( Mathf.Sin( 1.5708f + passed * (1f/options.vibrato[j][0]) * Mathf.PI ) );
					float diff = (1f-options.vibrato[j][1]);
					peakMulti = options.vibrato[j][1] + diff*peakMulti;
					height *= peakMulti;
				}	
			}
			// Debug.Log("i:"+i+" f:"+f+" passed:"+passed+" height:"+height+" time:"+time);
			if(passed + 0.5f*f>=time)
				break;

			generatedWaveDistances.Add( f );
			passed += f;

			list.Add( passed );
			list.Add( i%2==0 ? -height : height );
			if(i>=PROCESSING_ITERATIONS_MAX-1){
				Debug.LogError("LeanAudio has reached it's processing cap. To avoid this error increase the number of iterations ex: LeanAudio.PROCESSING_ITERATIONS_MAX = "+(PROCESSING_ITERATIONS_MAX*2));
			}
		}

		float[] wave = new float[ list.Count ];
		for(int i = 0; i < wave.Length; i++){
			wave[i] = list[i];
		}
		return wave;
	}

	private static AudioClip createAudioFromWave( float[] wave, LeanAudioOptions options ){
		float time = wave[ wave.Length - 2 ];
		float[] audioArr = new float[ (int)(options.frequencyRate*time) ];

		int waveIter = 0;
		float subWaveDiff = wave[waveIter];
		float subWaveTimeLast = 0f;
		float subWaveTime = wave[waveIter];
		float waveHeight = wave[waveIter+1];
		for(int i = 0; i < audioArr.Length; i++){
			float passedTime = (float)i / (float)options.frequencyRate;
			if(passedTime > wave[waveIter] ){
				subWaveTimeLast = wave[waveIter];
				waveIter += 2;
				subWaveDiff = wave[waveIter] - wave[waveIter-2];
				waveHeight = wave[waveIter+1];
				// Debug.Log("passed wave i:"+i);
			}
			subWaveTime = passedTime - subWaveTimeLast;
			float ratioElapsed = subWaveTime / subWaveDiff;

			float value = Mathf.Sin( ratioElapsed * Mathf.PI );
			//if(i<25)
			//	Debug.Log("passedTime:"+passedTime+" value:"+value+" ratioElapsed:"+ratioElapsed+" subWaveTime:"+subWaveTime+" subWaveDiff:"+subWaveDiff);
			
			value *= waveHeight;

			audioArr[i] = value;
			// Debug.Log("pt:"+pt+" i:"+i+" val:"+audioArr[i]+" len:"+audioArr.Length);
		}

		int lengthSamples = audioArr.Length;
		#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6
		AudioClip audioClip = AudioClip.Create("Generated Audio", lengthSamples, 1, options.frequencyRate, false);
		#else
		bool is3dSound = false;
		AudioClip audioClip = AudioClip.Create("Generated Audio", lengthSamples, 1, options.frequencyRate, is3dSound, false);
		#endif
		audioClip.SetData(audioArr, 0);

		return audioClip;
	}

	public static AudioClip generateAudioFromCurve( AnimationCurve curve, int frequencyRate = 44100 ){
		float curveTime = curve[ curve.length - 1 ].time;
		float time = curveTime;
		float[] audioArr = new float[ (int)(frequencyRate*time) ];

		// Debug.Log("curveTime:"+curveTime+" AudioSettings.outputSampleRate:"+AudioSettings.outputSampleRate);
		for(int i = 0; i < audioArr.Length; i++){
			float pt = (float)i / (float)frequencyRate;
			audioArr[i] = curve.Evaluate( pt );
			// Debug.Log("pt:"+pt+" i:"+i+" val:"+audioArr[i]+" len:"+audioArr.Length);
		}

		int lengthSamples = audioArr.Length;//(int)( (float)frequencyRate * curveTime );
		#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6
		AudioClip audioClip = AudioClip.Create("Generated Audio", lengthSamples, 1, frequencyRate, false);
		#else
		bool is3dSound = false;
		AudioClip audioClip = AudioClip.Create("Generated Audio", lengthSamples, 1, frequencyRate, is3dSound, false);
		#endif
		audioClip.SetData(audioArr, 0);

		return audioClip;
	}
	
	public static void playAudio( AudioClip audio, Vector3 pos, float volume, float pitch ){
		// Debug.Log("audio length:"+audio.length);
		AudioSource audioSource = playClipAt(audio, pos);
		audioSource.minDistance = 1f;
		audioSource.pitch = pitch;
		audioSource.volume = volume;
	}

	public static AudioSource playClipAt( AudioClip clip, Vector3 pos ) {
		GameObject tempGO = new GameObject(); // create the temp object
		tempGO.transform.position = pos; // set its position
		AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
		aSource.clip = clip; // define the clip
		aSource.Play(); // start the sound
		Destroy(tempGO, clip.length); // destroy object after clip du1783ration
		return aSource; // return the AudioSource reference
	}

	public static void printOutAudioClip( AudioClip audioClip, ref AnimationCurve curve, float scaleX = 1f ){
		// Debug.Log("Audio channels:"+audioClip.channels+" frequency:"+audioClip.frequency+" length:"+audioClip.length+" samples:"+audioClip.samples);
		float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);
        int i = 0;

        Keyframe[] frames = new Keyframe[samples.Length];
        while (i < samples.Length) {
           frames[i] = new Keyframe( (float)i * scaleX, samples[i] );
           ++i;
        }
        curve = new AnimationCurve( frames );
	}
}


/**
* Pass in options to LeanAudio
*
* @class LeanAudioOptions
* @constructor
*/
public class LeanAudioOptions : object {
	public Vector3[] vibrato;
	public int frequencyRate = 44100;

	public LeanAudioOptions(){}

	/**
	* Set the frequency for the audio is encoded. 44100 is CD quality, but you can usually get away with much lower (or use a lower amount to get a more 8-bit sound).
	* 
	* @method setFrequency
	* @param {int} frequencyRate:int of the frequency you wish to encode the AudioClip at
	* @return {LeanAudioOptions} LeanAudioOptions describing optional values
	* @example
	* AnimationCurve volumeCurve = new AnimationCurve( new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));<br>
	* AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));<br>
	* AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato( new Vector3[]{ new Vector3(0.32f,0f,0f)} ).setFrequency(12100) );<br>
	*/
	public LeanAudioOptions setFrequency( int frequencyRate ){
		this.frequencyRate = frequencyRate;
		return this;
	}

	/**
	* Set details about the shape of the curve by adding vibrato waves through it. You can add as many as you want to sculpt out more detail in the sound wave.
	* 
	* @method setVibrato
	* @param {Vector3[]} vibratoArray:Vector3[] The first value is the period in seconds that you wish to have the vibrato wave fluctuate at. The second value is the minimum height you wish the vibrato wave to dip down to (default is zero). The third is reserved for future effects.
	* @return {LeanAudioOptions} LeanAudioOptions describing optional values
	* @example
	* AnimationCurve volumeCurve = new AnimationCurve( new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));<br>
	* AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));<br>
	* AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato( new Vector3[]{ new Vector3(0.32f,0.3f,0f)} ).setFrequency(12100) );<br>
	*/
	public LeanAudioOptions setVibrato( Vector3[] vibrato ){
		this.vibrato = vibrato;
		return this;
	}
}

}