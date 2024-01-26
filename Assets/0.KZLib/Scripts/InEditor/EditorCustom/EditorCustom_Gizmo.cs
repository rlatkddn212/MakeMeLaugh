#if UNITY_EDITOR
using System;
using KZLib.KZAttribute;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KZLib.KZEditor
{
	public partial class EditorCustom
	{
		[Serializable]
		private class GizmoData
		{
			[JsonProperty] public string HandleSelectHexColor { get; set; } = "#FFFF00FF";

			[JsonProperty] public string AnchorNormalHexColor { get; set; } = "#FF5F5FFF";
			[JsonProperty] public string AnchorHighlightHexColor { get; set; } = "#BC0A0AFF";

			[JsonProperty] public string ControlNormalHexColor { get; set; } = "#5999FFFF";
			[JsonProperty] public string ControlHighlightHexColor { get; set; } = "#3232C0FF";

			[JsonProperty] public string NormalLineHexColor { get; set; } = "#00FF00FF";
			[JsonProperty] public string GuideLineHexColor { get; set; } = "#FFFFFFFF";
		}

		[TitleGroup("기즈모 설정",BoldTitle = false,Order = 2)]
		[BoxGroup("기즈모 설정/핸들",Order = 0)]
		[VerticalGroup("기즈모 설정/핸들/0",Order = 0),LabelText("핸들 선택 색상"),ShowInInspector,KZHexColor]
		private string HandleSelectHexColor
		{
			get => Gizmo.HandleSelectHexColor;
			set
			{
				Gizmo.HandleSelectHexColor = value;

				SaveData(GIZMO_DATA,Gizmo);
			}
		}
		public static Color HandleSelectColor => Gizmo.HandleSelectHexColor.ToColor();

		[VerticalGroup("기즈모 설정/핸들/1",Order = 1),LabelText("앵커 일반 색상"),ShowInInspector,KZHexColor]
		private string AnchorNormalHexColor
		{
			get => Gizmo.AnchorNormalHexColor;
			set
			{

				Gizmo.AnchorNormalHexColor = value;

				SaveData(GIZMO_DATA,Gizmo);
			}
		}
		public static Color AnchorNormalColor => Gizmo.AnchorNormalHexColor.ToColor();


		[VerticalGroup("기즈모 설정/핸들/1",Order = 1),LabelText("앵커 강조 색상"),ShowInInspector,KZHexColor]
		private string AnchorHighlightHexColor
		{
			get => Gizmo.AnchorHighlightHexColor;
			set
			{
				Gizmo.AnchorHighlightHexColor = value;

				SaveData(GIZMO_DATA,Gizmo);
			}
		}
		public static Color AnchorHighlightColor => Gizmo.AnchorHighlightHexColor.ToColor();

		[VerticalGroup("기즈모 설정/핸들/2",Order = 2),LabelText("컨트롤 일반 색상"),ShowInInspector,KZHexColor]
		private string ControlNormalHexColor
		{
			get => Gizmo.ControlNormalHexColor;
			set
			{
				Gizmo.ControlNormalHexColor = value;

				SaveData(GIZMO_DATA,Gizmo);
			}
		}
		public static Color ControlNormalColor => Gizmo.ControlNormalHexColor.ToColor();

		[VerticalGroup("기즈모 설정/핸들/2",Order = 2),LabelText("컨트롤 강조 색상"),ShowInInspector,KZHexColor]
		private string ControlHighlightHexColor
		{
			get => Gizmo.ControlHighlightHexColor;
			set
			{
				Gizmo.ControlHighlightHexColor = value;

				SaveData(GIZMO_DATA,Gizmo);
			}
		}
		public static Color ControlHighlightColor => Gizmo.ControlHighlightHexColor.ToColor();

		[BoxGroup("기즈모 설정/라인",Order = 1)]
		[VerticalGroup("기즈모 설정/라인/0",Order = 0),LabelText("실선 라인 색상"),ShowInInspector,KZHexColor]
		private string NormalLineHexColor
		{
			get => Gizmo.NormalLineHexColor;
			set
			{
				Gizmo.NormalLineHexColor = value;

				SaveData(GIZMO_DATA,Gizmo);
			}
		}
		public static Color NormalLineColor => Gizmo.NormalLineHexColor.ToColor();

		[VerticalGroup("기즈모 설정/라인/0",Order = 0),LabelText("점선 라인 색상"),ShowInInspector,KZHexColor]
		private string GuideLineHexColor
		{
			get => Gizmo.GuideLineHexColor;
			set
			{
				Gizmo.GuideLineHexColor = value;

				SaveData(GIZMO_DATA,Gizmo);
			}
		}
		public static Color GuideLineColor => Gizmo.GuideLineHexColor.ToColor();
	}
}
#endif