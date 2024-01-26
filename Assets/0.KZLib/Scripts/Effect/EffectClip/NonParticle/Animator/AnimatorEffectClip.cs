using UnityEngine;

#if UNITY_EDITOR

using UnityEditor.Animations;

#endif

public class AnimatorEffectClip : EffectClip
{
	[SerializeField]
	private Animator m_Animator = null;

	[SerializeField]
	private string m_AnimatorName = null;

	protected override void Start()
	{
		base.Start();

		if(m_AnimatorName.IsEmpty())
		{
			return;
		}

		m_Animator.Play(m_AnimatorName);
	}

	protected override void Reset()
	{
		base.Reset();

		if(!m_Animator)
		{
			m_Animator = GetComponent<Animator>();
		}

		m_Duration = m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

#if UNITY_EDITOR
		var controller = m_Animator.runtimeAnimatorController as AnimatorController;
		var stateMachine = controller.layers[0].stateMachine;

		m_AnimatorName = stateMachine.states[0].state.name;
#endif
	}
}