
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutosaveOnRun: ScriptableObject
{
	static AutosaveOnRun()
	{
		EditorApplication.playmodeStateChanged = () =>
		{
			if(EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			{
				Debug.Log("Auto-Saving scene before entering Play mode: " + SceneManager.GetActiveScene().ToString());

                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                //EditorSceneManager.Sav
			}
		};
	}
}
#endif