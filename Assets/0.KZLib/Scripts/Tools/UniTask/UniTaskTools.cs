using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public static partial class UniTaskTools
{
	public static async UniTask LoopUniTaskAsync(Func<UniTask> _task,int _count,CancellationToken _token)
	{
		await LoopAsync(null,_task,_count,_token);
	}

	public static async UniTask LoopActionWaitSecondeAsync(Action _onAction,float _second,int _count,CancellationToken _token)
	{
		await LoopAsync(_onAction,()=>
		{
			return UniTask.WaitForSeconds(_second,cancellationToken : _token);
		},_count,_token);
	}

	public static async UniTask LoopActionWaitFrameAsync(Action _onAction,int _count,CancellationToken _token)
	{
		await LoopAsync(_onAction,()=>
		{
			return UniTask.Yield(_token);
		},_count,_token);
	}

	private static async UniTask LoopAsync(Action _onAction,Func<UniTask> _task,int _count,CancellationToken _token)
	{
		if(_count == 0)
		{
			return;
		}

		var count = _count;

		while(count == -1 || count-- > 0)
		{
			if(_token.IsCancellationRequested)
			{
				return;
			}

			_onAction?.Invoke();

			if(_task != null)
			{
				await _task.Invoke();
			}
		}
	}
}