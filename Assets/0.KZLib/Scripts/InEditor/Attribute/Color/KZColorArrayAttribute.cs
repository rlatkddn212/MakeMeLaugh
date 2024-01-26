using System;
using UnityEngine;
using System.Diagnostics;
using Sirenix.Utilities;

#if UNITY_EDITOR

using Sirenix.Utilities.Editor;

#endif

namespace KZLib.KZAttribute
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,AllowMultiple = false,Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class KZColorArrayAttribute : Attribute { }

#if UNITY_EDITOR
	public class KZColorArrayAttributeDrawer : KZAttributeDrawer<KZColorArrayAttribute,Color[]>
	{
		protected override void DoDrawPropertyLayout(GUIContent _label)
		{
			//? label
			var rect = DrawPrefixLabel(_label);
			var colorArray = ValueEntry.SmartValue;

			if(colorArray.IsNullOrEmpty())
			{
				return;
			}
			
			//? 색상
			// 전체 = 현재 가로-(배열사이의 빈칸)
			var total = rect.width-(colorArray.Length-1);
			var width = Mathf.RoundToInt(total/colorArray.Length);
			var space = total-(width*colorArray.Length);

			DrawColor(new Rect(rect.x,rect.y,width+space,rect.height),colorArray[0]);

			for(var i=1;i<colorArray.Length;i++)
			{
				DrawColor(new Rect(rect.x+space+((width+1)*i),rect.y,width,rect.height),colorArray[i]);
			}
		}
	}
#endif
}