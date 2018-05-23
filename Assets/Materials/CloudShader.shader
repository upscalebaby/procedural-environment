// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CloudShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_Scale( "Scale", Float ) = 2
		_Bias( "Bias", Float ) = -1.5
		_Frequency( "Frequency", Float ) = 4
		_Speed( "Speed", Float ) = 0.03
		_WaveAmplitude( "WaveAmplitude", Float ) = 5
		_WaveFrequency( "WaveFrequency", Float ) = 30
	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		LOD 100
		Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Assets/Materials/SimplexNoise.cginc"

			uniform float _Scale;
			uniform float _Bias;
			uniform float _Frequency;
			uniform float _Speed;

			uniform float _WaveAmplitude;
			uniform float _WaveFrequency;

			uniform float _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				// Apply sine wave to vertex for wavey motion
				float2 pos = float2(v.vertex.x, v.vertex.z);
				v.vertex.y = _WaveAmplitude * sin(_Time * _WaveFrequency);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the the noise based on 3D vertex
				fixed4 col = float4(1, 1, 1, 0.25);
				float2 pos = i.uv;
				float3 pos3 = float3(i.uv.x, i.uv.y, _SinTime.x / 20);
				float noise = _Scale * snoise(float3(pos3 * _Frequency) + _Time * _Speed) + _Bias;
				col.a = (noise + 1) / 2;

				return col;
			}
			ENDCG
		}
	}
}
