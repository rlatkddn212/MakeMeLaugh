using UnityEngine;
using DG.Tweening;

public abstract partial class EffectClip : BaseComponent
{
	protected Sequence m_Sequence = null;

	public YieldInstruction WaitForEffect => m_Sequence.WaitForCompletion();

	private void InitializeTween(ParamData _param)
	{
		m_Sequence = DOTween.Sequence();

		m_CurrentTime = 0.0f;

		var tween = DOTween.To(()=>m_CurrentTime,x => m_CurrentTime = x,m_Duration,m_Duration).OnComplete(() => { EndEffect(false); });

		if(_param != null && _param.OnComplete != null)
		{
			tween.onComplete += ()=> { _param.OnComplete?.Invoke(); };
		}

		m_Sequence.Join(tween);

		// 필요한거 join으로 추가
		JoinTween(_param);

		if(m_IgnoreTimeScale)
		{
			m_Sequence.SetUpdate(m_IgnoreTimeScale);
		}

		if(m_UseLoop)
		{
			m_Sequence.SetLoops(-1);
		}
	}

	protected virtual void JoinTween(ParamData _param)
	{
		if(UseSound)
		{
			JoinSoundTween(_param);
		}
	}	
}