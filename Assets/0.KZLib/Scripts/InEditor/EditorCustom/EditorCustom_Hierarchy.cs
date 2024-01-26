#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using KZLib.KZAttribute;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KZLib.KZEditor
{
	public partial class EditorCustom
	{
		[Serializable]
		private class HierarchyData
		{
			[JsonProperty] public bool UseHierarchy { get; set; } = false;

			[JsonProperty] public bool UseBranchTree { get; set; } = false;
			[JsonProperty] public string BranchTreeHexColor { get; set; } = "#FF61C2FF";

			[JsonProperty] public bool UseCategoryLine { get; set; } = false;
			[JsonProperty] public string CategoryHexColor { get; set; } = "#877FE9FF";
		}

		[TitleGroup("하이라키 설정",BoldTitle = false,Order = 1)]
		[VerticalGroup("하이라키 설정/사용",Order = 0),LabelText("하이라키 사용"),ToggleLeft,ShowInInspector]
		private bool UseHierarchy
		{
			get => Hierarchy.UseHierarchy;
			set
			{
				Hierarchy.UseHierarchy = value;

				SaveData(HIERARCHY_DATA,Hierarchy);
			}
		}

		[BoxGroup("하이라키 설정/데이터",Order = 1,ShowLabel = false)]
		[HorizontalGroup("하이라키 설정/데이터/0"),LabelText("브랜치 트리 사용"),ToggleLeft,ShowInInspector,ShowIf("UseHierarchy")]
		private bool UseBranchTree
		{
			get => Hierarchy.UseBranchTree && UseHierarchy;
			set
			{
				Hierarchy.UseBranchTree = value;

				SaveData(HIERARCHY_DATA,Hierarchy);
			}
		}

		[HorizontalGroup("하이라키 설정/데이터/0"),HideLabel,KZHexColor,ShowInInspector,ShowIf("UseBranchTree")]
		private string BranchTreeHexColor
		{
			get => Hierarchy.BranchTreeHexColor;
			set
			{
				Hierarchy.BranchTreeHexColor = value;

				SaveData(HIERARCHY_DATA,Hierarchy);
			}
		}
		private static Color BranchTreeColor => Hierarchy.BranchTreeHexColor.ToColor();

		[HorizontalGroup("하이라키 설정/데이터/1"),LabelText("카테고리 라인 사용"),ToggleLeft,ShowInInspector,ShowIf("UseHierarchy")]
		private bool UseCategoryLine
		{
			get => Hierarchy.UseCategoryLine && UseHierarchy;
			set
			{
				Hierarchy.UseCategoryLine = value;

				SaveData(HIERARCHY_DATA,Hierarchy);
			}
		}

		[HorizontalGroup("하이라키 설정/데이터/1"),HideLabel,KZHexColor,ShowInInspector,ShowIf("UseCategoryLine")]
		private string CategoryHexColor
		{
			get => Hierarchy.CategoryHexColor;
			set
			{
				Hierarchy.CategoryHexColor = value;

				SaveData(HIERARCHY_DATA,Hierarchy);
			}
		}
		private static Color CategoryColor => Hierarchy.CategoryHexColor.ToColor();

		private static readonly Dictionary<int,InstanceData> s_InstanceDataDict = new();

		private static void SetHierarchy()
		{
			if(!Hierarchy.UseHierarchy)
			{
				return;
			}

			// 중복 세팅 제거 용
			EditorApplication.hierarchyWindowItemOnGUI -= OnDrawHierarchyGUI;
			EditorApplication.hierarchyChanged -= OnUpdateFromScene;

			EditorApplication.hierarchyWindowItemOnGUI += OnDrawHierarchyGUI;
			EditorApplication.hierarchyChanged += OnUpdateFromScene;

			OnUpdateFromScene();

			EditorApplication.RepaintHierarchyWindow();
		}

		private static void OnDrawHierarchyGUI(int _instanceID,Rect _rect)
		{
			if(!s_InstanceDataDict.ContainsKey(_instanceID))
			{
				return;
			}

			var data = s_InstanceDataDict[_instanceID];
			var category = Hierarchy.UseCategoryLine && data.IsCategory;

			if(category)
			{
				var rect = new Rect(37.0f,_rect.y,_rect.width+25.0f+data.TreeLevel*14.0f,_rect.height);
				var current = EditorUtility.InstanceIDToObject(_instanceID) as GameObject;
				var color = Selection.activeGameObject == current ? CategoryColor.InvertColor() : CategoryColor;

				EditorGUI.DrawRect(rect,color);
				EditorGUI.LabelField(rect,current.name,new GUIStyle()
				{
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter,
					normal = new GUIStyleState() { textColor = color.grayscale > 0.5f ? Color.black : Color.white },
				});
			}

			if(Hierarchy.UseBranchTree && data.TreeLevel >= 0)
			{
				if(data.TreeLevel < 0 || _rect.x < 60 || category)
				{
					return;
				}

				if(data.IsLast)
				{
					DrawHVHLine(_rect,data.TreeLevel,data.HasChild);
				}
				else
				{
					DrawVHLine(_rect,data.TreeLevel,data.HasChild);
				}
			}
		}

		private static void OnUpdateFromScene()
		{
			s_InstanceDataDict.Clear();

			var prefab = PrefabStageUtility.GetCurrentPrefabStage();

			if(prefab)
			{
				var root = prefab.prefabContentsRoot;
				var level = root.transform.parent ? 1 : 0;

				AnalyzeGameObject(root,level,0,true);

				return;
			}

			for(var i=0;i<SceneManager.sceneCount;i++)
			{
				var scene = SceneManager.GetSceneAt(i);

				if(!scene.isLoaded)
				{
					continue;
				}

				var objectArray = scene.GetRootGameObjects();

				for(var j=0;j<objectArray.Length;j++)
				{
					AnalyzeGameObject(objectArray[j],0,j,j == objectArray.Length-1);
				}
			}
		}

		private static void AnalyzeGameObject(GameObject _object,int _treeLevel,int _treeGroup,bool _isLastChild)
		{
			var instanceId = _object.GetInstanceID();

			if(s_InstanceDataDict.ContainsKey(instanceId))
			{
				return;
			}

			var childCount = _object.transform.childCount;

			s_InstanceDataDict.Add(instanceId,new InstanceData(_treeLevel,_treeGroup,childCount > 0,_isLastChild,_object.CompareTag(Global.CATEGORY_TAG)));

			for(var i=0;i<childCount;i++)
			{
				AnalyzeGameObject(_object.transform.GetChild(i).gameObject,_treeLevel+1,_treeGroup,i == childCount-1);
			}
		}
		
		/// <summary>
		/// |
		/// |----
		/// |
		/// </summary>
		//! Vertical-Horizontal
		private static void DrawVHLine(Rect _rect,int _treeLevel,bool _hasChild)
		{
			DrawHalfVerticalLine(_rect,true,_treeLevel);
			DrawHorizontalLine(_rect,_treeLevel,_hasChild);
			DrawHalfVerticalLine(_rect,false,_treeLevel);
		}

		/// <summary>
		/// |
		/// |----
		/// </summary>
		//! Half-Vertical-Horizontal
		private static void DrawHVHLine(Rect _rect,int _treeLevel,bool _hasChild)
		{
			DrawHalfVerticalLine(_rect,true,_treeLevel);
			DrawHorizontalLine(_rect,_treeLevel,_hasChild);
		}

		/// <summary>
		/// |
		/// </summary>
		//! Half-Vertical
		private static void DrawHalfVerticalLine(Rect _rect,bool _isUp,int _treeLevel)
		{
			var height = _rect.height/2.0f;
			var x = GetTreeStartX(_rect,_treeLevel);
			var y = _isUp ? _rect.y : (_rect.y+height);

			EditorGUI.DrawRect(new Rect(x,y,2.0f,height),BranchTreeColor);
		}

		/// <summary>
		/// ----
		/// </summary>
		//! Horizontal
		private static void DrawHorizontalLine(Rect _rect,int _treeLevel,bool _hasChild)
		{
			var x = GetTreeStartX(_rect,_treeLevel);
			var y = _rect.y + _rect.height/2.0f;
			var width = _rect.height + (_hasChild ? -5.0f :  2.0f);

			EditorGUI.DrawRect(new Rect(x,y,width,2.0f),BranchTreeColor);
		}

		private static float GetTreeStartX(Rect _rect,int _treeLevel)
		{
			return 37.0f+(_rect.height-2.0f)*_treeLevel;
		}

		private record InstanceData(int TreeLevel,int TreeGroup,bool HasChild,bool IsLast,bool IsCategory);
	}
}
#endif