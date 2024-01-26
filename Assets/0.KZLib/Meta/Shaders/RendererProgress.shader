Shader "KZLib/RendererProgress"
{
	Properties
	{
		_Color("Color", Color) = (1.0,1.0,1.0,1.0)
		_Progress("Progress",Range(0.0,1.0)) = 0.0

		[Toggle] _Horizontal("Is Horizontal",Float) = 1.0
		[Toggle] _Reverse("Is Reverse",Float) = 0.0

		[HideInInspector] _MainTex("Main Tex (RGBA)", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Overlay+1"
		}

		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				float4 _Color;
				float _Progress;
				bool _Horizontal;
				bool _Reverse;
				
				struct v2f
				{
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
				};
				
				v2f vert(appdata_base v)
				{
					v2f result;
					
					result.pos = UnityObjectToClipPos(v.vertex);
					result.uv = TRANSFORM_UV(0);

					return result;
				}

				half4 frag(v2f _index) : COLOR
				{
					half4 color = tex2D(_MainTex,_index.uv);

					if(_Horizontal)
					{
						color.a *= _Reverse ? _index.uv.x > 1.0-_Progress : _index.uv.x < _Progress;
					}
					else
					{
						color.a *= _Reverse ? _index.uv.y > 1.0-_Progress : _index.uv.y < _Progress;
					}

					return color * _Color;
				}
			ENDCG
		}
	}
}