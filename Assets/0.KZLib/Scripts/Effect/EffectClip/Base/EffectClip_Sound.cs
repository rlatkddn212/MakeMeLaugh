using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public abstract partial class EffectClip : BaseComponent
{
	[BoxGroup("이펙트 설정/사운드",Order = EFFECT_SETTING_SOUND_ORDER),SerializeField,LabelText("오디오 소스"),ShowIf("UseSound")]
	protected AudioSource m_AudioSource = null;

	[BoxGroup("이펙트 설정/사운드",Order = EFFECT_SETTING_SOUND_ORDER),SerializeField,LabelText("피치 커브 사용"),ShowIf("UseSound")]
	private bool m_UsePitchCurve = false;

	private bool UsePitchCurve => UseSound && m_UsePitchCurve;

	[BoxGroup("이펙트 설정/사운드",Order = EFFECT_SETTING_SOUND_ORDER),SerializeField,LabelText("오디오 피치 커브"),ShowIf("UsePitchCurve")]
	protected AnimationCurve m_AudioPitchCurve = AnimationCurve.EaseInOut(0.0f,0.0f,1.0f,1.0f);

	[BoxGroup("이펙트 설정/사운드",Order = EFFECT_SETTING_SOUND_ORDER),SerializeField,LabelText("피치 커브 사용"),ShowIf("UseSound")]
	private bool m_UseVolumeCurve = false;

	private bool UseVolumeCurve => UseSound && m_UseVolumeCurve;

	[BoxGroup("이펙트 설정/사운드",Order = EFFECT_SETTING_SOUND_ORDER),SerializeField,LabelText("오디오 피치 커브"),ShowIf("UseVolumeCurve")]
	protected AnimationCurve m_AudioVolumeCurve = AnimationCurve.EaseInOut(0.0f,0.0f,1.0f,1.0f);

	private float m_StartPitch = 0.0f;
	private float m_StartVolume = 0.0f;

	protected virtual void InitializeSound(ParamData _param)
	{
		m_AudioSource.playOnAwake = false;
		m_AudioSource.loop = false;

		m_StartPitch = m_AudioSource.pitch;
		m_StartVolume = m_AudioSource.volume;

		m_AudioSource.pitch = m_AudioPitchCurve.Evaluate(0.0f);
		m_AudioSource.volume = m_AudioVolumeCurve.Evaluate(0.0f);
	}

	protected virtual void JoinSoundTween(ParamData _param)
	{
		var duration = m_AudioSource.clip.length;

		m_Sequence.Join(TweenTools.SetProgress(0.0f,m_AudioPitchCurve.length,m_Duration,(progress)=>
		{
			if(UsePitchCurve)
			{
				m_AudioSource.pitch = m_AudioPitchCurve.Evaluate(progress)*m_StartPitch;
			}

			if(UseVolumeCurve)
			{
				m_AudioSource.volume = m_AudioVolumeCurve.Evaluate(progress)*m_StartVolume;
			}
		}));
	}

	private void ResetSound() 
	{
		if(!m_AudioSource)
		{
			m_AudioSource = GetComponentInChildren<AudioSource>();

			if(m_AudioSource)
			{
				m_AudioSource.playOnAwake = false;
				m_AudioSource.loop = false;
			}
		}
	}
}