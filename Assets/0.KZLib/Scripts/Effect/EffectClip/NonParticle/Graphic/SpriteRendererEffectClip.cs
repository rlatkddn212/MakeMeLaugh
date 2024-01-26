using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class SpriteRendererEffectClip : GraphicEffectClip
{
	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("렌더러"),SerializeField]
	protected SpriteRenderer m_Renderer = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_Renderer)
		{
			m_Renderer = GetComponent<SpriteRenderer>();
		}
	}

	protected override void JoinTween(ParamData _param)
	{
		base.JoinTween(_param);

		m_Sequence.Join(TweenTools.PlaySpriteRendererFader(m_Renderer,FadeDuration));
	}
}