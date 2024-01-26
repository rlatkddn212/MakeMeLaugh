using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class UIRepeaterButton : UIBaseButton,IPointerDownHandler,IPointerUpHandler
{
	[SerializeField]
	private AnimationCurve m_ClickSpeedCurve = new();

	private CancellationTokenSource  m_Source = new();

	private Action m_OnPushDown = null;

	public event Action OnPushDown
	{
		add { m_OnPushDown -= value; m_OnPushDown += value; }
		remove { m_OnPushDown -= value; }
	}

	private Action m_OnPushUp = null;

	public event Action OnPushUp
	{
		add { m_OnPushUp -= value; m_OnPushUp += value; }
		remove { m_OnPushUp -= value; }
	}

	private float m_PressedTime = 0.0f;
	private bool m_IsPressing = false;

	private bool IsPressing
	{
		get
		{
			return m_IsPressing;
		}
		
		set
		{
			m_IsPressing = value;

			if(value)
			{
				m_PressedTime = Time.realtimeSinceStartup;

				m_Source?.Dispose();
				m_Source = new();

				PressRepeatAsync().Forget();
			}
			else
			{
				m_PressedTime = 0.0f;

				m_Source?.Cancel();
			}
		}
	}

	void OnEnable()
	{
		IsPressing = false;
	}

	void OnDisable()
	{
		IsPressing = false;
	}

	public void OnPointerDown(PointerEventData _data)
	{
		if(!IsPressing)
		{
			m_OnPushDown?.Invoke();

			IsPressing = true;
		}
	}

	public void OnPointerUp(PointerEventData _data)
	{
		m_OnPushUp?.Invoke();

		IsPressing = false;
	}
	
	private async UniTask PressRepeatAsync()
	{
		while(true)
		{
			ClickButton();

			var delay = m_ClickSpeedCurve.Evaluate(Time.realtimeSinceStartup-m_PressedTime);

			await UniTask.Delay(TimeSpan.FromSeconds(delay),false,cancellationToken : m_Source.Token);
		}
	}

	private void ClickButton()
	{
		m_Button.onClick?.Invoke();
	}
}