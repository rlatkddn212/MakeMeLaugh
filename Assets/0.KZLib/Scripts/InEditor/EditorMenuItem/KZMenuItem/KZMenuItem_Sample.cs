#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace KZLib.KZMenu
{
	public partial class KZMenuItem
	{
		[MenuItem("KZMenu/Open Main Scene",false,(int) Category.Scene_Core)]
		private static void OnOpenMainScene()
		{
			if(Tools.DisplayCheck("메인 씬 열기","메인 씬을 여시겼습니까?"))
			{
				EditorSceneManager.OpenScene(GetScenePath("MainScene"));
			}
		}

		[MenuItem("KZMenu/Sample/Open Focus Scroller Sample",false,(int) Category.Scene_Core)]
		private static void OnOpenFocusScrollerSample()
		{
			if(Tools.DisplayCheck("포커스 스크롤러 샘플 열기","포커스 스크롤러 샘플을 여시겼습니까?"))
			{
				EditorSceneManager.OpenScene(GetScenePath("FocusScrollerSample"));
			}
		}

		private static string GetScenePath(string _sceneName)
		{
			var guid = AssetDatabase.FindAssets(string.Format("t:Scene {0}",_sceneName)).FirstOrDefault();

			if(!guid.IsEmpty())
			{
				return AssetDatabase.GUIDToAssetPath(guid);
			}

			return string.Empty;
		}
	}
}
#endif
