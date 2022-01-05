Shader "Custom/Testing/FreyaShader4"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Pattern ("Pattern", 2D)= "white" {}
		_Rock ("Rock", 2D)= "white" {}
		_MipSampleLevel ("MIP", Float) = 0
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

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Pattern;

			sampler2D _Rock;

			float _MipSampleLevel;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
				float2 topDownProjection = i.worldPos.xz;
                float4 moss = tex2Dlod(_MainTex, float4(topDownProjection, _MipSampleLevel.xx));
				float4 rock = tex2D(_Rock, topDownProjection);
				float pattern = tex2D(_Pattern, i.uv).x;

				float4 finalColor = lerp (rock, moss, pattern);

                return finalColor;
            }
            ENDCG
        }
    }
}
