using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib
{
	public partial class SoundMgr : LoadSingletonMB<SoundMgr>
	{
		private const int EFFECT_MAX_COUNT = 15;

		[SerializeField]
		private Transform m_EffectBox = null;

		private readonly List<AudioSource> m_EffectList = new(EFFECT_MAX_COUNT);

		private float m_EffectVolume = 1.0f;
		private bool m_EffectMute = false;

		public void PlayEffect(AudioClip _clip,bool _ignoreTime = false,int _count = 1)
		{
			if(_count == 0)
			{
				return;
			}

			var source = SetEffectSource(_clip);

			source.PlayOneShot(_clip);

			source.ignoreListenerPause = _ignoreTime;

			if(_count != 1)
			{
				var count = _count > 0 ? _count-1 : -1;

				var sequence = DOTween.Sequence().AppendInterval(_clip.length).SetLoops(count).OnStepComplete(()=>
				{
					source.PlayOneShot(_clip);
				});

				AddSequence(source,sequence);
			}
		}

		public void StopAllEffect()
		{
			for(var i=0;i<m_EffectList.Count;i++)
			{
				StopEffect(m_EffectList[i]);
			}
		}

		public void StopEffect(string _clipName)
		{
			var effect = m_EffectList.Find(x=>x.clip.name.IsEqual(_clipName));

			if(effect)
			{
				StopEffect(effect);
			}
		}

		public void StopEffect(AudioClip _clip)
		{
			var effect = m_EffectList.Find(x=>x.clip.Equals(_clip));

			if(effect)
			{
				StopEffect(effect);
			}
		}

		public void StopEffect(AudioSource _source)
		{
			_source.Stop();

			KillSequence(_source);
		}

		private AudioSource SetEffectSource(AudioClip _clip)
		{
			var source = GetEmptyEffect();

			if(!source)
			{
				Log.Sound.W("소스가 꽉차서 추가합니다. 현재 : {0}",m_EffectList.Count);

				source = CreateEffectSource(m_EffectList.Count);
			}

			SetAudioSource(source,_clip,string.Format("[Effect] {0}",_clip.name),GetAudioMixerGroup(MIXER_EFFECT),false,m_EffectMute,m_EffectVolume);

			return source;
		}

		private AudioSource GetEmptyEffect()
		{
			//? 현재 동작중이지 않은 사운드클립의 인덱스를 검색.
			for(var i=0;i<m_EffectList.Count;i++)
			{
				if(!m_EffectList[i].isPlaying)
				{
					return m_EffectList[i];
				}
			}

			//? 전부다 사용중
			return null;
		}

		private AudioSource CreateEffectSource(int _order)
		{
			var child = m_EffectBox.AddChild(string.Format("SoundEffect_{0}",_order));
			var source = child.gameObject.AddComponent<AudioSource>();

			source.loop = false;
			source.playOnAwake = false;

			return source;
		}
	}
}