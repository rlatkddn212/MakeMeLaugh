using DG.Tweening;
using UnityEngine;

public class DrawLineRenderer : BaseLineRenderer
{
	private Tween m_Tween = null;

	private void OnDisable()
	{
		ResetRenderer();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		ResetRenderer();
	}

	private void ResetRenderer()
	{
		m_LineRenderer.positionCount = 0;

		m_Tween.Kill();
	}

	public void SetLineRenderer(Vector2? _width = null,Gradient _color = null)
	{
		if(_width != null)
		{
			m_LineRenderer.startWidth = _width.Value.x;
			m_LineRenderer.endWidth = _width.Value.y;
		}

		if(_color != null)
		{
			m_LineRenderer.colorGradient = _color;
		}
	}

	public Tween DrawLineTween(Vector3[] _pointArray,float _speed,bool _ignoreTimescale)
	{
		ResetRenderer();

		m_LineRenderer.positionCount = 1;
		m_LineRenderer.SetPosition(0,_pointArray[0]);

		var duration = Tools.GetTotalDistance(_pointArray)/_speed;

		m_Tween = TweenTools.SetProgress(0.0f,_pointArray.Length-1,duration,(progress)=>
		{
			var prev = Mathf.FloorToInt(progress);
			var next = Mathf.CeilToInt(progress);
			var time = progress-prev;

			if(m_LineRenderer.positionCount <= prev)
			{
				var start = m_LineRenderer.positionCount;
				m_LineRenderer.positionCount = prev+1;

				for(var i=start;i<m_LineRenderer.positionCount;i++)
				{
					m_LineRenderer.SetPosition(i,_pointArray[i]);
				}
			}
			else
			{
				var position = Vector3.Lerp(_pointArray[prev],_pointArray[next],time);

				m_LineRenderer.SetPosition(prev,position);
			}
		}).SetUpdate(_ignoreTimescale).SetEase(Ease.Linear);

		return m_Tween;
	}
}