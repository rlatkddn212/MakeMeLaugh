using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static partial class UniTaskTools
{
	#region Fade
	public static async UniTask FadeInKeepOutAsync(Graphic _graphic,Vector3 _duration,bool _ignoreTimescale = false,CancellationToken _token = default)
	{
		await FadeInKeepOutAsync(new List<Graphic>(1) { _graphic },_duration,_ignoreTimescale,_token);
	}

	public static async UniTask FadeInKeepOutAsync(List<Graphic> _graphicList,Vector3 _duration,bool _ignoreTimescale = false,CancellationToken _token = default)
	{
		if(_duration.x > 0.0f)
		{
			await FadeInAsync(_graphicList,_duration.x,_ignoreTimescale,_token);
		}

		if(_duration.y > 0.0f)
		{
			await UniTask.WaitForSeconds(_duration.y,_ignoreTimescale,cancellationToken : _token);
		}

		if(_duration.z > 0.0f)
		{
			await FadeOutAsync(_graphicList,_duration.z,_ignoreTimescale,_token);
		}
	}

	/// <summary>
	/// 이미지가 점점 보임
	/// </summary>
	public static async UniTask FadeInAsync(Graphic _graphic,float _duration,bool _ignoreTimescale,CancellationToken _token = default)
	{
		await FadeAsync(new List<Graphic>(1) { _graphic },_duration,0.0f,1.0f,_ignoreTimescale,_token);
	}

	/// <summary>
	/// 이미지가 점점 보임
	/// </summary>
	public static async UniTask FadeInAsync(List<Graphic> _graphicList,float _duration,bool _ignoreTimescale,CancellationToken _token = default)
	{
		await FadeAsync(_graphicList,_duration,0.0f,1.0f,_ignoreTimescale,_token);
	}

	/// <summary>
	/// 이미지가 점점 사라짐
	/// </summary>
	public static async UniTask FadeOutAsync(Graphic _graphic,float _duration,bool _ignoreTimescale,CancellationToken _token = default)
	{
		await FadeAsync(new List<Graphic>(1) { _graphic },_duration,1.0f,0.0f,_ignoreTimescale,_token);
	}

	/// <summary>
	/// 이미지가 점점 사라짐
	/// </summary>
	public static async UniTask FadeOutAsync(List<Graphic> _graphicList,float _duration,bool _ignoreTimescale,CancellationToken _token = default)
	{
		await FadeAsync(_graphicList,_duration,1.0f,0.0f,_ignoreTimescale,_token);
	}

	private static async UniTask FadeAsync(List<Graphic> _graphicList,float _duration,float _start,float _finish,bool _ignoreTimescale,CancellationToken _token)
	{
		if(_token.IsCancellationRequested)
		{
			return;
		}

		foreach(var graphic in _graphicList)
		{
			graphic.color = graphic.color.MaskAlpha(_start);
		}

		await ExecuteOverTimeAsync(_start,_finish,_duration,(progress)=>
		{
			foreach(var graphic in _graphicList)
			{
				graphic.color = graphic.color.MaskAlpha(progress);
			}
		},_ignoreTimescale,null,_token);

		if(_token.IsCancellationRequested)
		{
			return;
		}

		foreach(var graphic in _graphicList)
		{
			graphic.color = graphic.color.MaskAlpha(_finish);
		}
	}
	#endregion Fade
}