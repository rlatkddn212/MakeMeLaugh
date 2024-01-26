// 마스크 넣고 그대로 마스킹 해주는 쉐이더
Shader "KZLib/TextureMask"
{
	Properties
	{
		[Toggle] _Cutout("Is Cutout",Float) = 1.0
		[Toggle] _Reverse("Is Reverse",Float) = 0.0
		_MaskTex("Mask Texture",2D) = "white" {}

		[HideInInspector] _StencilComp("Stencil Comparison", Float) = 8.0
		[HideInInspector] _Stencil("Stencil ID", Float) = 0.0
		[HideInInspector] _StencilOp("Stencil Operation", Float) = 0.0
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255.0
		[HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255.0

		_Range("Range", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue"				= "Transparent"
			"IgnoreProjector"	= "True"
			"RenderType"		= "Transparent"
			"PreviewType"		= "Plane"
			"CanUseSpriteAtlas"	= "True"
		}

		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};

			v2f vert(appdata_t _data)
			{
				v2f result;

				result.worldPosition = _data.vertex;
				result.vertex = UnityObjectToClipPos(result.worldPosition);
				result.texcoord = _data.texcoord;
				result.color = _data.color;

				return result;
			}

			sampler2D _MainTex;
			sampler2D _MaskTex;
			float _Range;
			float _Cutout;
			float _Reverse;

			fixed4 frag(v2f _data) : SV_Target
			{
				fixed4 texColor = tex2D(_MainTex,_data.texcoord)*_data.color;
				float mask = tex2D(_MaskTex,_data.texcoord).a;
				float alpha = _Reverse ? 1.0-mask : mask;

				if(_Range == 1.0)
				{
					texColor.a = 0.0;
				}
				else
				{
					float pivot = _Cutout ? 0.0 : 0.1;

					texColor.a *= smoothstep(_Range-pivot,_Range+pivot,alpha);
				}

				return texColor;
			}
			ENDCG
		}
	}
}