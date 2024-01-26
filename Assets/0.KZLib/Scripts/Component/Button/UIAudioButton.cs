using KZLib;
using UnityEngine;

public class UIAudioButton : UIBaseButton
{
	[SerializeField]
	private AudioClip m_AudioClip = null;

	protected override void Awake()
	{
		base.Awake();

		if(!m_AudioClip)
		{
			return;
		}

		m_Button.SetOnClickListener(()=>
		{
			SoundMgr.In.PlayEffect(m_AudioClip,true);
		});
	}

	public void SetAudio(AudioClip _clip)
	{
		m_AudioClip = _clip;
	}

	public void SetAudio(string _name)
	{
		m_AudioClip = ResMgr.In.GetAudioClip(_name);
	}
}