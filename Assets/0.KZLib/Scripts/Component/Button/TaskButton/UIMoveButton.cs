using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class UIMoveButton : UITaskButton
{
	protected override bool UseGizmos => true;

	[SerializeField]
	protected Transform m_Target = null;

	[VerticalGroup("기즈모/1",Order = 1),SerializeField,LabelText("기즈모 색상")]
	protected Color m_GizmosColor = Color.white;

	[SerializeField]
	private Vector3 m_Position = Vector3.zero;

	private Transform Target => m_Target != null ? m_Target : transform;

	protected override async UniTask DoClickedAsync()
	{
		var basePosition = Target.position;

		var position = basePosition+m_Position.MultiplyEach(Target.lossyScale);
		var duration = m_Duration/2.0f;

		await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,duration,(progress)=>
		{
			Target.position = Vector3.Lerp(basePosition,position,progress);
		},false,null,m_Source.Token);

		Target.position = position;

		await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,duration,(progress)=>
		{
			Target.position = Vector3.Lerp(position,basePosition,progress);
		},false,null,m_Source.Token);

		Target.position = basePosition;
	}

#if UNITY_EDITOR
	protected override void DrawGizmo()
	{
		base.DrawGizmo();

		if(Selection.activeObject == gameObject)
		{
			var color = Gizmos.color;
			var position = Target.position+m_Position.MultiplyEach(Target.lossyScale);

			Gizmos.color = m_GizmosColor;

			Gizmos.DrawLine(Target.position,position);

			Gizmos.color = color;
		}
	}

	protected override void Reset()
	{
		base.Reset();

		if(!m_Target)
		{
			m_Target = transform;
		}
	}
#endif
}