Shader "Lightweight Render Pipeline/User/LWTerrain"
{
    // Keep properties of StandardSpecular shader for upgrade reasons.
    Properties
    {
        
        _Open("IsOpen",Float) = 0.0
        _Center("OwnerPos",Vector) = (0,0,0,0)
        _Radius("Radius",Float) = 5
        _Amplitude("_Amplitude",Range(-10,5))=-5
        _Offset("Offset",Range(-10,10))=0
        [PowerSlider(3)]_Frequency("_Frequency",Range(0,2))=0.25
        [PowerSlider(5)]_Ratio("Ratio",Range(0,1))=0.01
    
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex("Base (RGB) Glossiness / Alpha (A)", 2D) = "white" {}

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

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

        _EmissionColor("Emission Color", Color) = (0,0,0)
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

            #pragma vertex vert
            #pragma fragment LitPassFragmentSimple
            #define BUMP_SCALE_NOT_SUPPORTED 1
            
            float _Open;
            float4 _Center;
            float _Radius;
            float _Amplitude;
            float _Frequency;
            float _Offset;
            float _Ratio;
            sampler2D _Distancemap;

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitForwardPass.hlsl"
            
            Varyings vert(Attributes input){
            
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                if(_Open == 1){
                    float3 worldSpaceVertex = TransformObjectToWorld(input.positionOS.xyz);
                    float d = distance(_Center,worldSpaceVertex);
                    float waveValueA = cos(d*_Frequency)*_Amplitude+_Offset;
                    if(d<_Radius){
                        float value = sqrt(d/_Radius*_Ratio);
                        //d = _Radius-d;
                        float dx = _Center.x-worldSpaceVertex.x;
                        float valuex = sqrt(abs(_Radius-dx)/_Radius*_Ratio);
                        float dz = _Center.z-worldSpaceVertex.z;
                        float valuez = sqrt(abs(_Radius-dz)/_Radius*_Ratio);
                        if(abs(dx)>0.1){
                            if(abs(dz)>0.1){
                                input.positionOS.xyz = float3(input.positionOS.x+dx*valuex,input.positionOS.y + waveValueA,input.positionOS.z+dz*valuez);
                            }else{
                                input.positionOS.xyz = float3(input.positionOS.x+dx*valuex,input.positionOS.y + waveValueA,input.positionOS.z);
                            }
                        }else{
                            if(abs(dz)>0.1){
                                input.positionOS.xyz = float3(input.positionOS.x,input.positionOS.y + waveValueA,input.positionOS.z+dz*valuez);
                            }else{
                                input.positionOS.xyz = float3(input.positionOS.x,input.positionOS.y + waveValueA,input.positionOS.z);
                            }
                        }
                    }   
                }
                /*
                if(_Open == 1){
                    float3 worldSpaceVertex = TransformObjectToWorld(input.positionOS.xyz);
                    float d = distance(_Center,worldSpaceVertex);
                    float waveValueA = -cos((_Radius-d)*_Frequency)*_Amplitude+_Offset;
                    if(d<_Radius){
                        
                        input.positionOS.xyz = float3(input.positionOS.x,input.positionOS.y + waveValueA,input.positionOS.z);
                    }   
                }*/
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                
                half3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
                            
            #if !SHADER_HINT_NICE_QUALITY
                viewDirWS = SafeNormalize(viewDirWS);
            #endif
            
                half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
                half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
            
                output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
                output.posWSShininess.xyz = vertexInput.positionWS;
                output.posWSShininess.w = _Shininess * 128.0;
                output.positionCS = vertexInput.positionCS;
            
            #ifdef _NORMALMAP
                output.normal = half4(normalInput.normalWS, viewDirWS.x);
                output.tangent = half4(normalInput.tangentWS, viewDirWS.y);
                output.bitangent = half4(normalInput.bitangentWS, viewDirWS.z);
            #else
                output.normal = normalInput.normalWS;
                output.viewDir = viewDirWS;
            #endif
            
                OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
                OUTPUT_SH(output.normal.xyz, output.vertexSH);
            
                output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
            
            #if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
                output.shadowCoord = GetShadowCoord(vertexInput);
            #endif
            
                return output;
            }
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
