using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static partial class TweenTools
{
	/// <summary>
	/// 그래픽 페이드인->페이드 아웃 하는 함수
	/// </summary>
	public static Sequence PlayGraphicFader(Graphic _graphic,Vector3 _fadeDuration,int _loopCount = 1)
	{
		if(_loopCount == 0)
		{
			return null;
		}

		var sequence = DOTween.Sequence();

		if(_fadeDuration.x > 0.0f)
		{
			_graphic.color = _graphic.color.MaskAlpha(0.0f);

			var fadeInTween = _graphic.DOFade(1.0f,_fadeDuration.x);
			fadeInTween.id = string.Format("FadeIn_{0}",DateTime.Now.ToString("HH:mm:ss"));

			sequence.Append(fadeInTween);
		}
		else
		{
			_graphic.color = _graphic.color.MaskAlpha(1.0f);
		}

		if(_fadeDuration.z > 0.0f)
		{
			var fadeOutTween = _graphic.DOFade(0.0f,_fadeDuration.z);

			if(_fadeDuration.y > 0.0f)
			{
				fadeOutTween = fadeOutTween.SetDelay(_fadeDuration.y);
			}

			fadeOutTween.id = string.Format("FadeOut_{0}",DateTime.Now.ToString("HH:mm:ss"));

			sequence.Append(fadeOutTween);
		}

		if(_loopCount != 1)
		{
			sequence.SetLoops(_loopCount);
		}

		sequence.id = string.Format("GraphicFade_{0}",DateTime.Now.ToString("HH:mm:ss"));

		return sequence;
	}

	/// <summary>
	/// 스프라이트 렌더러 페이드인->페이드 아웃 하는 함수
	/// </summary>
	public static Sequence PlaySpriteRendererFader(SpriteRenderer _renderer,Vector3 _fadeDuration,int _loopCount = 1)
	{
		if(_loopCount == 0)
		{
			return null;
		}

		var sequence = DOTween.Sequence();

		if(_fadeDuration.x > 0.0f)
		{
			_renderer.color = _renderer.color.MaskAlpha(0.0f);

			var fadeInTween = _renderer.DOFade(1.0f,_fadeDuration.x);
			fadeInTween.id = string.Format("FadeIn_{0}",DateTime.Now.ToString("HH:mm:ss"));

			sequence.Append(fadeInTween);
		}
		else
		{
			_renderer.color = _renderer.color.MaskAlpha(1.0f);
		}

		if(_fadeDuration.z > 0.0f)
		{
			var fadeOutTween = _renderer.DOFade(0.0f,_fadeDuration.z);

			if(_fadeDuration.y > 0.0f)
			{
				fadeOutTween = fadeOutTween.SetDelay(_fadeDuration.y);
			}

			fadeOutTween.id = string.Format("FadeOut_{0}",DateTime.Now.ToString("HH:mm:ss"));

			sequence.Append(fadeOutTween);
		}

		sequence.SetLoops(_loopCount);
		sequence.id = string.Format("SpriteRenderer_{0}",DateTime.Now.ToString("HH:mm:ss"));

		return sequence;
	}

	// /// <summary>
	// /// 타이핑 애니메이션
	// /// </summary>
	// public static Tween TypingText(TMP_Text _textMesh,string _sentence,float _duration)
	// {
	// 	var tween = _textMesh.DOText(_sentence,_duration);
	// 	tween.id = string.Format("typing_{0}",DateTime.Now.ToString("HH:mm:ss"));

	// 	return tween;
	// }

	/// <summary>
	/// 매 초 숫자 카운팅
	/// </summary>
	public static Tween CountNumber(TMP_Text _textMesh,int _start,int _finish,string _format = null)
	{
		var format = _format.IsEmpty() ? "{0}" : _format;
		var count = Mathf.Abs(_finish-_start);

		_textMesh.SetSafeTextMeshPro(string.Format(format,_start--));

		var tween = PlayTimer(1.0f,count,()=>
		{
			_textMesh.SetSafeTextMeshPro(string.Format(format,_start--));
		});

		return tween;
	}

	// /// <summary>
	// /// 매 프레임 숫자 카운팅
	// /// </summary>
	// public static Sequence CountNumber(TMP_Text _textMesh,float _start,float _finish,float _duration,string _format = null)
	// {
	// 	var tag = string.Format("COUNT_{0}",DateTime.Now.ToString("HH:mm:ss"));
	// 	var format = _format.IsEmpty() ? "{0}" : _format;

	// 	_textMesh.SetSafeTextMeshPro(string.Format(format,_start));

	// 	var sequence = SetSequence(tag,_start,_finish,_duration,(time)=>
	// 	{
	// 		_textMesh.SetSafeTextMeshPro(string.Format(format,time));
	// 	},()=>
	// 	{
	// 		_textMesh.SetSafeTextMeshPro(string.Format(format,_finish));
	// 	});

	// 	return sequence;
	// }
}