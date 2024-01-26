using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class TweenTask : BaseComponent
{
    [BoxGroup("기본 기능"),SerializeField,LabelText("자동 재생")]
	protected bool m_AutoPlay = false;

	[BoxGroup("기본 기능",ShowLabel = false,Order = 1),SerializeField,LabelText("반복 횟수"),PropertyTooltip("-1은 무한/0은 작동 안함")]
	protected int m_LoopCount = 1;
	[BoxGroup("기본 기능",ShowLabel = false,Order = 2),SerializeField,LabelText("스킵 버튼")]
	protected Button m_SkipButton = null;

	[BoxGroup("기본 기능",ShowLabel = false,Order = 3),SerializeField,LabelText("시간 무시")]
	protected bool m_IgnoreTimeScale = false;

	protected CancellationTokenSource m_Source = null;

	public void Initialize()
	{
		if(m_SkipButton)
		{
			m_SkipButton.SetOnClickListener(() => { gameObject.SetActiveSelf(false); });
		}
	}

	protected virtual void OnEnable()
	{
		m_Source?.Dispose();
		m_Source = new();

		if(m_AutoPlay)
		{
			PlayTask();
		}
	}

	protected virtual void OnDisable()
	{
		m_Source?.Cancel();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		m_Source?.Cancel();
		m_Source?.Dispose();
	}

	public void PlayTask()
	{
		PlayTaskAsync().Forget();
	}

	public async UniTask PlayTaskAsync()
	{
		InitializeTask();

		await UniTaskTools.LoopUniTaskAsync(()=>
		{
			return GetTaskAsync();
		},m_LoopCount,m_Source.Token);
	}

	protected virtual void InitializeTask() { }

	protected abstract UniTask GetTaskAsync();
}