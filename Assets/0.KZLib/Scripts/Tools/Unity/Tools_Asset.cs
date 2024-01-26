#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static partial class Tools
{
	public static List<string> LoadAllAssetsPath(string _filter,string[] _searchInFolders = null)
	{
		var pathList = new List<string>();

		foreach(var guid in AssetDatabase.FindAssets(_filter,_searchInFolders))
		{
			pathList.Add(AssetDatabase.GUIDToAssetPath(guid));
		}

		return pathList;
	}

	public static List<TObject> LoadAllAssetsInEditor<TObject>(string _filter,string[] _searchInFolders = null) where TObject : Object
	{
		var assetList = new List<TObject>();

		foreach(var guid in AssetDatabase.FindAssets(_filter,_searchInFolders))
		{
			var asset = AssetDatabase.LoadAssetAtPath<TObject>(AssetDatabase.GUIDToAssetPath(guid));

			if(asset)
			{
				assetList.Add(asset);
			}
		}

		return assetList;
	}

	public static List<GameObject> GetAllPrefabsByList()
	{	
		return LoadAllAssetsInEditor<GameObject>("t:prefab");
	}

	public static TObject[] LoadAllAssetsInFolder<TObject>(string _folderPath) where TObject : Object
	{
		var assetList = new List<TObject>();

		foreach(var path in FileTools.GetAllFilePathInFolder(FileTools.GetFullPath(_folderPath)))
		{
			var asset = AssetDatabase.LoadAssetAtPath<TObject>(FileTools.GetAssetsPath(path));

			if(asset)
			{
				assetList.Add(asset);
			}
		}

		return assetList.IsNullOrEmpty() ? null : assetList.ToArray();
	}

	public static TObject LoadAsset<TObject>(string _filter,string[] _searchInFolders = null) where TObject : Object
	{
		var guid = AssetDatabase.FindAssets(_filter,_searchInFolders).FirstOrDefault();

		return guid != null ? AssetDatabase.LoadAssetAtPath<TObject>(AssetDatabase.GUIDToAssetPath(guid)) : null;
	}

	public static void SaveAssetData(string _dataPath,Object _asset)
	{
		var folderPath = FileTools.GetParentPath(_dataPath);
		var assetPath = FileTools.GetAssetsPath(_dataPath);

		FileTools.CreateFolder(folderPath);

		if(FileTools.IsExistFile(_dataPath))
		{
			AssetDatabase.DeleteAsset(assetPath);
		}

		AssetDatabase.CreateAsset(_asset,assetPath);
		
		EditorUtility.SetDirty(_asset);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		Log.Data.I("{0}가 {1}에 저장 되었습니다.",_asset.name,_dataPath);
	}
}
#endif