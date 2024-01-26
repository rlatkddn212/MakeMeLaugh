using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class UITaskButton : UIBaseButton
{
	private class TaskData : IEquatable<TaskData>
	{
		private readonly Func<UniTask> m_OnTask = null;
		private readonly Action m_OnAction = null;

		public TaskData(Func<UniTask> _onTask,Action _onAction)
		{
			m_OnTask = _onTask;
			m_OnAction = _onAction;
		}

		public bool Equals(TaskData _data)
		{
			return m_OnTask == _data.m_OnTask && m_OnAction == _data.m_OnAction;
		}

		public async UniTask Run()
		{
			m_OnAction?.Invoke();

			if(m_OnTask != null)
			{
				await m_OnTask.Invoke();
			}
		}
	}

	[SerializeField]
	protected float m_Duration = 0.0f;
	public float Duration => m_Duration;

	private readonly List<TaskData> m_TaskDataList = new();

	protected CancellationTokenSource m_Source = null;

	protected override void Awake()
	{
		base.Awake();

		m_Button.SetOnClickListener(()=>
		{
			OnClickedAsync().Forget();
		});

		m_TaskDataList.Clear();
	}

	protected virtual void OnEnable()
	{
		m_Source?.Dispose();
		m_Source = new();
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

	public void AddTaskData(Func<UniTask> _onTask = null,Action _onAction = null)
	{
		m_TaskDataList.Add(new TaskData(_onTask,_onAction));
	}

	public void RemoveTask(Func<UniTask> _onTask = null,Action _onAction = null)
	{
		m_TaskDataList.Remove(new TaskData(_onTask,_onAction));
	}

	private async UniTaskVoid OnClickedAsync()
	{
		await DoClickedAsync();

		for(var i=0;i<m_TaskDataList.Count;i++)
		{
			var data = m_TaskDataList[i];

			await data.Run();
		}
	}

	protected abstract UniTask DoClickedAsync();
}