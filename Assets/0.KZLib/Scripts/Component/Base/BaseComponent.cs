using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR

using UnityEditor;

#endif

public abstract class BaseComponent : SerializedMonoBehaviour
{
	private const int GIZMOS_ORDER = 30;

	protected virtual bool UseGizmos => false;
	private bool IsExistText => UseGizmos && !GizmosText.IsEmpty();
	protected virtual string GizmosText => string.Empty;
	
	[FoldoutGroup("기즈모",Order = GIZMOS_ORDER)]
	[VerticalGroup("기즈모/0",Order = 0),SerializeField,LabelText("기즈모 오프 셋"),ShowIf("IsExistText")]
	protected Vector3 m_GizmosOffset = Vector3.zero;
	[VerticalGroup("기즈모/0",Order = 0),SerializeField,LabelText("기즈모 글씨 색상"),ShowIf("IsExistText")]
	protected Color m_GizmosTextColor = Color.white;

	protected virtual void Awake() { }

	protected virtual void OnDestroy() { }

	protected virtual void Reset() { }

#if UNITY_EDITOR
	protected virtual void DrawGizmo() { }
	private void OnDrawGizmos()
	{
		if(!UseGizmos)
		{
			return;
		}

		var style = new GUIStyle();
		style.normal.textColor = m_GizmosTextColor;

		var position = transform.position+m_GizmosOffset;

		Handles.Label(position,GizmosText,style);

		DrawGizmo();
	}
#endif
}