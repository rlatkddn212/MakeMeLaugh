#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace KZLib.KZWindow
{
	public class RunInEditWindow : OdinEditorWindow
	{
		[SerializeField,HideInInspector]
		private bool m_NowUpdating = false;

		private bool Updating => m_NowUpdating;
		private bool IsPlayable => !m_NowUpdating && !m_RunningObjectList.IsNullOrEmpty();

		[BoxGroup("2",ShowLabel = false,Order = 2)]
		[HorizontalGroup("2/1"),ShowInInspector,LabelText(" "),ListDrawerSettings(ShowFoldout = false),OnValueChanged("OnCheckList"),Searchable]
		private readonly List<MonoBehaviour> m_RunningObjectList = new();

		[BoxGroup("3",ShowLabel = false,Order = 1)]
		[HorizontalGroup("3/1"),Button("시작 하기",ButtonSizes.Large),EnableIf("IsPlayable")]
		private void OnRunObject()
		{
			m_NowUpdating = true;
			EditorApplication.update += EditorUpdate;
		}

		[HorizontalGroup("3/1"),Button("그만 하기",ButtonSizes.Large),EnableIf("Updating")]
		private void OnStopObject()
		{
			m_NowUpdating = false;
			EditorApplication.update -= EditorUpdate;
		}

		static void EditorUpdate()
		{
			EditorApplication.QueuePlayerLoopUpdate();
		}

		private void OnCheckList()
		{
			var removeList = new List<MonoBehaviour>();

			foreach(var remove in m_RunningObjectList)
			{
				if(remove)
				{
					continue;
				}

				removeList.Add(remove);
			}

			foreach(var remove in removeList)
			{
				m_RunningObjectList.Remove(remove);
			}
		}
	}
}
#endif