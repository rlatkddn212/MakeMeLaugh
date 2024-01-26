using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static partial class UniTaskTools
{
	public static async UniTask ScaleTweenAsync(Transform _target,float _from,float _to,float _duration,AnimationCurve _curve,bool _ignoreTimescale = false,CancellationToken _token = default)
	{
		_target.localScale = Vector3.one*_from;

		await ExecuteOverTimeAsync(_from,_to,_duration,(progress)=>
		{
			_target.localScale = Vector3.one*progress;
		},_ignoreTimescale,_curve,_token);
	}

	public static async UniTask ShakePositionAsync(Transform _target,Vector3 _intensity,float _duration,AnimationCurve _curve,bool _ignoreTimescale = false,CancellationToken _token = default)
	{
		var position = _target.localPosition;

		await ExecuteOverTimeAsync(1.0f,0.0f,_duration,(progress)=>
		{
			var amount = _intensity*progress;

			_target.localPosition = position+new Vector3(Tools.GetRndFloat(amount.x),Tools.GetRndFloat(amount.y),Tools.GetRndFloat(amount.z));
		},_ignoreTimescale,_curve,_token);

		_target.localPosition = position;
	}

	public static async UniTask ShakeRotateAsync(Transform _target,Vector3 _intensity,float _duration,AnimationCurve _curve,bool _ignoreTimescale = false,CancellationToken _token = default)
	{
		var rotation = _target.localRotation;

		await ExecuteOverTimeAsync(1.0f,0.0f,_duration,(progress)=>
		{
			var amount = _intensity*progress;

			_target.localRotation = Quaternion.Euler(rotation.eulerAngles+new Vector3(Tools.GetRndFloat(amount.x),Tools.GetRndFloat(amount.y),Tools.GetRndFloat(amount.z)));
		},_ignoreTimescale,_curve,_token);

		_target.localRotation = rotation;
	}
}