using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static partial class UniTaskTools
{
	/// <summary>
	/// 지정된 시간 또는 조건이 충족될 때까지 대기하는 코루틴.
	/// </summary>
	public static async UniTask WaitForSecondOrActionAsync(float _duration,Func<bool> _onCondition,bool _ignoreTimescale = false,AnimationCurve _curve = null,CancellationToken _token = default)
	{
		await UniTask.WhenAny(ExecuteDurationAsync(_duration,_curve,_ignoreTimescale,null,_token),UniTask.WaitUntil(_onCondition,cancellationToken : _token));
	}

	public static async UniTask ExecuteLoopAsync(Action<float> _onProgress,bool _ignoreTimescale = false,AnimationCurve _curve = null,CancellationToken _token = default)
	{
		await ExecuteDurationAsync(-1.0f,_curve,_ignoreTimescale,(timer)=>
		{
			_onProgress?.Invoke(timer);
		},_token);
	}

	public static async UniTask ExecuteOverTimeAsync(float _start,float _finish,float _duration,Action<float> _onProgress,bool _ignoreTimescale = false,AnimationCurve _curve = null,CancellationToken _token = default)
	{
		await ExecuteDurationAsync(_duration,_curve,_ignoreTimescale,(timer)=>
		{
			_onProgress?.Invoke((_finish-_start)*timer+_start);
		},_token);
	}

	private static async UniTask ExecuteDurationAsync(float _duration,AnimationCurve _curve,bool _ignoreTimescale,Action<float> _onProgress,CancellationToken _token)
	{
		if(_duration == 0.0f)
		{
			return;
		}

		var curve = _curve ?? Tools.GetEaseCurve(EaseType.Linear);
		var startTime = _ignoreTimescale ? Time.realtimeSinceStartup : Time.time;
		var elapsedTime = 0.0f;

		while(_duration < 0.0f || elapsedTime <= _duration)
		{
			if(_token.IsCancellationRequested)
			{
				return;
			}

			_onProgress?.Invoke(curve.Evaluate(elapsedTime/_duration));

			await UniTask.Yield(_token);

			var current = (_ignoreTimescale ? Time.realtimeSinceStartup : Time.time)-startTime;

			if(elapsedTime != current)
			{
				elapsedTime = current;
			}
		}
	}
}