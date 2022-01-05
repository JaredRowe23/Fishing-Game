Shader "Custom/Testing/FreyaShader2"
{
    Properties
    {
		_ColorA ("Color A", Color) = (1, 1, 1, 1)
		_ColorB ("Color B", Color) = (1, 1, 1, 1)
		_ColorStart ("Color Start", Range(0, 1)) = 0
		_ColorEnd ("Color End", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { 
			"RenderType"="Transparent"
			"Queue"="Transparent" 
		}

        Pass
        {
		
			Cull Off
			ZWrite Off
			ZTest Always
			Blend One One // additive
			//Blend DstColor Zero // multiply

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#define TAU 6.28318530718

            #include "UnityCG.cginc"

			float4 _ColorA;
			float4 _ColorB;
			float _ColorStart;
			float _ColorEnd;

            struct MeshData
            {
                float4 vertex : POSITION;
				float3 normals : NORMAL;
                float2 uv0 : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
				float2 uv : TEXCOORD1;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normals);
				o.uv = v.uv0;
                return o;
            }

			float InverseLerp(float a, float b, float v)
			{
				return (v-a)/(b-a);
			}

            fixed4 frag (Interpolators i) : SV_Target
            {
				//float t = saturate(InverseLerp(_ColorStart, _ColorEnd, i.uv.x));

				float xOffset = cos(i.uv.x * TAU * 8) * 0.01;
				float t = cos((i.uv.y + xOffset - _Time.y * 0.1) * TAU * 5) * 0.5 + 0.5;
				t *= 1-i.uv.y;

				float topBottomRemover = (abs(i.normal.y) < 0.999);
				float waves = t * topBottomRemover;

				float4 gradient = lerp(_ColorA, _ColorB, i.uv.y);

				return gradient * waves;

				//float4 outColor = lerp(_ColorA, _ColorB, t);

				//return outColor;
            }
            ENDCG
        }
    }
}
