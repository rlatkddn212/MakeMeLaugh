using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TransformTask : TweenTask
{
	[Flags]
	private enum ModeCategory { None = 0, Translate = 1<<0, Rotate = 1<<1, Scale = 1<<2, All = -1 }

	[SerializeField,LabelText("현재 모드")]
	private ModeCategory m_Mode = ModeCategory.None;

	private bool IsTranslate => m_Mode.HasFlag(ModeCategory.Translate);
	private bool IsRotate => m_Mode.HasFlag(ModeCategory.Rotate);
	private bool IsScale => m_Mode.HasFlag(ModeCategory.Scale);

	[SerializeField,LabelText("진행 시간")]
	private float m_Duration = 0.0f;

	[SerializeField,LabelText("이동 값 X"),ShowIf(nameof(IsTranslate))]
	private AnimationCurve m_TranslateCurveX = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);
	[SerializeField,LabelText("이동 값 Y"),ShowIf(nameof(IsTranslate))]
	private AnimationCurve m_TranslateCurveY = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);
	[SerializeField,LabelText("이동 값 Z"),ShowIf(nameof(IsTranslate))]
	private AnimationCurve m_TranslateCurveZ = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);

	[SerializeField,LabelText("회전 값 X"),ShowIf(nameof(IsRotate))]
	private AnimationCurve m_RotateCurveX = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);
	[SerializeField,LabelText("회전 값 Y"),ShowIf(nameof(IsRotate))]
	private AnimationCurve m_RotateCurveY = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);
	[SerializeField,LabelText("회전 값 Z"),ShowIf(nameof(IsRotate))]
	private AnimationCurve m_RotateCurveZ = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);

	[SerializeField,LabelText("크기 값 X"),ShowIf(nameof(IsScale))]
	private AnimationCurve m_ScaleCurveX = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);
	[SerializeField,LabelText("크기 값 Y"),ShowIf(nameof(IsScale))]
	private AnimationCurve m_ScaleCurveY = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);
	[SerializeField,LabelText("크기 값 Z"),ShowIf(nameof(IsScale))]
	private AnimationCurve m_ScaleCurveZ = AnimationCurve.Linear(0.0f,0.0f,1.0f,0.0f);

	private Vector3 m_BasePosition = Vector3.zero;
	private Vector3 m_BaseRotation = Vector3.zero;
	private Vector3 m_BaseScale = Vector3.zero;

	protected override void InitializeTask()
	{
		base.InitializeTask();

		m_BasePosition = transform.position;
		m_BaseRotation = transform.rotation.eulerAngles;
		m_BaseScale = transform.localScale;
	}

	protected override async UniTask GetTaskAsync()
	{
		await UniTaskTools.ExecuteOverTimeAsync(0.0f,1.0f,m_Duration,(progress)=>
		{
			if(IsTranslate)
			{
				var translateX = m_TranslateCurveX.Evaluate(progress);
				var translateY = m_TranslateCurveY.Evaluate(progress);
				var translateZ = m_TranslateCurveZ.Evaluate(progress);

				transform.position = new Vector3(m_BasePosition.x+translateX,m_BasePosition.y+translateY,m_BasePosition.z+translateZ);
			}

			if(IsRotate)
			{
				var rotateX = m_RotateCurveX.Evaluate(progress);
				var rotateY = m_RotateCurveY.Evaluate(progress);
				var rotateZ = m_RotateCurveZ.Evaluate(progress);

				transform.rotation = Quaternion.Euler(m_BaseRotation.x+rotateX,m_BaseRotation.y+rotateY,m_BaseRotation.z+rotateZ);
			}

			if(IsScale)
			{
				var scaleX = m_ScaleCurveX.Evaluate(progress);
				var scaleY = m_ScaleCurveY.Evaluate(progress);
				var scaleZ = m_ScaleCurveZ.Evaluate(progress);

				transform.localScale = new Vector3(m_BaseScale.x+scaleX,m_BaseScale.y+scaleY,m_BaseScale.z+scaleZ);
			}
		},m_IgnoreTimeScale,null,m_Source.Token);
	}
}