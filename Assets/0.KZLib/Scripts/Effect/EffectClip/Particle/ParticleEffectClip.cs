using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ParticleEffectClip : EffectClip
{
	public new record ParamData : EffectClip.ParamData
	{
		public Color? StartColor { get; }

		public ParamData(Color? _color = null,Action _onComplete = null) : base(_onComplete)
		{
			StartColor = _color;
		}
	}

	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("메인 파티클"),SerializeField]
	protected ParticleSystem m_MainParticle = null;
	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("서브 파티클들"),SerializeField]
	protected List<ParticleSystem> m_SubParticleList = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_MainParticle)
		{
			m_MainParticle = GetComponent<ParticleSystem>();
		}

		m_Duration = m_MainParticle.main.duration;
	}

	protected override void DoInitialize(EffectClip.ParamData _param)
	{
		base.DoInitialize(_param);
		
		var data = _param as ParamData;

		if(data == null)
		{
			return;
		}

		if(data.StartColor.HasValue)
		{
			SetColor(m_MainParticle.main,data.StartColor.Value);

			foreach(var particle in m_SubParticleList)
			{
				SetColor(particle.main,data.StartColor.Value);
			}
		}
	}

	private void SetColor(ParticleSystem.MainModule _module,Color _color)
	{
		_module.startColor = new ParticleSystem.MinMaxGradient(_color);
	}
}