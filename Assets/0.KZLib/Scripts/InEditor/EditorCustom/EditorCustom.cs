	#if UNITY_EDITOR
	using System;
	using Newtonsoft.Json;
	using Sirenix.OdinInspector;
	using UnityEditor;

	namespace KZLib.KZEditor
	{
		[Serializable]
		public partial class EditorCustom
		{
			private const string HIERARCHY_DATA = "[EditorCustom] HierarchyData";
			private const string GIZMO_DATA = "[EditorCustom] GizmoData";

			private static HierarchyData s_Hierarchy = null;

			private static GizmoData s_Gizmo = null;

			private static HierarchyData Hierarchy => s_Hierarchy ??= LoadData<HierarchyData>(HIERARCHY_DATA);

			private static GizmoData Gizmo => s_Gizmo ??= LoadData<GizmoData>(GIZMO_DATA);

			[InitializeOnLoadMethod]
			private static void Initialize()
			{
				Tools.AddTag(Global.CATEGORY_TAG);

				SetHierarchy();
			}

			private void SaveData<TData>(string _key,TData _data)
			{
				EditorPrefs.SetString(_key,JsonConvert.SerializeObject(_data));
			}

			private static TData LoadData<TData>(string _key) where TData : new()
			{
				var text = EditorPrefs.GetString(_key,"");

				return text.IsEmpty() ? new TData() : JsonConvert.DeserializeObject<TData>(text);
			}

			public void SaveGizmo()
			{
				SaveData(GIZMO_DATA,Gizmo);
			}

			[FoldoutGroup("옵션 설정",Order = 0)]
			[VerticalGroup("옵션 설정/버튼",Order = 0),Button("에디터 커스텀 초기화",ButtonSizes.Large)]
			private void OnResetCustom()
			{
				if(Tools.DisplayCheck("에디터 커스텀 초기화","에디터 커스텀을 초기화 하시겠습니까?"))
				{
					EditorPrefs.DeleteKey(HIERARCHY_DATA);
					s_Hierarchy = null;

					EditorPrefs.DeleteKey(GIZMO_DATA);
					s_Gizmo = null;

					Tools.DisplayInfo("에디터 커스텀을 초기화 했습니다.");
				}
			}
		}
	}
	#endif