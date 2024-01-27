using System;
using UnityEngine;
using System.Diagnostics;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace KZLib.KZAttribute
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,AllowMultiple = false,Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class KZIsValidAttribute : Attribute
	{
		public string CorrectText { get; private set; }
		public string WrongText { get; private set; }

		public KZIsValidAttribute(string _correctText = "O",string _wrongText = "X")
		{
			CorrectText = _correctText;
			WrongText = _wrongText;
		}
	}

#if UNITY_EDITOR
	public class KZIsValidAttributeDrawer : KZAttributeDrawer<KZIsValidAttribute,bool>
	{
		protected override void DoDrawPropertyLayout(GUIContent _label)
		{
			GUI.enabled = true;

			var isValid = ValueEntry.SmartValue;

			//? label
			var rect = DrawPrefixLabel(_label);
			//? 텍스트
			EditorGUI.LabelField(rect,isValid ? Attribute.CorrectText : Attribute.WrongText,GetValidateStyle(isValid));
		}
	}
#endif
}