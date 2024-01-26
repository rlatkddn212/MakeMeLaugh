using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BaseLineRenderer : BaseComponent
{
	[SerializeField]
	protected LineRenderer m_LineRenderer = null;

	protected override void Reset()
	{
		base.Reset();

		if(!m_LineRenderer)
		{
			m_LineRenderer = GetComponent<LineRenderer>();
		}
	}

	public void SetPointArray(Vector3[] _pointArray)
	{
		m_LineRenderer.positionCount = _pointArray.Length;
		m_LineRenderer.SetPositions(_pointArray);
	}

	public Mesh BakeMesh()
	{
		var mesh = new Mesh();

		m_LineRenderer.BakeMesh(mesh,true);

		return mesh;
	}
}