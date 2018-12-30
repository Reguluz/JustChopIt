Shader "User/GridsTransparentUnmove" {
	Properties {
		_MainColor ("Main color", Color) = (0.1, 0.1, 0.1, 1)
		_LineColor ("Line color", Color) = (0.1, 1, 0.1, 1)
		_EmissionColor ("Emission color", Color) = (1, 1, 1, 1)
		_EmissionGain ("Emission gain", Range(0, 1)) = 0.5
        _GridTex("Grid texture", 2D) = "white" {}
		_Specular ("Shininess", Range(1, 1000)) = 100
		_RimWidth("_RimWidth", Range(0.6,9.0)) = 0.9
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector" = "True" }
		LOD 200
		Blend One OneMinusSrcAlpha
		ZWrite Off
		CGPROGRAM
		#pragma surface surf Standard alpha:fade



		/*half4 LightingSimpleSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize (lightDir + viewDir);

			half diff = max (0, dot (s.Normal, lightDir));

			float nh = max (0, dot (s.Normal, h));

			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff) * atten;
			c.a = s.Alpha;
			return c;
		}*/

		struct Input {
			float2 uv_GridTex;
			float3 viewDir;
		};

		sampler2D _GridTex;

		float4 _LineColor;
		float4 _MainColor;
		
		
		fixed4 _EmissionColor;
		float _EmissionGain;
		float _Specular;
		fixed _RimWidth;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			//移动
			float4 t = tex2D(_GridTex, IN.uv_GridTex);
			fixed val = saturate(1 - (t.r + t.g + t.b));
			o.Albedo = _LineColor;//lerp(_LineColor, _MainColor, val);
			o.Alpha = t.a;


			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = t * _EmissionColor;
		}
		ENDCG
	}

	Fallback "Diffuse"
}