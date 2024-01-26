using System;
using KZLib;
using KZLib.KZDevelop;

namespace GameData
{
	public partial class Option : IGameData
	{
		private const string SOUND_GROUP = "[Sound] Sound_Group";

		public class Sound
		{
			public class SoundData
			{
				public float Volume { get; set; }
				public bool Mute { get; set; }

				public SoundData() { Volume = 1.0f; Mute = false; }
			}

			public SoundData MaserSound{ get; private set; }
			public SoundData MusicSound { get; private set; }
			public SoundData EffectSound { get; private set; }

			public Sound()
			{
				MaserSound = new();
				MusicSound = new();
				EffectSound = new();
			}

			public void SetMasterSound(float? _volume = null,bool? _mute = null)
			{
				SetSound(MaserSound,_volume,_mute);
			}

			public void SetMusicDataSound(float? _volume = null,bool? _mute = null)
			{
				SetSound(MusicSound,_volume,_mute);
			}

			public void SetEffectDataSound(float? _volume = null,bool? _mute = null)
			{
				SetSound(EffectSound,_volume,_mute);
			}

			private void SetSound(SoundData _data,float? _volume,bool? _mute)
			{
				var flag = false;

				if(_volume.HasValue && _data.Volume != _volume.Value)
				{
					_data.Volume = _volume.Value;

					flag = true;
				}

				if(_mute.HasValue && _data.Mute != _mute.Value)
				{
					_data.Mute = _mute.Value;

					flag = true;
				}

				if(flag)
				{
					Broadcaster.SendEvent(EventTag.ChangeSound);
				}
			}
		}

		private Sound m_SoundOption = null;

		public Sound SoundOption => m_SoundOption;

		private void InitializeSound()
		{
			m_SoundOption = m_SaveData.GetData(SOUND_GROUP,new Sound());

			Broadcaster.EnableListener(EventTag.ChangeSound,SaveSoundData);
		}

		private void ReleaseSound()
		{
			Broadcaster.DisableListener(EventTag.ChangeSound,SaveSoundData);
		}

		private void SaveSoundData()
		{
			m_SaveData.SetData(SOUND_GROUP,m_SoundOption);
		}
	}
}