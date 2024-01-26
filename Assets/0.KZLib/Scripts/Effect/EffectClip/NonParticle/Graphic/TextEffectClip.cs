using UnityEngine;
using TMPro;
using System;
using Sirenix.OdinInspector;

public class TextEffectClip : GraphicEffectClip
{
	public new record ParamData : EffectClip.ParamData
	{
		public string Sentence { get; }
		public float FontSize { get; }
		public Color? StartColor { get; }

		public ParamData(string _sentence,float _size,Color? _color = null,Action _onComplete = null) : base(_onComplete)
		{
			Sentence = _sentence;
			FontSize = _size;
			StartColor = _color;
		}
	}

	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("텍스트 메쉬"),SerializeField]
	private TMP_Text m_Text = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_Text)
		{
			m_Text = GetComponent<TMP_Text>();
		}
	}

	protected override void JoinTween(EffectClip.ParamData _param)
	{
		base.JoinTween(_param);
		
		var data = _param as ParamData;

		m_Text.fontSize = data.FontSize;

		m_Text.SetSafeTextMeshPro(data.Sentence,data.StartColor);

		JoinGraphicTween(m_Text);
	}
}