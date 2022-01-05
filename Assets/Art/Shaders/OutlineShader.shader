Shader "Custom/OutlineShader"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
		_OutlineThickness ("OutlineThickness", Range(0, 1)) = 0.1
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
				float3 normals : NORMAL;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;
			float _OutlineThickness;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normals);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);

				float3 camPos = _WorldSpaceCameraPos;

				float3 fragToCam = camPos - i.worldPos;

				float3 viewDir = normalize(fragToCam);

				float edge = dot(viewDir, i.normal);
				edge = step(_OutlineThickness, edge);
				return float4(edge.xxx, 1);
            }
            ENDCG
        }
    }
}
