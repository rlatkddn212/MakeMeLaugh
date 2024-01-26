using UnityEngine;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ImageEffectClip : GraphicEffectClip
{
	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("이미지"),SerializeField]
	private Image m_Image = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_Image)
		{
			m_Image = GetComponent<Image>();
		}
	}

	protected override void JoinTween(ParamData _param)
	{
		base.JoinTween(_param);

		JoinGraphicTween(m_Image);
	}
}