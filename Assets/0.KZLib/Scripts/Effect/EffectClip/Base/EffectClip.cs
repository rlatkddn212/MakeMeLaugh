using KZLib;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using KZLib.KZAttribute;
using DG.Tweening;

public abstract partial class EffectClip : BaseComponent
{
	private const int EFFECT_DURATION_ORDER = -10;
	protected const int EFFECT_SETTING_ORDER = -5;

	private const int EFFECT_SETTING_SOUND_ORDER = 10;

	public record ParamData(Action OnComplete);

	[SerializeField,HideInInspector]
	protected float m_CurrentTime = 0.0f;

	[VerticalGroup("이펙트 시간",Order = EFFECT_DURATION_ORDER),LabelText("현재 시간"),ShowInInspector,KZRichText]
	public string CurrentTime => string.Format("{0} 초",m_CurrentTime);

	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("실행 시간"),SerializeField,ValidateInput("IsPlayable","지속시간이 0으로 설정되어 있습니다.",InfoMessageType.Error)]
	protected float m_Duration = 0.0f;
	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),LabelText("반복 사용"),SerializeField]
	protected bool m_UseLoop = false;

	[BoxGroup("이펙트 설정",ShowLabel = false,Order = EFFECT_SETTING_ORDER),SerializeField,LabelText("타임스케일 무시")]
	protected bool m_IgnoreTimeScale = false;

	private bool IsPlayable => m_Duration > 0.0f;

	public float Duration => m_Duration;
	public float Progress => IsPlayable ? m_CurrentTime/m_Duration : 0.0f;

	protected virtual bool UseSound => false;

	public virtual void SetIgnoreTimeScale(bool _use)
	{
		m_IgnoreTimeScale = _use;
	}

	protected virtual void DoInitialize(ParamData _param) { }

	public void Initialize(ParamData _param)
	{
		DoInitialize(_param);

		InitializeTween(_param);

		if(UseSound)
		{
			InitializeSound(_param);
		}
	}

	protected virtual void Start()
	{
		m_CurrentTime = 0.0f;

		if(!IsPlayable)
		{
			Log.Effect.E("{0}의 지속시간이 0으로 설정되어 있습니다.",gameObject.name);

			EndEffect(true);

			return;
		}

		if(UseSound)
		{
			m_AudioSource.Play();
		}
	}

	protected virtual void EndEffect(bool _destroy = false)
	{
		ForceEndEffect();

		if(_destroy)
		{
			ObjectTools.DestroyObject(gameObject);
			
			return;
		}

		if(EffectMgr.HasInstance)
		{
			EffectMgr.In.ReleaseEffect(this);
		}
	}

	public void ForceEndEffect()
	{
		m_Sequence.Kill(true);
	}

	protected override void Reset() 
	{
		base.Reset();

		if(UseSound)
		{
			ResetSound();
		}
	}
}