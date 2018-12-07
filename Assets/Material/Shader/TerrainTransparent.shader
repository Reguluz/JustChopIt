// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "User/TerrainTransparent"
{
    Properties
    {
        _Open("IsOpen",Float) = 0.0
        _Center("OwnerPos",Vector) = (0,0,0,0)
        _Radius("Radius",Float) = 5
        _Amplitude("_Amplitude",Range(-5,5))=1
        _Offset("Offset",Range(-5,5))=0
        [PowerSlider(3)]_Frequency("_Frequency",Range(0,2))=0.25

        _Color ("Color", Color) = (1,1,1,1)
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Normalmap("Normal Map",2D) = "white"{}

        _Emissionmap ("Emission Map", 2D) = "white" {}
        _EmissionColor("Emission Color", Color) = (1,1,1,1)
        _EmissionLevel("EmissionLevel", Range(0,1)) = 0.5

        _MainTex("Tex",2D) = "white"{}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector" = "True"}
        LOD 200
            Blend One OneMinusSrcAlpha
            // 根据_ZWrite参数，设置深度写入模式开关与否
            ZWrite Off


        CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma surface surf Standard alpha:premul vertex:vert
            //#pragma shader_feature _ _ALPHATEST_OFF _ALPHABLEND_OFF _ALPHAPREMULTIPLY_ON
            #pragma target 3.0
            //#include "UnityCG.cginc"

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            
            
            float _Open;
            float4 _Center;
            float _Radius;
            float _Amplitude;
            float _Frequency;
            float _Offset;

            fixed4 _Color;
            float _Metallic;
            float _Glossiness;
            sampler2D _Normalmap;

            sampler2D _Emissionmap;
            fixed4 _EmissionColor;
            float _EmissionLevel;

            sampler2D _MainTex;
            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void vert(inout appdata_full v){
                if(_Open == 1){
                    float4 worldSpaceVertex = mul(unity_ObjectToWorld,(v.vertex));
                    float d = distance(_Center,worldSpaceVertex);
                    float waveValueA = cos(d*_Frequency)*_Amplitude+_Offset;
                    if(d<_Radius){
                        v.vertex.xyz = float3(v.vertex.x,v.vertex.y + waveValueA,v.vertex.z);
                        v.normal = normalize(float3(v.normal.x + waveValueA,v.normal.y,v.normal.z));
                    }   
                }
            }

            
            void surf (Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                o.Alpha = c.a*_Color;
                o.Normal = UnpackNormal(tex2D (_Normalmap, IN.uv_MainTex));
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Emission = tex2D (_Emissionmap, IN.uv_MainTex) * _EmissionColor * _EmissionLevel;
            }
            ENDCG
    }
    FallBack "Diffuse"
}
