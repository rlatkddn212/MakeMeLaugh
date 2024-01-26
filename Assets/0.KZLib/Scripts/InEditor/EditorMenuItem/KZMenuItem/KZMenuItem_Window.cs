#if UNITY_EDITOR
using KZLib.KZWindow;
using UnityEditor;

namespace KZLib.KZMenu
{
	public partial class KZMenuItem
	{
		[MenuItem("KZMenu/Open Local Data Window",false,(int) Category.Window_LocalData)]
		private static void OnOpenLocalDataWindow()
		{
			EditorWindow.GetWindow<LocalDataWindow>().Show();
		}

		// [MenuItem("KZMenu/Open Manual Window",false,(int) Category.Window_Manual)]
		// private static void OnOpenManualWindow()
		// {
		// 	EditorWindow.GetWindow<ManualWindow>().Show();
		// }

		[MenuItem("KZMenu/Utility/Open Run In Edit Window",false,(int) Category.Window_Utility)]
		private static void OnOpenRunInEditWindow()
		{
			EditorWindow.GetWindow<RunInEditWindow>().Show();
		}

		[MenuItem("KZMenu/Utility/Open Preset Edit Window",false,(int) Category.Window_Utility)]
		private static void OnOpenPresetEditWindow()
		{
			EditorWindow.GetWindow<PresetEditWindow>().Show();
		}

		[MenuItem("KZMenu/Utility/Open Sprite Edit Window",false,(int) Category.Window_Utility)]
		private static void OnOpenSpriteEditWindow()
		{
			EditorWindow.GetWindow<SpriteEditWindow>().Show();
		}
	}
}
#endif