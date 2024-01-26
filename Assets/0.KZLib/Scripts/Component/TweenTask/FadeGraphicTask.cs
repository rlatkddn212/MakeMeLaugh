using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KZLib.KZAttribute;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FadeGraphicTask : TweenTask
{
	[SerializeField,LabelText("그래픽 리스트")]
	private List<Graphic> m_GraphicList = null;

	[BoxGroup("페이드",ShowLabel = false,Order = 0),SerializeField,LabelText("페이드 인 시간")]
	private float m_FadeInTime = 0.0f;
	[BoxGroup("페이드",ShowLabel = false,Order = 0),SerializeField,LabelText("페이드 유지 시간")]
	private float m_KeepDuration = 0.0f;
	[BoxGroup("페이드",ShowLabel = false,Order = 0),SerializeField,LabelText("페이드 아웃 시간")]
	private float m_FadeOutTime = 0.0f;
	[BoxGroup("페이드",ShowLabel = false,Order = 0),ShowInInspector,LabelText("페이드 총 시간"),KZRichText]
	private string TotalTime => string.Format("{0} 초",m_FadeInTime+m_KeepDuration+m_FadeOutTime);

	protected override async UniTask GetTaskAsync()
	{
		await UniTaskTools.FadeInKeepOutAsync(m_GraphicList,new Vector3(m_FadeInTime,m_KeepDuration,m_FadeOutTime),m_IgnoreTimeScale,m_Source.Token);
	}
}