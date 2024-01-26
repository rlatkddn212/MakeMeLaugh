using System;
using DG.Tweening;
using UnityEngine;

public class TrailParticleEffectClip : ParticleEffectClip
{
	public new record ParamData : ParticleEffectClip.ParamData
	{
		public float Duration { get; }
		public Vector3 EndPosition { get; }
		
		public ParamData(Vector3 _endPosition,float _duration,Color? _color = null,Action _onComplete = null) : base(_color,_onComplete)
		{
			Duration = _duration;
			EndPosition = _endPosition;
		}
	}

	protected override void DoInitialize(EffectClip.ParamData _param)
	{
		base.DoInitialize(_param);

		var data = _param as ParamData;

		m_Duration = data.Duration;
	}

	protected override void JoinTween(EffectClip.ParamData _param)
	{
		base.JoinTween(_param);

		var data = _param as ParamData;

		m_Sequence.Join(transform.DOLocalMove(data.EndPosition,m_Duration));
	}
}