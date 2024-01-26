using System;
using UnityEngine;
using System.Diagnostics;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace KZLib.KZAttribute
{
	[AttributeUsage(AttributeTargets.All,AllowMultiple = false,Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class KZRichTextAttribute : Attribute { }

#if UNITY_EDITOR
	public class KZRichTextAttributeDrawer : KZAttributeDrawer<KZRichTextAttribute,string>
	{
		protected override void DoDrawPropertyLayout(GUIContent _label)
		{
			var cashed = GUI.enabled;

			GUI.enabled = true;

			//? label
			var rect = DrawPrefixLabel(_label);

			//? 텍스트
			EditorGUI.LabelField(rect,ValueEntry.SmartValue,new GUIStyle(GUI.skin.label)
			{
				richText = true,
			});

			GUI.enabled = cashed;
		}
	}
#endif
}