#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using System.Collections.Generic;

namespace KZLib.KZWindow
{
	public partial class ManualWindow : OdinMenuEditorWindow
	{
		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree
			{
				// { "이징 정보",m_easingWindow },
			};

			tree.Config.DrawSearchToolbar = true;
			tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
			tree.Selection.SupportsMultiSelect = false;

			return tree;
		}
	}
}
#endif