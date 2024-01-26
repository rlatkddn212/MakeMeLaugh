#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib.KZMenu
{
	public static partial class AssetsMenuItem
	{
		[MenuItem("Assets/KZSubMenu/Find Missing Component",false,(int) Category.Total)]
		private static void OnFindMissingComponent()
		{
			var textList = new List<string>();

			foreach(var prefab in PrefabList)
			{
				prefab.transform.TraverseChildren((child)=>
				{
					foreach(var component in child.GetComponents<Component>())
					{
						if(component)
						{
							continue;
						}

						textList.Add(string.Format("프리펩 <b> {0} </b>이며 내부 경로는 <b> {1} </b>입니다.",prefab.name,child.transform.GetHierarchy()));
					}
				});
			}

			if(textList.IsNullOrEmpty())
			{
				Log.Menu.I("컴포넌트가 오류인 프리펩은 없습니다.");
			}
			else
			{
				Log.Menu.I("컴포넌트가 오류인 프리펩 리스트 입니다.");

				foreach(var text in textList)
				{
					Log.Menu.I(text);
				}
			}
		}

		[MenuItem("Assets/KZSubMenu/Find Missing MeshFilter",false,(int) Category.Total)]
		private static void OnFindMissingMeshFilter()
		{
			var textList = new List<string>();

			foreach(var prefab in PrefabList)
			{
				prefab.transform.TraverseChildren((child)=>
				{
					var filter = child.GetComponent<MeshFilter>();

					if(!filter || filter.sharedMesh)
					{
						return;
					}

					textList.Add(string.Format("프리펩 <b> {0} </b>이며 내부 경로는 <b> {1} </b>입니다.",prefab.name,child.transform.GetHierarchy()));
				});
			}

			if(textList.IsNullOrEmpty())
			{
				Log.Menu.I("메쉬 필터가 오류인 프리펩은 없습니다.");
			}
			else
			{
				Log.Menu.I("메쉬 필터가 오류인 프리펩 리스트 입니다.");

				foreach(var text in textList)
				{
					Log.Menu.I(text);
				}
			}
		}

		private static List<GameObject> s_PrefabList = null;

		private static List<GameObject> PrefabList
		{
			get
			{
				return s_PrefabList ??= LoadPrefab();
			}
		}

		private static List<GameObject> LoadPrefab()
		{
			if(Tools.DisplayCancelableProgressBar("에셋 파인더 로드","에셋 파인더 로드 중",0.1f))
			{
				Tools.ClearProgressBar();

				return new List<GameObject>();
			}

			var dataList = Tools.LoadAllAssetsInEditor<GameObject>("t:prefab");

			Tools.ClearProgressBar();

			return dataList;
		}
	}
}	
#endif