using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
	public class CoroutineMgr : AutoSingletonMB<CoroutineMgr>
	{
		private int m_Number = 0;
		private readonly Dictionary<string,IEnumerator> m_RoutineDict = new();

		public Coroutine AddCoroutine(IEnumerator _routine,string _key = null)
		{
			var key = _key ?? string.Format("Dummy_{0}",m_Number++);

			if(m_RoutineDict.ContainsKey(key))
			{
				RemoveCoroutineInner(key);
			}

			m_RoutineDict.Add(key,_routine);
			
			return StartCoroutine(AddCoroutineInner(_routine,key));
		}

		public void RemoveCoroutine(string _key)
		{
			if(m_RoutineDict.ContainsKey(_key))
			{
				RemoveCoroutineInner(_key);
			}
		}

		private IEnumerator AddCoroutineInner(IEnumerator _routine,string _key)
		{
			while(_routine.MoveNext())
			{
				yield return _routine.Current;
			}

			RemoveCoroutineInner(_key);
		}

		private void RemoveCoroutineInner(string _key)
		{
			var routine = m_RoutineDict[_key];

			StopCoroutine(routine);

			m_RoutineDict.Remove(_key);

			if(m_RoutineDict.Count == 0)
			{
				ObjectTools.DestroyObject(gameObject);
			}
		}
	}
}