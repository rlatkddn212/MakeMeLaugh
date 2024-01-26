using UnityEngine;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;

namespace KZLib
{
	public partial class ResMgr : Singleton<ResMgr>
	{
		private SortedSet<LoadingData> m_LoadingSet = new();
		
		private void AddLoadingQueue(string _path,int _order = 0,Transform _parent = null)
		{
			m_LoadingSet.Add(new LoadingData(_path,_order,_parent));
		}

		private async UniTaskVoid LoadingAsync()
		{
			while(true)
			{
				await UniTask.WaitUntil(() => m_LoadingSet.Count > 0);

				await UniTask.Delay(TimeSpan.FromSeconds(UPDATE_PERIOD),true);

				var data = m_LoadingSet.Min;

				while(data == null)
				{
					m_LoadingSet.Remove(data);

					data = m_LoadingSet.Min;
				}

				GetObject(data.DataPath,data.Parent,true);

				m_LoadingSet.Remove(data);
			}
		}

		private record LoadingData : IComparable<LoadingData>
		{
			private readonly int m_Order = 0;
			public string DataPath { get; }
			public Transform Parent { get; }

			public LoadingData(string _path,int _order,Transform _parent)
			{
				m_Order = _order;
				DataPath = _path;
				Parent = _parent;
			}

			public int CompareTo(LoadingData _data)
			{
				return m_Order.CompareTo(_data.m_Order);
			}
		}
	}
}