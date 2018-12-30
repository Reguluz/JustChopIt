Shader "Lightweight Render Pipeline/User/LWThemeGridMove"
{
    // Keep properties of StandardSpecular shader for upgrade reasons.
    Properties
    {
        
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Cutoff("AlphaCutout", Range(0.0, 1.0)) = 0.5
        [Toggle] _SampleGI("SampleGI", float) = 0.0
        _ScrollXSpeed("X Scroll Speed", Range(0,10)) = 2
		_ScrollYSpeed("Y Scroll Speed", Range(0,10)) = 2
		_LineColor ("Line color", Color) = (0.1, 1, 0.1, 1)
		//_EmissionColor ("Emission color", Color) = (1, 1, 1, 1)
		_EmissionGain ("Emission gain", Range(0, 1)) = 0.5
		[Toggle] _UseSpecular ("Use Shininess", float) = 0
		_Specular ("Shininess", Range(1, 1000)) = 100
		_RimWidth("_RimWidth", Range(0.6,9.0)) = 0.9
    


        _Shininess("Shininess", Range(0.01, 1.0)) = 0.5
        _GlossMapScale("Smoothness Factor", Range(0.0, 1.0)) = 1.0

        _Glossiness("Glossiness", Range(0.0, 1.0)) = 0.5
        [Enum(Specular Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [HideInInspector] _SpecSource("Specular Color Source", Float) = 0.0
        _SpecColor("Specular", Color) = (0.5, 0.5, 0.5)
        _SpecGlossMap("Specular", 2D) = "white" {}
        [HideInInspector] _GlossinessSource("Glossiness Source", Float) = 0.0
        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

        [HideInInspector] _BumpScale("Scale", Float) = 1.0
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}


        _EmissionMap("Emission", 2D) = "white" {}

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        [ToogleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "LightweightPipeline" "IgnoreProjector" = "True"}
        LOD 300

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "LightweightForward" }

            // Use same blending / depth states as Standard shader
            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _ _SPECGLOSSMAP _SPECULAR_COLOR
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA
            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _EMISSION
            #pragma shader_feature _RECEIVE_SHADOWS_OFF

            // -------------------------------------
            // Lightweight Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex LitPassVertexSimple
            #pragma fragment LitPassFragmentSimple
            #define BUMP_SCALE_NOT_SUPPORTED 1
            
            half _ScrollXSpeed;
            half _ScrollYSpeed;
            //half4 _EmissionColor;
            float _EmissionGain;
            half _RimWidth;
            sampler2D _GridTex;
            half4 _LineColor;

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitForwardPass.hlsl"

            
            /*Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv.xy = TRANSFORM_TEX(input.uv, _MainTex);
                output.uv.z = ComputeFogFactor(vertexInput.positionCS.z);

#if defined(_SAMPLE_GI) || defined(_SAMPLE_GI_NORMALMAP)
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.normal = normalInput.normalWS;
    #if defined(_SAMPLE_GI_NORMALMAP)
                output.tangent = normalInput.tangentWS;
                output.bitangent = normalInput.bitangentWS;
    #endif
                OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
                OUTPUT_SH(output.normal, output.vertexSH);
#endif
                return output;
            }*/
            
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
            
               
                
                half2 uv = input.uv;
                /*half xScrollValue = _ScrollXSpeed * _Time;
                half yScrollValue = _ScrollYSpeed * _Time;
                uv += half2(xScrollValue,yScrollValue);*/
               /* float4 t = tex2D(_GridTex, uv);
			    half val = saturate(1 - (t.r + t.g + t.b));*/
                
                
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                
                half4 diffuseAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_PARAM(_MainTex, sampler_MainTex));
                half3 diffuse = diffuseAlpha.rgb * _Color.rgb;
            
                half alpha = diffuseAlpha.a * _Color.a;
                AlphaDiscard(alpha, _Cutoff);
            #ifdef _ALPHAPREMULTIPLY_ON
                diffuse *= alpha;
            #endif
            
                half3 normalTS = SampleNormal(uv, TEXTURE2D_PARAM(_BumpMap, sampler_BumpMap));
                
                //VertexNormalInputs normalinput = GetVertexNormalInputs(input.normal);
                //half rim = 1.0 - saturate(dot (normalize(input.viewDir), normalinput.normalWS));
			    //half3 emission = texColor * exp(_EmissionGain * 5.0f) * _EmissionColor * pow (rim, _RimWidth);
			
                half3 emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_PARAM(_EmissionMap, sampler_EmissionMap));
                half4 specularGloss = SampleSpecularGloss(uv, diffuseAlpha.a, _SpecColor, TEXTURE2D_PARAM(_SpecGlossMap, sampler_SpecGlossMap));
                half shininess = input.posWSShininess.w;
            
                InputData inputData;
                InitializeInputData(input, normalTS, inputData);
            
                half4 color = LightweightFragmentBlinnPhong(inputData, diffuse, specularGloss, shininess, emission, alpha);
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                return color;
            };
            
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/ShadowCasterPass.hlsl"
            
           
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }

        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags{ "LightMode" = "Meta" }

            Cull Off

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex LightweightVertexMeta
            #pragma fragment LightweightFragmentMetaSimple

            #pragma shader_feature _EMISSION
            #pragma shader_feature _SPECGLOSSMAP

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitMetaPass.hlsl"

            ENDHLSL
        }
    }
    Fallback "Hidden/InternalErrorShader"
    //CustomEditor "UnityEditor.Experimental.Rendering.LightweightPipeline.SimpleLitShaderGUI"
}
