using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrailRendererEffectClip : EffectClip
{
	public new record ParamData : EffectClip.ParamData
	{
		public Vector3 EndPosition { get; }

		public Gradient TrailColor { get; }

		public ParamData(Vector3 _endPosition,Gradient _trailColor = null,Action _onComplete = null) : base(_onComplete)
		{
			EndPosition = _endPosition;
			TrailColor = _trailColor;
		}
	}

	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("트레일 렌더러"),SerializeField]
	protected TrailRenderer m_TrailRenderer = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_TrailRenderer)
		{
			m_TrailRenderer = GetComponent<TrailRenderer>();
		}

		if(m_Duration == 0.0f)
		{
			m_Duration = m_TrailRenderer.time;
		}
	}

	protected override void DoInitialize(EffectClip.ParamData _param)
	{
		base.DoInitialize(_param);
		
		var data = _param as ParamData;

		if(data.TrailColor != null)
		{
			m_TrailRenderer.colorGradient = data.TrailColor;
		}
	}

	protected override void JoinTween(EffectClip.ParamData _param)
	{
		base.JoinTween(_param);

		var data = _param as ParamData;

		m_Sequence.Join(transform.DOLocalMove(data.EndPosition,m_Duration));
	}
}