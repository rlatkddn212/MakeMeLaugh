using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KZLib.KZDevelop;
using UnityEngine;

public class EnemyMgr : SingletonMB<EnemyMgr>
{
	[SerializeField]
	private List<Transform> m_PositionList = new();

	[SerializeField]
	private Enemy m_Pivot = null;

	[SerializeField]
	private Transform m_Storage = null;

	private GameObjectPool m_ObjectPool = null;
	
	[SerializeField]
	private Enemy m_NowEnemy = null;

	private int? m_index = null;

	protected override void Initialize()
	{
		base.Initialize();

        m_ObjectPool = new GameObjectPool(m_Pivot.gameObject,m_Storage,3);

        m_PositionList.Randomize();
	}

	public async UniTask KillEnemyAsync()
	{
		if(m_NowEnemy != null)
		{
			await m_NowEnemy.DestroyEffectAsync();

			m_ObjectPool.Put(m_NowEnemy.gameObject);

        }
	}

	public void SpawnEnemy()
	{
		var dataList = new List<Transform>(m_PositionList);
        
        if (m_index.HasValue)
		{
			dataList.RemoveAt(m_index.Value);
		}

		var data = dataList.GetRndValue();

		m_NowEnemy = m_ObjectPool.Get<Enemy>(transform);

        m_NowEnemy.Initialize(data);

		m_index = dataList.IndexOf(data);
	}
}
