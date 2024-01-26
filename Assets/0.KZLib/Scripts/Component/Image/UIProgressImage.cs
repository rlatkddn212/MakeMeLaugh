using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UIProgressImage : UIBaseImage
{
	private const int RANGE_ORDER = 0;
	private const int OPTION_ORDER = 1;
	private const int OPTION_COLOR_ORDER = 0;
	private const int VALUE_ORDER = 2;

	[SerializeField,HideInInspector]
	private float m_MinValue = 0.0f;
	[SerializeField,HideInInspector]
	private float m_MaxValue = 1.0f;

	[SerializeField,HideInInspector]
	private float m_NowValue = 1.0f;

	[FoldoutGroup("범위",RANGE_ORDER)]
	[InfoBox("$m_ErrorMessage",InfoMessageType.Error,"@string.IsNullOrEmpty(this.m_ErrorMessage)")]
	[VerticalGroup("범위/0"),LabelText("최솟값"),ShowInInspector]
	private float MinValue
	{
		get => m_MinValue;
		set
		{
			if(m_MinValue == value)
			{
				return;
			}

			m_ErrorMessage = string.Empty;

			if(value > m_MaxValue)
			{
				m_ErrorMessage = "최솟 값이 오류 입니다.";

				return;
			}

			m_MinValue = value;
		}
	}

	[VerticalGroup("범위/0"),LabelText("최댓값"),ShowInInspector]
	private float MaxValue
	{
		get => m_MaxValue;
		set
		{
			if(m_MaxValue == value)
			{
				return;
			}

			m_ErrorMessage = string.Empty;

			if(value < m_MinValue)
			{
				m_ErrorMessage = "최댓 값이 오류 입니다.";

				return;
			}

			m_MaxValue = value;
		}
	}

	[FoldoutGroup("옵션",OPTION_ORDER)]
	[VerticalGroup("옵션/색상",Order = OPTION_COLOR_ORDER),LabelText("색상 사용"),SerializeField]
	private readonly bool m_UseColor = false;
	[VerticalGroup("옵션/색상",Order = OPTION_COLOR_ORDER),LabelText("그라데이션 색상"),SerializeField,ShowIf("m_UseColor")]
	private readonly Gradient m_GradientColor = null;

	[VerticalGroup("값",Order = VALUE_ORDER),ShowInInspector,LabelText("현재 값")]
	public float NowValue { get => m_NowValue; private set => SetValue(value); }

	public float NowProgress => m_NowValue/(m_MaxValue-m_MinValue);
	private string m_ErrorMessage = string.Empty;

	protected override void Awake()
	{
		base.Awake();

		SetValue(m_MinValue);
	}

	public void SetRange(float _min,float _max)
	{
		if(_min > _max)
		{
			throw new ArgumentOutOfRangeException(string.Format("최소와 최대가 오류 입니다. 최소:{0}/최대{1}",_min,_max));
		}

		m_MinValue = _min;
		m_MaxValue = _max;
	}

	public void SetValue(float _value)
	{
		m_NowValue = Mathf.Clamp(_value,m_MinValue,m_MaxValue);

		if(m_Image)
		{
			m_Image.fillAmount = NowProgress;
		}

		if(m_UseColor)
		{
			m_Image.color = m_GradientColor.Evaluate(NowProgress);
		}
	}
	
	public void SetProgress(float _nowValue,float _maxValue)
	{
		SetRange(0.0f,_maxValue);

		SetValue(_nowValue);
	}

	public void SetValueDuration(float _value,float _duration)
	{
		UniTaskTools.ExecuteOverTimeAsync(m_NowValue,_value,_duration,SetValue,false).Forget();
	}

	protected override void Reset() 
	{
		base.Reset();

		m_Image.type = Image.Type.Filled;
		m_Image.fillOrigin = 0;
		m_Image.fillAmount = 0.0f;

		m_MinValue = 0.0f;
		m_MaxValue = 1.0f;

		m_NowValue = 0.0f;
	}
}