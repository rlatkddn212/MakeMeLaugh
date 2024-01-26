// using System.Collections;
// using UnityEngine;

// public static partial class CoroutineTools
// {
// 	public static IEnumerator CoScaleTween(Transform _target,float _from,float _to,float _duration,AnimationCurve _curve,bool _ignoreTimescale = false)
// 	{
// 		_target.localScale = Vector3.one*_from;

// 		yield return CoExecuteOverTime(_from,_to,_duration,(progress)=>
// 		{
// 			_target.localScale = Vector3.one*progress;
// 		},_ignoreTimescale,_curve);
// 	}

// 	public static IEnumerator CoShakePosition(Transform _target,Vector3 _intensity,float _duration,AnimationCurve _curve,bool _ignoreTimescale = false)
// 	{
// 		var position = _target.localPosition;

// 		yield return CoExecuteOverTime(1.0f,0.0f,_duration,(progress)=>
// 		{
// 			var amount = _intensity*progress;

// 			_target.localPosition = position+new Vector3(Tools.GetRndFloat(amount.x),Tools.GetRndFloat(amount.y),Tools.GetRndFloat(amount.z));
// 		},_ignoreTimescale,_curve);


// 		_target.localPosition = position;
// 	}

// 	public static IEnumerator CoShakeRotate(Transform _target,Vector3 _intensity,float _duration,AnimationCurve _curve,bool _ignoreTimescale = false)
// 	{
// 		var rotation = _target.localRotation;

// 		yield return CoExecuteOverTime(1.0f,0.0f,_duration,(progress)=>
// 		{
// 			var amount = _intensity*progress;

// 			_target.localRotation = Quaternion.Euler(rotation.eulerAngles+new Vector3(Tools.GetRndFloat(amount.x),Tools.GetRndFloat(amount.y),Tools.GetRndFloat(amount.z)));
// 		},_ignoreTimescale,_curve);


// 		_target.localRotation = rotation;
// 	}
// }