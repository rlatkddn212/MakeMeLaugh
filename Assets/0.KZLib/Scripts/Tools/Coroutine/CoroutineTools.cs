// using System;
// using System.Collections;
// using UnityEngine;

// public static partial class CoroutineTools
// {
// 	public static IEnumerator CoConvertToCoroutine(Action _onAction)
// 	{
// 		_onAction?.Invoke();

// 		yield break;
// 	}

// 	public static IEnumerator CoMergeCoroutine(params IEnumerator[] _coroutineArray)
// 	{
// 		for(var i=0;i<_coroutineArray.Length;i++)
// 		{
// 			yield return _coroutineArray[i];
// 		}
// 	}

// 	public static void ReleaseCoroutine(MonoBehaviour _behaviour,IEnumerator _coroutine)
// 	{
// 		if(_behaviour != null && _coroutine != null)
// 		{
// 			_behaviour.StopCoroutine(_coroutine);
// 		}
// 	}

// 	public static IEnumerator CoLoopCoroutine(IEnumerator _coroutine,int _count)
// 	{
// 		if(_count == 0)
// 		{
// 			yield break;
// 		}

// 		var count = _count;

// 		while(count == -1 || count-- > 0)
// 		{
// 			yield return _coroutine;
// 		}
// 	}
// }