using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UITweenTaskButton : UIBaseButton
{
	[SerializeField]
	private List<TweenTask> m_TweenTaskList = new();

	protected override void Awake()
	{
		base.Awake();

		m_Button.SetOnClickListener(()=>
		{
			PlayTaskAsync().Forget();
		});
	}

	private async UniTaskVoid PlayTaskAsync()
	{
		var taskList = new List<UniTask>();

		for(var i=0;i<m_TweenTaskList.Count;i++)
		{
			taskList.Add(UniTask.Defer(m_TweenTaskList[i].PlayTaskAsync));
		}

		if(taskList.Count > 1)
		{
			await UniTask.WhenAll(taskList);
		}
		else if(taskList.Count == 1)
		{
			await taskList[0];
		}
	}
}