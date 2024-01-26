using System;
using UnityEngine;
using System.Diagnostics;
using Sirenix.Utilities;

#if UNITY_EDITOR

using UnityEditor;
using Sirenix.Utilities.Editor;

#endif

namespace KZLib.KZAttribute
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,AllowMultiple = false,Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class KZPercentAttribute : Attribute { }

#if UNITY_EDITOR
	public class KZPercentAttributeDrawer : KZAttributeDrawer<KZPercentAttribute,float>
	{
		protected override void DoDrawPropertyLayout(GUIContent _label)
		{
			//? label
			var rect = DrawPrefixLabel(_label);

			var percent = ValueEntry.SmartValue;

			//? 텍스트
			EditorGUI.LabelField(rect,percent.ToString());
			
			//? 색상
			var width = rect.width;

			SirenixEditorGUI.DrawSolidRect(rect.HorizontalPadding(0.0f,width*percent),Color.blue);
			SirenixEditorGUI.DrawSolidRect(rect.HorizontalPadding(width*(1.0f-percent),0.0f),new Color(0.5f,0.5f,0.5f,1.0f));
		}
	}
#endif
}