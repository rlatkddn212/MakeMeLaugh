using UnityEngine;
using UnityEngine.Video;
using System;
using Object = UnityEngine.Object;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace KZLib
{
	public partial class ResMgr : Singleton<ResMgr>
	{
		public TComponent GetObject<TComponent>(string _path,Transform _parent = null,bool _Immediately = true) where TComponent : Component
		{
			var data = GetObject(_path,_parent,_Immediately);

			return data ? data.GetComponent<TComponent>() : null;
		}

		public GameObject GetObject(string _path,Transform _parent = null,bool _Immediately = true)
		{
			if(_Immediately)
			{
				var data = GetResource<GameObject>(_path);

				if(data)
				{
					data.transform.SetParent(_parent);

					return data;
				}
			}
			else
			{
				AddLoadingQueue(_path,0,_parent);
			}

			return null;
		}

		public AnimatorOverrideController GetAnimatorController(string _path)
		{
			return GetResource<AnimatorOverrideController>(_path);
		}

		public AnimationClip GetAnimationClip(string _path)
		{
			return GetResource<AnimationClip>(_path);
		}

		public ScriptableObject GetScriptableObject(string _path)
		{
			return GetResource<ScriptableObject>(_path);
		}

		public TObject GetScriptableObject<TObject>(string _path) where TObject : ScriptableObject
		{
			return GetResource<TObject>(_path);
		}

		public AudioClip GetAudioClip(string _path)
		{
			return GetResource<AudioClip>(_path);
		}

		public VideoClip GetVideoClip(string _path)
		{
			return GetResource<VideoClip>(_path);
		}

		public Sprite GetSprite(string _path)
		{
			return GetResource<Sprite>(_path);
		}

		public TextAsset GetTextAsset(string _path)
		{
			return GetResource<TextAsset>(_path);
		}

		public Material GetMaterial(string _path)
		{
			return GetResource<Material>(_path);
		}

		private TObject GetResource<TObject>(string _path) where TObject : Object
		{
			if(_path.IsEmpty())
			{
				throw new NullReferenceException("경로가 없습니다.");
			}

			var resource = GetData<TObject>(_path);

			if(!resource)
			{
				resource = LoadSingle<TObject>(_path);

				if(!resource)
				{
					throw new NullReferenceException(string.Format("리소스가 없습니다.[파일 경로 : {0}]",_path));
				}

				PutData(_path,resource);
			}

			if(resource is GameObject)
			{
				var data = ObjectTools.CopyObject(resource);
#if SERVER_PLAY
				Tools.ReAssignShader(data as GameObject);
#endif
				return data;
			}
			else
			{
				return resource;
			}
		}

		private TObject LoadSingle<TObject>(string _filePath) where TObject : Object
		{
#if UNITY_EDITOR
			if(!FileTools.IsFilePath(_filePath))
			{
				throw new NullReferenceException(string.Format("경로가 폴더 경로 입니다.[경로 : {0}]",_filePath));
			}
#endif
			//? Resources 안의 파일 이므로 바로 로드 한다.
			if(_filePath.StartsWith(RESOURCES))
			{
				var filePath = FileTools.RemoveHeaderDirectory(_filePath,RESOURCES);

				return Resources.Load<TObject>(filePath[..filePath.LastIndexOf('.')]);
			}

			var assetPath = FileTools.GetAssetsPath(_filePath);

#if SERVER_PLAY
			return AddressablesMgr.In.GetSingle<TObject>(assetPath);
#endif

#if UNITY_EDITOR
			return AssetDatabase.LoadAssetAtPath<TObject>(assetPath);
#else
			return null;
#endif
		}
	}
}