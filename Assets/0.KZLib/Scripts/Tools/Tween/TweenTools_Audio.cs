using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static partial class TweenTools
{
	// public static Sequence FadeAudio(AudioSource _source,float _start,float _finish,float _duration,Action _onComplete)
	// {
	// 	_source.volume = _start;

	// 	var sequence = DOTween.Sequence();
	// 	sequence.Append(_source.DOFade(_finish,_duration));

	// 	if(_onComplete != null)
	// 	{
	// 		sequence.OnComplete(()=> { _onComplete(); });
	// 	}

	// 	sequence.id = string.Format("AudioFade_{0}",DateTime.Now.ToString("HH:mm:ss"));

	// 	return sequence;
	// }

	public static Sequence PlayAudioFader(AudioSource _source,Vector3 _fadeDuration,float _volume,int _loopCount = 1)
	{
		if(_loopCount == 0)
		{
			return null;
		}

		var sequence = DOTween.Sequence();

		if(_fadeDuration.x > 0.0f)
		{
			_source.volume = 0.0f;

			var fadeInTween = _source.DOFade(_volume,_fadeDuration.x);
			fadeInTween.id = string.Format("FadeIn_{0}",DateTime.Now.ToString("HH:mm:ss"));

			sequence.Append(fadeInTween);
		}
		else
		{
			_source.volume = _volume;
		}

		if(_fadeDuration.z > 0.0f)
		{
			var fadeOutTween = _source.DOFade(0.0f,_fadeDuration.z);

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

		sequence.id = string.Format("AudioFade_{0}",DateTime.Now.ToString("HH:mm:ss"));

		return sequence;
	}
}