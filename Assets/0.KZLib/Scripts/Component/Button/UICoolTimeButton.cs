using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class UICoolTimeButton : UIBaseButton
{
	[SerializeField,HideInInspector]
	private float m_Duration = 0.0f;

	[SerializeField,HideInInspector]
	private float m_CurrentTime = 0.0f;

	[LabelText("총 시간"),ShowInInspector,DisplayAsString]
	private float Duration => m_Duration;

	[LabelText("현재 시간"),ShowInInspector,DisplayAsString]
	private float CurrentTime => m_CurrentTime;

	private bool m_IgnoreTime = false;
	private bool m_IsPaused = false;

	private Action m_OnStart = null;
	private Action<float> m_OnUpdate = null;
	private Action m_OnComplete = null;

	private CancellationTokenSource  m_Source = null;

	protected override void Awake()
	{
		base.Awake();

		m_OnStart += () => { m_Button.enabled = false; };
		m_OnUpdate += (progress) => { m_CurrentTime = progress; };
		m_OnComplete += () => { m_Button.enabled = true; };

		m_Button.SetOnClickListener(()=> { RestartCoolDown(); });
	}

	private void OnEnable()
	{
		m_IsPaused = false;

		m_Source?.Dispose();
		m_Source = new();

		ClickedButtonAsync().Forget();
	}

	private void OnDisable()
	{
		m_Source?.Cancel();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		m_Source?.Cancel();
		m_Source?.Dispose();
	}

	public void SetButton(float _duration,bool _ignoreTime = false,Action _onStart = null,Action<float> _onUpdate = null,Action _onComplete = null)
	{
		m_Duration = _duration;

		m_IgnoreTime = _ignoreTime;

		m_OnStart += _onStart;
		m_OnUpdate += _onUpdate;
		m_OnComplete += _onComplete;
	}

	private async UniTask ClickedButtonAsync()
    {
		m_OnStart?.Invoke();
		var duration = m_Duration - m_CurrentTime;

        await UniTaskTools.ExecuteOverTimeAsync(m_CurrentTime,m_Duration,duration,m_OnUpdate,m_IgnoreTime,null,m_Source.Token);

		m_OnComplete?.Invoke();
    }

	public void PauseCoolDown(bool _pause)
	{
		m_IsPaused = _pause;
	}

	public void RestartCoolDown()
	{
		m_Duration = 0.0f;
		m_IsPaused = false;

		ClickedButtonAsync().Forget();
	}
}