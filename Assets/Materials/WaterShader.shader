Shader "Custom/WaterShader" {
	Properties {
		_Value1( "Water Amplitude", Float ) = 5
		_Value2( "Water Frequency", Float ) = 0.2
		_Value3( "Water level", Float ) = 20
		_Value4( "Speed", Float ) = 2.5

		_Frequency( "Wave Frequency", Float ) = 50
		_Amplitude( "Wave Amplitude", Float ) = 1

		_Color ("Color", Color) = (0, 0.11, 0.44, 0.28)
		_Glossiness ("Smoothness", Range(0,1)) = 0.9
		_Metallic ("Metallic", Range(0,1)) = 0
	}

	SubShader {
		Cull Off

		Tags {
			"Queue"="Transparent"
			"RenderType" = "Transparent"
			}

		CGPROGRAM
			#include "Assets/Materials/SimplexNoise.cginc"
			#pragma surface surf Standard vertex:vert alpha:premul
			#pragma target 3.0

			struct Input {
				float4 vertex;
				float3 normal;
			};

			uniform float _Value1;
			uniform float _Value2;
			uniform float _Value3;
			uniform float _Value4;

			uniform float _Frequency;
			uniform float _Amplitude;


			void vert (inout appdata_full i, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input, o);

				// This should be cleaned to re-use duplicate vertices to avoid code redundancy
				// Essentially I'm reconstructing all neighboring vertices and calculating their normals
				float2 aPos = float2(i.vertex.x, i.vertex.z);
				float2 bPos = float2(i.vertex.x, i.vertex.z + 1);
				float2 cPos = float2(i.vertex.x + 1, i.vertex.z + 1);

				float2 aPos2 = float2(i.vertex.x, i.vertex.z);
				float2 bPos2 = float2(i.vertex.x + 1, i.vertex.z);
				float2 cPos2 = float2(i.vertex.x + 1, i.vertex.z + 1);

				float2 aPos3 = float2(i.vertex.x, i.vertex.z);
				float2 bPos3 = float2(i.vertex.x, i.vertex.z + 1);
				float2 cPos3 = float2(i.vertex.x - 1, i.vertex.z);

				float2 aPos4 = float2(i.vertex.x, i.vertex.z);
				float2 bPos4 = float2(i.vertex.x - 1, i.vertex.z);
				float2 cPos4 = float2(i.vertex.x - 1, i.vertex.z - 1);

				float2 aPos5 = float2(i.vertex.x, i.vertex.z);
				float2 bPos5 = float2(i.vertex.x - 1, i.vertex.z - 1);
				float2 cPos5 = float2(i.vertex.x, i.vertex.z - 1);

				float2 aPos6 = float2(i.vertex.x, i.vertex.z);
				float2 bPos6 = float2(i.vertex.x, i.vertex.z - 1);
				float2 cPos6 = float2(i.vertex.x + 1, i.vertex.z);

				float time = _Time;

				float noiseValueA = _Value1 * snoise(float2(aPos * _Value2) + time * _Value4) + _Value3;
				float noiseValueB = _Value1 * snoise(float2(bPos * _Value2) + time * _Value4) + _Value3;
				float noiseValueC = _Value1 * snoise(float2(cPos * _Value2) + time * _Value4) + _Value3;

				float noiseValueA2 = _Value1 * snoise(float2(aPos2 * _Value2) + time * _Value4) + _Value3;
				float noiseValueB2 = _Value1 * snoise(float2(bPos2 * _Value2) + time * _Value4) + _Value3;
				float noiseValueC2 = _Value1 * snoise(float2(cPos2 * _Value2) + time * _Value4) + _Value3;

				float noiseValueA3 = _Value1 * snoise(float2(aPos3 * _Value2) + time * _Value4) + _Value3;
				float noiseValueB3 = _Value1 * snoise(float2(bPos3 * _Value2) + time * _Value4) + _Value3;
				float noiseValueC3 = _Value1 * snoise(float2(cPos3 * _Value2) + time * _Value4) + _Value3;

				float noiseValueA4 = _Value1 * snoise(float2(aPos4 * _Value2) + time * _Value4) + _Value3;
				float noiseValueB4 = _Value1 * snoise(float2(bPos4 * _Value2) + time * _Value4) + _Value3;
				float noiseValueC4 = _Value1 * snoise(float2(cPos4 * _Value2) + time * _Value4) + _Value3;

				float noiseValueA5 = _Value1 * snoise(float2(aPos5 * _Value2) + time * _Value4) + _Value3;
				float noiseValueB5 = _Value1 * snoise(float2(bPos5 * _Value2) + time * _Value4) + _Value3;
				float noiseValueC5 = _Value1 * snoise(float2(cPos5 * _Value2) + time * _Value4) + _Value3;

				float noiseValueA6 = _Value1 * snoise(float2(aPos6 * _Value2) + time * _Value4) + _Value3;
				float noiseValueB6 = _Value1 * snoise(float2(bPos6 * _Value2) + time * _Value4) + _Value3;
				float noiseValueC6 = _Value1 * snoise(float2(cPos * _Value2) + time * _Value4) + _Value3;

				float3 b = float3(bPos.x, noiseValueB, bPos.y);
				float3 c = float3(cPos.x, noiseValueC, cPos.y);
				float3 sideAB = b - i.vertex;
				float3 sideAC = c - i.vertex;
				float3 surfaceNormal = normalize(cross(sideAB, sideAC));

				float3 b2 = float3(bPos2.x, noiseValueB2, bPos2.y);
				float3 c2 = float3(cPos2.x, noiseValueC2, cPos2.y);
				float3 sideAB2 = b2 - i.vertex;
				float3 sideAC2 = c2 - i.vertex;
				float3 surfaceNormal2 = normalize(cross(sideAB2, sideAC2));

				float3 b3 = float3(bPos3.x, noiseValueB3, bPos3.y);
				float3 c3 = float3(cPos3.x, noiseValueC3, cPos3.y);
				float3 sideAB3 = b3 - i.vertex;
				float3 sideAC3 = c3 - i.vertex;
				float3 surfaceNormal3 = normalize(cross(sideAB3, sideAC3));

				float3 b4 = float3(bPos4.x, noiseValueB4, bPos4.y);
				float3 c4 = float3(cPos4.x, noiseValueC4, cPos4.y);
				float3 sideAB4 = b4 - i.vertex;
				float3 sideAC4 = c4 - i.vertex;
				float3 surfaceNormal4 = normalize(cross(sideAB4, sideAC4));

				float3 b5 = float3(bPos5.x, noiseValueB5, bPos5.y);
				float3 c5 = float3(cPos5.x, noiseValueC5, cPos5.y);
				float3 sideAB5 = b5 - i.vertex;
				float3 sideAC5 = c5 - i.vertex;
				float3 surfaceNormal5 = normalize(cross(sideAB5, sideAC5));

				float3 b6 = float3(bPos6.x, noiseValueB6, bPos6.y);
				float3 c6 = float3(cPos6.x, noiseValueC6, cPos6.y);
				float3 sideAB6 = b6 - i.vertex;
				float3 sideAC6 = c6 - i.vertex;
				float3 surfaceNormal6 = normalize(cross(sideAB6, sideAC6));

				// Prepare output
				o.normal = normalize(i.normal + abs(surfaceNormal + surfaceNormal2 + surfaceNormal3 + surfaceNormal4 + surfaceNormal5 + surfaceNormal6));
				i.vertex.y = noiseValueA;
				i.vertex.y += sin(_Time * _Frequency) * _Amplitude;
				o.vertex = i.vertex;

			}

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			void surf (Input IN, inout SurfaceOutputStandard o) {
				o.Albedo = _Color.rgb;
				o.Alpha = _Color.a;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Normal = IN.normal;
			}

		ENDCG
	} 
	Fallback "Diffuse"
}