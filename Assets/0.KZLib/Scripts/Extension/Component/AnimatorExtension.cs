using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class AnimatorExtension
{
	public static bool IsState(this Animator _animator,string _animationName,int _layerIndex = 0)
	{
		return IsState(_animator,Animator.StringToHash(_animationName),_layerIndex);
	}

	public static bool IsState(this Animator _animator,int _animationNameHash,int _layerIndex = 0)
	{
		return _animator.GetCurrentAnimatorStateInfo(_layerIndex).shortNameHash ==_animationNameHash;
	}

	/// <summary>
	/// 현재 애니메이션을 특정 프레임의 시점으로 세팅 ( 속도를 0으로 넣으면 고정 ) 0번프레임으로 고정시킬때 주로 사용.
	/// </summary>
	public static void SetAnimationStopAtFrame(this Animator _animator,string _animationName,float _normalizedTime,int _layerIndex = 0,float _speed = 1.0f)
	{
		SetAnimationStopAtFrame(_animator,Animator.StringToHash(_animationName),_normalizedTime,_layerIndex,_speed);
	}

	/// <summary>
	/// 현재 애니메이션을 특정 프레임의 시점으로 세팅 ( 속도를 0으로 넣으면 고정 ) 0번프레임으로 고정시킬때 주로 사용.
	/// </summary>
	public static void SetAnimationStopAtFrame(this Animator _animator,int _animationNameHash,float _normalizedTime,int _layerIndex = 0,float _speed = 1.0f)
	{
		_animator.speed = _speed;
		_animator.Play(_animationNameHash,_layerIndex,_normalizedTime);
	}

	public static AnimatorStateInfo GetCurrentState(this Animator _animator,int _layerIndex = 0)
	{
		return _animator.GetNextAnimatorStateInfo(_layerIndex).shortNameHash == 0 ? _animator.GetCurrentAnimatorStateInfo(_layerIndex) : _animator.GetNextAnimatorStateInfo(_layerIndex);
	}

	public static IEnumerator PlayAndWait(this Animator _animator,string _animationName,int _layerIndex = 0)
	{
		yield return PlayAndWait(_animator,Animator.StringToHash(_animationName),_layerIndex);
	}

	public static IEnumerator PlayAndWait(this Animator _animator,int _animationNameHash,int _layerIndex = 0)
	{
		_animator.Play(_animationNameHash,_layerIndex);

		yield return WaitForAnimationFinish(_animator,_animationNameHash,_layerIndex);
	}

	public static IEnumerator WaitForAnimationStart(this Animator _animator,string _animationName,int _layerIndex = 0)
	{
		yield return WaitForAnimationStart(_animator,Animator.StringToHash(_animationName),_layerIndex);
	}

	public static IEnumerator WaitForAnimationStart(this Animator _animator,int _animationNameHash,int _layerIndex = 0)
	{
		yield return new WaitWhile(()=> _animator.GetCurrentAnimatorStateInfo(_layerIndex).shortNameHash == _animationNameHash && _animator.GetCurrentAnimatorStateInfo(_layerIndex).normalizedTime > 0.0f);
	}

	public static IEnumerator WaitForAnimationFinish(this Animator _animator,string _animationName,int _layerIndex = 0)
	{
		yield return WaitForAnimationFinish(_animator,Animator.StringToHash(_animationName),_layerIndex);
	}

	public static IEnumerator WaitForAnimationFinish(this Animator _animator,int _animationNameHash,int _layerIndex = 0)
	{
		yield return new WaitWhile(()=> _animator.GetCurrentAnimatorStateInfo(_layerIndex).shortNameHash == _animationNameHash && _animator.GetCurrentAnimatorStateInfo(_layerIndex).normalizedTime < 1.0f);
	}

	public static float GetAnimationClipLength(this Animator _animator,string _animationName,int _layerIndex = 0)
	{
		var clipInfoArray = _animator.GetCurrentAnimatorClipInfo(_layerIndex);

		var index = clipInfoArray.FindIndex(x=>x.clip.name.IsEqual(_animationName));

		if(index == -1)
		{
			return -1.0f;
		}

		return clipInfoArray[index].clip.length;
	}
}