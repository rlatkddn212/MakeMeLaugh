// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public static partial class CoroutineTools
// {
// 	#region Fade
// 	public static IEnumerator CoFadeInKeepOut(Graphic _graphic,Vector3 _duration,bool _ignoreTimescale = false)
// 	{
// 		yield return CoFadeIn(_graphic,_duration.x,_ignoreTimescale);

// 		yield return CoWaitForSeconds(_duration.y,_ignoreTimescale);

// 		yield return CoFadeOut(_graphic,_duration.z,_ignoreTimescale);
// 	}

// 	public static IEnumerator CoFadeInKeepOut(List<Graphic> _graphicList,Vector3 _duration,bool _ignoreTimescale = false)
// 	{
// 		yield return CoFadeIn(_graphicList,_duration.x,_ignoreTimescale);

// 		yield return CoWaitForSeconds(_duration.y,_ignoreTimescale);

// 		yield return CoFadeOut(_graphicList,_duration.z,_ignoreTimescale);
// 	}

// 	/// <summary>
// 	/// 이미지가 점점 보임
// 	/// </summary>
// 	public static IEnumerator CoFadeIn(Graphic _graphic,float _duration,bool _ignoreTimescale)
// 	{
// 		yield return CoFade(_graphic,_duration,0.0f,1.0f,_ignoreTimescale);
// 	}

// 	/// <summary>
// 	/// 이미지가 점점 보임
// 	/// </summary>
// 	public static IEnumerator CoFadeIn(List<Graphic> _graphicList,float _duration,bool _ignoreTimescale)
// 	{
// 		yield return CoFade(_graphicList,_duration,0.0f,1.0f,_ignoreTimescale);
// 	}

// 	/// <summary>
// 	/// 이미지가 점점 사라짐
// 	/// </summary>
// 	public static IEnumerator CoFadeOut(Graphic _graphic,float _duration,bool _ignoreTimescale)
// 	{
// 		yield return CoFade(_graphic,_duration,1.0f,0.0f,_ignoreTimescale);
// 	}

// 	/// <summary>
// 	/// 이미지가 점점 사라짐
// 	/// </summary>
// 	public static IEnumerator CoFadeOut(List<Graphic> _graphicList,float _duration,bool _ignoreTimescale)
// 	{
// 		yield return CoFade(_graphicList,_duration,1.0f,0.0f,_ignoreTimescale);
// 	}

// 	private static IEnumerator CoFade(Graphic _graphic,float _duration,float _start,float _finish,bool _ignoreTimescale)
// 	{
// 		_graphic.color = _graphic.color.MaskAlpha(_start);

// 		yield return CoExecuteOverTime(_start,_finish,_duration,(progress)=>
// 		{
// 			_graphic.color = _graphic.color.MaskAlpha(progress);
// 		},_ignoreTimescale);

// 		_graphic.color = _graphic.color.MaskAlpha(_finish);
// 	}

// 	private static IEnumerator CoFade(List<Graphic> _graphicList,float _duration,float _start,float _finish,bool _ignoreTimescale)
// 	{
// 		foreach(var graphic in _graphicList)
// 		{
// 			graphic.color = graphic.color.MaskAlpha(_start);
// 		}

// 		yield return CoExecuteOverTime(_start,_finish,_duration,(progress)=>
// 		{
// 			foreach(var graphic in _graphicList)
// 			{
// 				graphic.color = graphic.color.MaskAlpha(progress);
// 			}
// 		},_ignoreTimescale);

// 		foreach(var graphic in _graphicList)
// 		{
// 			graphic.color = graphic.color.MaskAlpha(_finish);
// 		}
// 	}
// 	#endregion Fade
// }