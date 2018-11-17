Shader "User/RetroGridsTransparent" {
	Properties {
		_MainColor ("Main color", Color) = (0.1, 0.1, 0.1, 1)
		_LineColor ("Line color", Color) = (0.1, 1, 0.1, 1)
		_EmissionColor ("Emission color", Color) = (1, 1, 1, 1)
		_EmissionGain ("Emission gain", Range(0, 1)) = 0.5
        _GridTex("Grid texture", 2D) = "white" {}
		[Toggle] _UseSpecular ("Use Shininess", float) = 0
		_Specular ("Shininess", Range(1, 1000)) = 100
		_RimWidth("_RimWidth", Range(0.6,9.0)) = 0.9
		_ScrollXSpeed("X Scroll Speed", Range(0,10)) = 2
		_ScrollYSpeed("Y Scroll Speed", Range(0,10)) = 2
	}

	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector" = "True" }
		LOD 800
		ZWrite On
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade

		uniform float _UseSpecular;

		half4 LightingSimpleSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize (lightDir + viewDir);

			half diff = max (0, dot (s.Normal, lightDir));

			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular) * _UseSpecular;

			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
			c.a = s.Alpha;
			return c;
		}

		struct Input {
			float2 uv_GridTex;
			float3 viewDir;
		};

		sampler2D _GridTex;

		float4 _LineColor;
		float4 _MainColor;
		
        fixed _ScrollXSpeed;
        fixed _ScrollYSpeed;
		
		fixed4 _EmissionColor;
		float _EmissionGain;
		float _Specular;
		fixed _RimWidth;

		void surf(Input IN, inout SurfaceOutput o) {
			//移动
			fixed2 scrolledUV = IN.uv_GridTex;
			fixed xScrollValue = _ScrollXSpeed * _Time;
			fixed yScrollValue = _ScrollYSpeed * _Time;
			scrolledUV += fixed2(xScrollValue,yScrollValue);

			float4 t = tex2D(_GridTex, scrolledUV);
			fixed val = saturate(1 - (t.r + t.g + t.b));
			o.Albedo = lerp(_LineColor, _MainColor, val);
			o.Alpha = t.a;
			o.Specular = _Specular;


			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = t * exp(_EmissionGain * 5.0f) * _EmissionColor * pow (rim, _RimWidth);
		}
		ENDCG
	}

	Fallback "VertexLit"
}