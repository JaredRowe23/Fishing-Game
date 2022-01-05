Shader "Custom/Testing/FreyaShader5"
{
    Properties
    {
        [NoScaleOffset] _HealthbarTexture ("Healthbar Texture", 2D) = "white" {}
		_Health ("Health", Range(0, 1)) = 1
		_StartColor ("Start Color", Color) = (1, 0, 0, 1)
		_EndColor ("End Color", Color) = (0, 1, 0, 1)
		_PulseBrightness ("Pulse Brightness", Range(0, 1)) = 0.5
		_PulseStart("Pulse Start", Range(0, 1)) = 0.2
		_PulseFrequency ("Pulse Frequency", Float) = 1
		_PulseAmp ("Pulse Amplitude", Range(0, 1)) = 0.1
		_BorderSize("Border Size", Range(0, 0.5)) = 0.1
		//_EdgeRoundSpacing("Edge Round Spacing", Range (0, 0.5)) = 0.1
		//_BorderThickness ("Border Thickness", Float2) = 0.1, 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
			
			sampler2D _HealthbarTexture;
			float _Health;
			float4 _StartColor;
			float4 _EndColor;
			float _PulseBrightness;
			float _PulseStart;
			float _PulseFrequency;
			float _PulseAmp;
			float _BorderSize;

			float InverseLerp(float a, float b, float v)
			{
				return (v-a)/(b-a);
			}

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
				// Rounded corner clipping
				float2 coords = i.uv;
				coords.x *= 8;
				float2 pointOnLineSeg = float2( clamp(coords.x, 0.5, 7.5), 0.5);
				float sdf = distance(coords, pointOnLineSeg) * 2 - 1;
				clip(-sdf);

				float borderSdf = sdf + _BorderSize;

				float pd = fwidth(borderSdf);
				float borderMask = 1 - saturate(borderSdf / pd);

				//float borderMask = step(0, -borderSdf);

				// Generate low hp pulse wave
				float isPulse = _Health <= _PulseStart;
				isPulse *= _Health > 0;
				float pulse = sin(_Time.y * _PulseFrequency) * 0.5 + 0.5;
				float pulseWave = pulse * _PulseAmp;

				// Create mask
				float barMask = i.uv.x + (isPulse * pulseWave) <= _Health;
				float4 bgColor = float4(barMask.xxx, 1);
				
				// Set up 20% - 80% color change range
				float t = saturate(InverseLerp(0.2, 0.8, _Health));

				// Texture usage
				float4 barTex = tex2D(_HealthbarTexture, float2(t, i.uv.y));
				barTex *= bgColor;
				
				// Custom color usage
				//float4 color = lerp(_StartColor, _EndColor, t);
				//color *= bgColor;

				// Composite
				float4 outColor = barTex + (barMask * pulse * isPulse * _PulseBrightness * float4(1, 1, 1, 1));
				outColor *= float4(borderMask.xxx, 1);

				return outColor;
            }
            ENDCG
        }
    }
}
