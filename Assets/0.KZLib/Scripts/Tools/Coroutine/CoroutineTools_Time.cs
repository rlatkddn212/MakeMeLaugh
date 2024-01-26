// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public static partial class CoroutineTools
// {
// 	private class FloatComparer : IEqualityComparer<float>
// 	{
// 		bool IEqualityComparer<float>.Equals(float _x,float _y) => _x == _y;

// 		int IEqualityComparer<float>.GetHashCode(float _object) => _object.GetHashCode();
// 	}

// 	private static readonly Dictionary<float,WaitForSeconds> s_TimeIntervalDict = new(new FloatComparer());
// 	private static readonly Dictionary<float,WaitForSecondsRealtime> s_RealTimeIntervalDict = new(new FloatComparer());

// 	/// <summary>
// 	/// 지정된 시간 또는 조건이 충족될 때까지 대기하는 코루틴.
// 	/// </summary>
// 	public static IEnumerator CoWaitForSecondOrAction(float _duration,Func<bool> _onCondition,bool _ignoreTimescale = false,AnimationCurve _curve = null)
// 	{
// 		yield return CoExecuteDuration(_duration,_curve,_ignoreTimescale,null,_onCondition);
// 	}

// 	public static IEnumerator CoExecuteOverTime(float _start,float _finish,float _duration,Action<float> _onProgress,bool _ignoreTimescale = false,AnimationCurve _curve = null)
// 	{
// 		yield return CoExecuteDuration(_duration,_curve,_ignoreTimescale,(timer)=>
// 		{
// 			_onProgress?.Invoke((_finish-_start)*timer+_start);
// 		},null);
// 	}

// 	public static IEnumerator CoWaitForSeconds(float _seconds,bool _ignoreTimescale)
// 	{
// 		if(_ignoreTimescale)
// 		{
// 			if(!s_RealTimeIntervalDict.ContainsKey(_seconds))
// 			{
// 				s_RealTimeIntervalDict.Add(_seconds,new WaitForSecondsRealtime(_seconds));
// 			}

// 			yield return s_RealTimeIntervalDict[_seconds];
// 		}
// 		else
// 		{
// 			if(!s_TimeIntervalDict.ContainsKey(_seconds))
// 			{
// 				s_TimeIntervalDict.Add(_seconds,new WaitForSeconds(_seconds));
// 			}

// 			yield return s_TimeIntervalDict[_seconds];
// 		}
// 	}

// 	public static IEnumerator CoWaitForFrames(int _count,Action _onComplete)
// 	{
// 		if(_count <= 0)
// 		{
// 			_onComplete?.Invoke();

// 			yield break;
// 		}

// 		for(var i=0;i<_count;i++)
// 		{
// 			yield return null;
// 		}

// 		_onComplete?.Invoke();
// 	}

// 	private static IEnumerator CoExecuteDuration(float _duration,AnimationCurve _curve,bool _ignoreTimescale,Action<float> _onProgress,Func<bool> _onCondition)
// 	{
// 		if(_duration <= 0.0f)
// 		{
// 			yield break;
// 		}

// 		var curve = _curve ?? Tools.GetEaseCurve(EaseType.Linear);
// 		var elapsedTime = 0.0f;

// 		while(elapsedTime < _duration)
// 		{
// 			var deltaTime = _ignoreTimescale ? Time.deltaTime : Time.unscaledDeltaTime;

// 			if(deltaTime > 0.0f)
// 			{
// 				elapsedTime += deltaTime;

// 				_onProgress?.Invoke(curve.Evaluate(elapsedTime/_duration));
// 			}

// 			if(_onCondition?.Invoke() ?? false)
// 			{
// 				yield break;
// 			}

// 			yield return null;
// 		}
// 	}

// 	public static IEnumerator CoRepeatForever(Action _onRepeat,float _delay,bool _ignoreTimescale)
// 	{
// 		yield return CoRepeatUntilInner(() => true,_onRepeat,null,_delay,_ignoreTimescale);
// 	}

// 	public static IEnumerator CoRepeatUntil(Func<bool> _onCondition,Action _onRepeat,Action _onComplete,float _delay,bool _ignoreTimescale)
// 	{
// 		yield return CoRepeatUntilInner(_onCondition,_onRepeat,_onComplete,_delay,_ignoreTimescale);
// 	}

// 	private static IEnumerator CoRepeatUntilInner(Func<bool> _onCondition,Action _onRepeat,Action _onComplete,float _delay,bool _ignoreTimescale)
// 	{
// 		while(_onCondition())
// 		{
// 			_onRepeat?.Invoke();

// 			if(_delay > 0.0f)
// 			{
// 				yield return CoWaitForSeconds(_delay,_ignoreTimescale);
// 			}
// 		}

// 		_onComplete?.Invoke();
// 	}
// }