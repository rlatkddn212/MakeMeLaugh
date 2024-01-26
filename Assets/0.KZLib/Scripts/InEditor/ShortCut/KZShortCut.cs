#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace KZLib.KZEditorTool
{
	public partial class KZShortCut
	{
		[Shortcut("Look Center",KeyCode.F,ShortcutModifiers.Alt)]
		private static void OnOpenEffectTestScene()
		{
			SceneView.lastActiveSceneView?.LookAt(Vector3.zero);
		}
	}
}
#endif