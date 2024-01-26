using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 설정한 시간만큼 카운트 한다.
/// </summary>
public class UILoopText : UIBaseText
{
	[SerializeField,LabelText("루프 텍스트들")]
	private List<string> m_LoopTextList = new();
	[SerializeField,LabelText("무작위 모드")]
	private bool m_RandomMode = false;
	[SerializeField,LabelText("보여지는 시간"),MinValue(0.01f)]
	private float m_ShowDuration = 0.01f;

	private CancellationTokenSource m_Source = null;

	protected override void Awake()
	{
		base.Awake();

		m_LoopTextList = m_LoopTextList.Where(x=>!x.IsEmpty()).ToList();
	}

	private void OnEnable()
	{
		m_Source?.Dispose();
		m_Source = new();

		PlayLoopText();
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

	public void PlayLoopText()
	{
		PlayLoopTextAsync().Forget();
	}

	public async UniTask PlayLoopTextAsync()
	{
		var textList = new List<string>(m_LoopTextList);

		if(m_RandomMode)
		{
			textList.Randomize();
		}

		while(true)
		{
			for(var i=0;i<textList.Count;i++)
			{
				await UniTask.WaitForSeconds(m_ShowDuration,false,cancellationToken : m_Source.Token);

				if(m_Source.IsCancellationRequested)
				{
					return;
				}
			}
		}
	}
}