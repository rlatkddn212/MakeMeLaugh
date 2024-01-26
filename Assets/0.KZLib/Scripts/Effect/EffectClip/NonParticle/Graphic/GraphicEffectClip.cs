using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class GraphicEffectClip : EffectClip
{
	[SerializeField,HideInInspector]
	private Vector3 m_FadeDuration = Vector3.zero;

	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("페이드 시간"),ShowInInspector]
	protected Vector3 FadeDuration
	{
		get => m_FadeDuration;
		set
		{
			if(m_FadeDuration == value)
			{
				return;
			}

			m_FadeDuration = value;
			m_Duration = m_FadeDuration.x+m_FadeDuration.y+m_FadeDuration.z;
		}
	}

	protected void JoinGraphicTween(Graphic _graphic)
	{
		m_Sequence.Join(TweenTools.PlayGraphicFader(_graphic,m_FadeDuration));
	}
}