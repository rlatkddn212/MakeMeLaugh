using System.Collections.Generic;
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

	private int? m_index = null;

	protected override void Initialize()
	{
		base.Initialize();

		m_ObjectPool = new GameObjectPool(m_Pivot.gameObject,m_Storage,5);

		m_PositionList.Randomize();
	}

	public void SpawnEnemy()
	{
		var dataList = new List<Transform>(m_PositionList);

		if(m_index.HasValue)
		{
			dataList.RemoveAt(m_index.Value);
		}

		var data = dataList.GetRndValue();
		var enemy = m_ObjectPool.Get<Enemy>(transform);

		enemy.Initialize(data.position);

		m_index = dataList.IndexOf(data);
	}
}
