using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

namespace KZLib
{
	public partial class ResMgr : Singleton<ResMgr>
	{
		public TComponent[] GetObjects<TComponent>(string _folderPath,Transform _parent = null,bool _Immediately = true) where TComponent : Component
		{
			var dataArray = GetObjects(_folderPath,_parent,_Immediately);

			return dataArray != null ? Array.ConvertAll(dataArray,x=>x.GetComponent<TComponent>()) : null;
		}

		public GameObject[] GetObjects(string _folderPath,Transform _parent = null,bool _Immediately = true)
		{
			if(_Immediately)
			{
				var dataArray = GetResources<GameObject>(_folderPath);

				if(dataArray.IsNullOrEmpty())
				{
					return null;
				}

				foreach (var data in dataArray)
				{
					data.transform.SetParent(_parent);
				}

				return dataArray;
			}

			return null;
		}

		public AnimatorOverrideController[] GetAnimatorControllers(string _folderPath)
		{
			return GetResources<AnimatorOverrideController>(_folderPath);
		}

		public AnimationClip[] GetAnimationClips(string _folderPath)
		{
			return GetResources<AnimationClip>(_folderPath);
		}

		public ScriptableObject[] GetScriptableObjects(string _folderPath)
		{
			return GetResources<ScriptableObject>(_folderPath);
		}

		public AudioClip[] GetAudioClips(string _folderPath)
		{
			return GetResources<AudioClip>(_folderPath);
		}

		public VideoClip[] GetVideoClips(string _folderPath)
		{
			return GetResources<VideoClip>(_folderPath);
		}

		public Sprite[] GetSprites(string _folderPath)
		{
			return GetResources<Sprite>(_folderPath);
		}

		public TextAsset[] GetTextAssets(string _folderPath)
		{
			return GetResources<TextAsset>(_folderPath);
		}

		public Material[] GetMaterials(string _path)
		{
			return GetResources<Material>(_path);
		}

		private TObject[] GetResources<TObject>(string _folderPath) where TObject : Object
		{
			if(_folderPath.IsEmpty())
			{
				throw new NullReferenceException("폴더가 없습니다.");
			}

			var dataArray = LoadMulti<TObject>(_folderPath);

			if(dataArray.IsNullOrEmpty())
			{
				throw new NullReferenceException(string.Format("리소스가 없습니다.[폴더 경로 : {0}]",_folderPath));
			}

			if(typeof(TObject) == typeof(GameObject))
			{
				var resourceList = new List<TObject>();

				foreach(var data in dataArray)
				{
					var resource = ObjectTools.CopyObject<TObject>(data);
#if SERVER_PLAY
					Tools.ReAssignShader(resource as GameObject);
#endif
					resourceList.Add(resource);
				}

				return resourceList.ToArray();
			}

			return dataArray;
		}

		private TObject[] LoadMulti<TObject>(string _folderPath) where TObject : Object
		{
#if UNITY_EDITOR
			if(FileTools.IsFilePath(_folderPath))
			{
				throw new NullReferenceException(string.Format("경로가 파일 경로 입니다.[경로 : {0}]",_folderPath));
			}
#endif

			//? Resources로 시작하는건 리소스 폴더이므로
			if(_folderPath.StartsWith(RESOURCES))
			{
				return Resources.LoadAll<TObject>(FileTools.RemoveHeaderDirectory(_folderPath,RESOURCES));
			}

#if SERVER_PLAY
			return AddressablesMgr.In.GetMulti<TObject>(_folderPath);
#endif

#if UNITY_EDITOR
			return Tools.LoadAllAssetsInFolder<TObject>(_folderPath);
#else
			return null;
#endif
		}
	}
}