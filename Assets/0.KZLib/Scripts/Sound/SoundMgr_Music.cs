using System;
using DG.Tweening;
using UnityEngine;

namespace KZLib
{
	public partial class SoundMgr : LoadSingletonMB<SoundMgr>
	{
		[SerializeField]
		private AudioSource m_MusicSource = null;

		private float m_MusicVolume = 1.0f;
		private bool m_MusicMute = false;

		public bool IsPlaying => m_MusicSource.isPlaying;

		public string GetNowPlayingName()
		{
			return m_MusicSource.clip == null ? string.Empty : m_MusicSource.clip.name;
		}

		public void PlayMusic(AudioClip _clip,float _time = 0.0f,Action<float> _onProgress = null,Action _onComplete = null)
		{
			var loop = _onComplete == null;

			SetAudioSource(m_MusicSource,_clip,string.Format("[Music] {0}",_clip.name),GetAudioMixerGroup(MIXER_MUSIC),loop,m_MusicMute,m_MusicVolume);

			m_MusicSource.time = _time;
			m_MusicSource.Play();

			if(_onProgress == null && _onComplete == null)
			{
				return;
			}

			AddSequence(m_MusicSource,PlayProgress(m_MusicSource,_onProgress,_onComplete));
		}

		public void PlayMusicFadeIn(AudioClip _clip,float _fadeDuration,float _time = 0.0f)
		{
			if(_fadeDuration <= 0.0f)
			{
				return;
			}

			PlayMusic(_clip,_time);

			AddSequence(m_MusicSource,TweenTools.PlayAudioFader(m_MusicSource,new Vector3(_fadeDuration,0.0f,0.0f),m_MusicSource.volume));
		}

		public bool RestartMusic(float _time = 0.0f)
		{
			return RestartSound(m_MusicSource,_time);
		}

		public void PauseMusic()
		{
			PauseAudio(m_MusicSource);
		}

		public void UnPauseMusic()
		{
			UnPauseAudio(m_MusicSource);
		}

        public void StopMusic(bool _clear)
		{
			KillSequence(m_MusicSource);

			m_MusicSource.Stop();

			if(_clear)
			{
				m_MusicSource.clip = null;
			}
		}

        public void StopMusicFadeOut(float _fadeDuration,bool _clear)
		{
			if(_fadeDuration <= 0.0f)
			{
				StopMusic(_clear);

				return;
			}

			var volume = m_MusicSource.volume;
			var sequence = TweenTools.PlayAudioFader(m_MusicSource,new Vector3(0.0f,0.0f,_fadeDuration),m_MusicSource.volume);

			sequence.OnComplete(()=>
			{
				m_MusicSource.volume = volume;

				StopMusic(_clear);
			});

			AddSequence(m_MusicSource,sequence);
		}

		public Sequence PlayMusicFadeInOut(AudioClip _clip,Vector3 _fadeDuration,float _time = 0.0f)
		{
			PlayMusic(_clip,_time);

			var sequence = TweenTools.PlayAudioFader(m_MusicSource,_fadeDuration,m_MusicSource.volume).OnStepComplete(()=>
			{
				PlayMusic(_clip,_time);
			});

			AddSequence(m_MusicSource,sequence);

			return sequence;
		}
	}
}