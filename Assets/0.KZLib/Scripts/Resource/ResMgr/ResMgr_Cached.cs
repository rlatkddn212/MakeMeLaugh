using System;
using System.Collections.Generic;
using System.Threading;
using Object = UnityEngine.Object;

namespace KZLib
{
	public partial class ResMgr : Singleton<ResMgr>
	{
		private record CachedData
		{
			private const long DEFAULT_DELETE_TIME = 60*1000L;	// 60초

			public Object Resource { get; }
			private readonly Timer m_Timer = null;

			public CachedData(Object _resource,Action _onAction)
			{
				m_Timer = new Timer((dummy)=>
				{
					_onAction?.Invoke();
				},null,DEFAULT_DELETE_TIME,Timeout.Infinite);

				Resource = _resource;
			}

			public void Release()
			{
				m_Timer.Dispose();
			}
		}

		private readonly Dictionary<string,CachedData> m_CachedDataDict = new();

		private TObject GetData<TObject>(string _path) where TObject : Object
		{
			if(m_CachedDataDict.TryGetValue(_path,out var data))
			{
				return (TObject) data.Resource;
			}

			return null;
		}

		private void PutData<TObject>(string _path,TObject _object) where TObject : Object
		{
			if(m_CachedDataDict.ContainsKey(_path))
			{
				return;
			}

			m_CachedDataDict.Add(_path,new CachedData(_object,()=>
			{
				m_CachedDataDict[_path].Release();
				m_CachedDataDict.RemoveSafe(_path);
			}));
		}
	}
}