// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/WaterGeometryShader" {
	Properties {
		_Color ("Color", Color) = (0, 0.32, 0.55, 0.5)
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Float) = 40.0

		_Value1( "Value 1", Float ) = 1
		_Value2( "Value 2", Float ) = 0.7
		_Value3( "Value 3", Float ) = 20
		_Smoothing( "Smoothing", Float ) = 1.0
	}
	SubShader {
		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		//Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
				#include "UnityCG.cginc"
				#include "Assets/Materials/SimplexNoise.cginc"
				#pragma vertex vert
				#pragma geometry geom
				#pragma fragment frag alpha:premul

				uniform float _Value1;
				uniform float _Value2;
				uniform float _Value3;
				uniform float _Smoothing;

				uniform float4 _LightColor0;

				uniform float4 _Color;
				uniform float4 _SpecColor;
				uniform float _Shininess;

				struct v2g {
					float4 pos : SV_POSITION;
					float3	norm : NORMAL;
					float2 uv : TEXCOORD0;
				};

				struct g2f {
					float4 pos : SV_POSITION;
					float3 norm : NORMAL;
					float2 uv : TEXCOORD0;
					float3 diffuseColor : TEXCOORD1;
					float3 specularColor : TEXCOORD2;
				};

				v2g vert(appdata_full v) {
					// Animate vertex based on noise
					v.vertex.y = _Value1 * snoise(v.vertex.xz + _Time * _Value2) + _Value3;
					v2g OUT;
					OUT.pos = v.vertex;
					OUT.norm = v.normal;
					OUT.uv = v.texcoord;
					return OUT;
				}

				[maxvertexcount(3)]
				void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
					// grab three vertices
					float3 v0 = IN[0].pos.xyz;
					float3 v1 = IN[1].pos.xyz;
					float3 v2 = IN[2].pos.xyz;

					float3 centerPos = (v0 + v1 + v2) / 3.0;

					v1.y -= (v1.y - v0.y) * _Smoothing;
					v2.y -= (v2.y - v0.y) * _Smoothing;

					// calculate surface normal
					float3 vn = normalize(cross(v1 - v0, v2 - v0));

					// calculate lightning
					float4x4 modelMatrix = unity_ObjectToWorld;
					float4x4 modelMatrixInverse = unity_WorldToObject;

					float3 normalDirection = normalize(mul(float4(vn, 0.0), modelMatrixInverse).xyz);
					float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, float4(centerPos, 0.0)).xyz);
					float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
					float attenuation = 1.0;

					float3 ambientLighting =
						UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

					float3 diffuseReflection =
						attenuation * _LightColor0.rgb * _Color.rgb
						* max(0.0, dot(normalDirection, lightDirection));

					float3 specularReflection;
					// make sure light is not below sea level
					if (dot(normalDirection, lightDirection) > 0.0) {
						specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
					}

					// prepare outputs for each vertex
					g2f OUT;

					OUT.pos = UnityObjectToClipPos(IN[0].pos);
					OUT.norm = vn;
					OUT.uv = IN[0].uv;
					OUT.diffuseColor = ambientLighting + diffuseReflection;
					OUT.specularColor = specularReflection;
					triStream.Append(OUT);

					OUT.pos = UnityObjectToClipPos(IN[1].pos);
					OUT.norm = vn;
					OUT.uv = IN[1].uv;
					OUT.diffuseColor = ambientLighting + diffuseReflection;
					OUT.specularColor = specularReflection;
					triStream.Append(OUT);

					OUT.pos = UnityObjectToClipPos(IN[2].pos);
					OUT.norm = vn;
					OUT.uv = IN[2].uv;
					OUT.diffuseColor = ambientLighting + diffuseReflection;
					OUT.specularColor = specularReflection;
					triStream.Append(OUT);
				}

				half4 frag(g2f IN) : COLOR {
					return float4(IN.specularColor + IN.diffuseColor, 1.0);
				}
			ENDCG
		}
	}
}