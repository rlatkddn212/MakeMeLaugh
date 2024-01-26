using System;
using KZLib;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public static partial class GameTools
{
	public static void ClearUnLoadedAssetMemory()
	{
		Resources.UnloadUnusedAssets();

		GC.Collect();
	}

	public static void PauseGame()
	{
		Time.timeScale = 0.0f;
		AudioListener.pause = true;
	}

	public static void UnPauseGame()
	{
		Time.timeScale = 1.0f;
		AudioListener.pause = false;
	}

	public static bool IsPaused()
	{
		return Time.timeScale == 0.0f;
	}

#if UNITY_EDITOR
	public static byte[] GetTestImageData()
	{
		var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(FileTools.PathCombine("Assets",Global.TEMPLATE_FOLDER_PATH,"Ostrich.png"));

		return sprite != null ? sprite.texture.EncodeToPNG() : null;
	}
#endif
}