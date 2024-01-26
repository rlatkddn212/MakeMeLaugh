using System;
using UnityEngine;
using Sirenix.Utilities;

#if UNITY_EDITOR

using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

#endif

namespace KZLib.KZAttribute
{
#if UNITY_EDITOR
	public abstract class KZAttributeDrawer<TAttribute,TValue> : OdinAttributeDrawer<TAttribute,TValue> where TAttribute : Attribute
	{
		protected string m_ErrorMessage = null;

		protected abstract void DoDrawPropertyLayout(GUIContent _label);

		protected override void DrawPropertyLayout(GUIContent _label)
		{
			//? 에러가 없는 경우 -> 제대로 출력
			if(m_ErrorMessage.IsEmpty())
			{
				DoDrawPropertyLayout(_label);

				return;
			}

			//? 에러가 있는 경우 -> 제대로 출력
			SirenixEditorGUI.ErrorMessageBox(m_ErrorMessage);

			CallNextDrawer(_label);
		}

		protected Rect DrawPrefixLabel(GUIContent _label,float? _height = null)
		{
			var rect = _height.HasValue ? EditorGUILayout.GetControlRect(true,_height.Value) : EditorGUILayout.GetControlRect();

			if(_label == null)
			{
				return rect;
			}

			EditorGUI.PrefixLabel(rect,_label);

			return rect.HorizontalPadding(EditorGUIUtility.labelWidth,0.0f);
		}

		protected TProperty GetValueInProperty<TProperty>(string _propertyName)
		{
			if(_propertyName.IsEmpty())
			{
				return default;
			}

			var propertyInfo = Property.ParentType.GetProperty(_propertyName,Flags.InstanceAnyVisibility);

			if(propertyInfo == null)
			{
				return default;
			}

			return (TProperty) propertyInfo.GetValue(Property.ParentValues[0],null);
		}

		protected GUIStyle GetValidateStyle(bool _isValid)
		{
			var style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = _isValid ? style.normal.textColor : Global.WRONG_HEX_COLOR.ToColor();

			return style;
		}

		protected void DrawColor(Rect _rect,Color _color)
		{
			var height = _rect.height;

			SirenixEditorGUI.DrawSolidRect(_rect.VerticalPadding(0.0f,height*0.3f),_color.MaskAlpha(1.0f));

			SirenixEditorGUI.DrawSolidRect(_rect.VerticalPadding(height*0.7f,0.0f),Color.white);
			SirenixEditorGUI.DrawSolidRect(_rect.VerticalPadding(height*0.7f,0.0f),new Color(0.0f,0.0f,0.0f,_color.a));
		}
	}
#endif
}