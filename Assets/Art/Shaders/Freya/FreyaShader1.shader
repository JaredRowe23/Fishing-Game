Shader "Custom/Testing/FreyaShader"
{
    Properties
    {
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Gloss ("_Gloss", Float) = 1
		_ShorelineTex ("Shoreline", 2D) = "black" {}
		_WaterShallow ("WaterShallow", Color) = (0, 0, 1, 1)
		_WaterDeep ("WaterDeep", Color) = (0.5, 0.5, 1, 1)
		_WaveColor ("WaveColor", Color) = (0.5, 0.5, 1, 1)
    }
    SubShader
    {
		Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv0 : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 clipSpacePos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOOR2;
            };

			sampler2D _ShorelineTex;

			float4 _Color;
			float _Gloss;
			uniform float3 _MousePos;
			float3 _WaterDeep;
			float3 _WaterShallow;
			float3 _WaveColor;

            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
				o.uv0 = v.uv0;
				o.normal = v.normal;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.clipSpacePos = UnityObjectToClipPos(v.vertex);
                return o;
            }

			float3 InvLerp(float3 a, float3 b, float value)
			{
				return (value - a) / (b - a);
			}

			float3 MyLerp(float3 a, float3 b, float t)
			{
				return t * b + (1 - t) * a;
			}

			float Posterize(float steps, float value)
			{
				return floor(value * steps) / steps;
			}

            fixed4 frag (VertexOutput o) : SV_Target
            {
				float shoreline = tex2D(_ShorelineTex, o.uv0).x;

				float waveSize = 0.04;

				float shape = shoreline;

				float waveAmp = (sin(shape / waveSize + _Time.y * 4) + 1) * 0.5;
				waveAmp *= shoreline;

				float3 waterColor = lerp(_WaterDeep, _WaterShallow, shoreline);

				float3 waterWithWaves = (lerp(waterColor, _WaveColor, waveAmp));

				return float4(waterWithWaves, 0);

				return waveAmp;

				float waveValues = (sin(shape / waveSize + _Time.y * 4) + 1) * 0.5;

				return waveValues;

				return frac(_Time.y);

				return shoreline;

				float dist = distance(_MousePos, o.worldPos);

				float glow = saturate(1-dist);

				float2 uv = o.uv0;

				float3 colorA = float3(0.1, 0.8, 1);
				float3 colorB = float3(1, 0.1, 0.8);
				float t = uv.y;

				t = Posterize(8, t);

				float3 blend = MyLerp(colorA, colorB, t);

				//return float4(blend.xyz, 0);

				float3 normal = normalize(o.normal);

				// Lighting
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 lightColor = _LightColor0.rgb;

				// Direct Light
				float lightFalloff = max(0, dot(lightDir, normal));
				lightFalloff = Posterize(3, lightFalloff);
				float3 directDiffuseLight = lightColor * lightFalloff;

				// Ambient Light
				float3 ambientLight = float3(0.1, 0.1, 0.1);

				// Direct Specular Light
				float3 camPos = _WorldSpaceCameraPos;
				float3 fragToCam = camPos - o.worldPos;
				float3 viewDir = normalize(fragToCam);
				float3 viewReflect = reflect (-viewDir, normal);
				float3 specularFalloff = max(0, dot(viewReflect, lightDir));
				specularFalloff = pow(specularFalloff, _Gloss);
				specularFalloff = Posterize(3, specularFalloff);
				float3 directSpecular = specularFalloff * lightColor;

				// Composite
				float3 diffuseLight = ambientLight + directDiffuseLight;
				float3 finalSurfaceColor = diffuseLight * _Color.rgb + directSpecular + glow;

				return float4(finalSurfaceColor, 0);
            }
            ENDCG
        }
    }
}
