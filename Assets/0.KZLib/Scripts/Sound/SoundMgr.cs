using System;
using System.Collections.Generic;
using DG.Tweening;
using KZLib.KZDevelop;
using UnityEngine;
using UnityEngine.Audio;

namespace KZLib
{
	public partial class SoundMgr : LoadSingletonMB<SoundMgr>
	{
		private const float SOUND_DISTANCE = 15f;

		private bool m_AudioPause = false;

		public bool IsAudioPausing => m_AudioPause;

		private readonly Dictionary<AudioSource,Sequence> m_SequenceDict = new();

		protected override void Initialize()
		{
			if(m_Mixer)
			{
				m_MixerDict.AddRange(m_Mixer.FindMatchingGroups(MIXER_MASTER));
			}

			for(var i=0;i<EFFECT_MAX_COUNT;i++)
			{
				m_EffectList.Add(CreateEffectSource(i));
			}

			UpdateSound();

			Broadcaster.EnableListener(EventTag.ChangeSound,UpdateSound);
		}

		protected override void Release()
		{
			foreach(var pair in m_SequenceDict)
			{
				pair.Value.Kill();
			}

			m_MixerDict.Clear();

			m_EffectList.Clear();
			m_SequenceDict.Clear();

			Broadcaster.DisableListener(EventTag.ChangeSound,UpdateSound);
		}

		private void UpdateSound()
		{
			var option = GameDataMgr.In.Access<GameData.Option>().SoundOption;

			var master = option.MaserSound;
			var music = option.MusicSound;
			var effect = option.EffectSound;

			m_MusicVolume = master.Volume*music.Volume;
			m_MusicMute = master.Mute || music.Mute;

			m_EffectVolume = master.Volume*effect.Volume;
			m_EffectMute = master.Mute || effect.Mute;
		}

		private void SetAudioSource(AudioSource _source,AudioClip _clip,string _name,AudioMixerGroup _mixerGroup,bool _loop,bool _mute,float _volume)
		{
			_source.name = _name;
			_source.clip = _clip;
			_source.outputAudioMixerGroup = _mixerGroup;
			_source.loop = _loop;
			_source.pitch = 1.0f;
			_source.ignoreListenerPause = false;

			_source.volume = _volume;
			_source.mute = _mute;
		}

		private void AddSequence(AudioSource _source,Sequence _sequence)
		{
			KillSequence(_source);

			m_SequenceDict.Add(_source,_sequence);

			_sequence.Play();
		}

		private void KillSequence(AudioSource _source)
		{
			if(m_SequenceDict.TryGetValue(_source,out var sequence))
			{
				sequence.Kill();

				m_SequenceDict.RemoveSafe(_source);
			}
		}

		public void PauseAll(bool _pause)
		{
			m_AudioPause = _pause;

			if(m_AudioPause)
			{
				PauseAudio(m_MusicSource);

				for(var i=0;i<m_EffectList.Count;i++)
				{
					PauseAudio(m_EffectList[i]);
				}
			}
			else
			{
				UnPauseAudio(m_MusicSource);

				for(var i=0;i<m_EffectList.Count;i++)
				{
					UnPauseAudio(m_EffectList[i]);
				}
			}
		}

		private void PauseAudio(AudioSource _source)
		{
			_source.Pause();

			if(m_SequenceDict.TryGetValue(_source,out var sequence))
			{
				sequence.Pause();
			}
		}

		private void UnPauseAudio(AudioSource _source)
		{
			_source.UnPause();

			if(m_SequenceDict.TryGetValue(_source,out var sequence))
			{
				sequence.Play();
			}
		}

		private bool RestartSound(AudioSource _source,float _time = 0.0f)
		{
			if(!IsPlayingSource(_source))
			{
				return false;
			}

			_source.time = _time;
			_source.Play();

			return true;
		}

		private bool IsPlayingSource(AudioSource _source)
		{
			return _source.clip && _source.isPlaying;
		}

		private Sequence PlayProgress(AudioSource _source,Action<float> _onProgress,Action _onComplete)
		{
			var length = _source.clip.length;

			return DOTween.Sequence().Append(TweenTools.SetProgress(_source.time,length,length,_onProgress,_onComplete));
		}
	}
}