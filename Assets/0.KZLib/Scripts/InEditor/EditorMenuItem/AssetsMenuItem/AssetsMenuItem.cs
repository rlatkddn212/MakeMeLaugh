#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KZLib.KZMenu
{
	public static partial class AssetsMenuItem
	{
		private const int MENU_LINE = 20;
		private const int PIVOT_ORDER = 2000;

		private enum Category
		{
			Total				= PIVOT_ORDER+0,
			Prefab				= PIVOT_ORDER+1+MENU_LINE,
			Script				= PIVOT_ORDER+2+MENU_LINE,
			Texture				= PIVOT_ORDER+3+MENU_LINE,
			ScriptableObject	= PIVOT_ORDER+4+MENU_LINE,
		}

		private static Dictionary<string,List<string>> s_AssetsPathListDict = null;

		private static Dictionary<string,List<string>> AssetsPathListDict
		{
			get
			{
				if(s_AssetsPathListDict == null)
				{
					LoadAssetsPath();
				}

				return s_AssetsPathListDict;
			}
		}

		private static void LoadAssetsPath()
		{
			if(Tools.DisplayCancelableProgressBar("에셋 파인더 로드","에셋 파인더 로드 중",0.1f))
			{
				Tools.ClearProgressBar();

				return;
			}

			var assetList = Tools.LoadAllAssetsPath(null).Distinct().ToList();
			var dependantDict = new Dictionary<string,string[]>();
			var count = assetList.Count;

			for(var i=0;i<count;i++)
			{
				var asset = assetList[i];

				dependantDict[asset] = AssetDatabase.GetDependencies(asset,false);

				if(Tools.DisplayCancelableProgressBar("에셋 파인더 로드",string.Format("에셋 파인더 로드 중 [{0}/{1}]",i,count),i,count))
				{
					Tools.ClearProgressBar();

					return;
				}
			}

			s_AssetsPathListDict = new Dictionary<string,List<string>>();

			foreach(var pair in dependantDict)
			{
				foreach(var dependant in pair.Value)
				{
					if(!s_AssetsPathListDict.ContainsKey(dependant))
					{
						s_AssetsPathListDict.Add(dependant,new List<string>());
					}

					s_AssetsPathListDict[dependant].Add(pair.Key);
				}
			}

			Tools.ClearProgressBar();
		}

		public static void ReLoad()
		{
			LoadAssetsPath();
			s_PrefabList = null;
		}
	}
}	
#endif