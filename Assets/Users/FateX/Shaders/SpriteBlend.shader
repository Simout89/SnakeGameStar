Shader "Custom/URP/SpriteBlend"
{
    Properties
    {
        _BaseMap("Первая текстура", 2D) = "white" {}
        _BaseColor("Цвет первой текстуры", Color) = (1,1,1,1)
        
        _SecondTexture("Вторая текстура", 2D) = "white" {}
        _SecondColor("Цвет второй текстуры", Color) = (1,1,1,1)
        
        [Space(10)]
        [Header(Граница)]
        _SplitPosition("Позиция границы", Range(0, 1)) = 0.5
        _EdgeSoftness("Мягкость границы", Range(0.001, 0.2)) = 0.05
        
        [Space(5)]
        [Header(Шум границы)]
        _NoiseScale("Масштаб шума", Range(1, 100)) = 20
        _NoiseStrength("Искажение границы", Range(0, 0.5)) = 0.1
        
        [Space(5)]
        [KeywordEnum(Horizontal, Vertical, Diagonal)] _SplitDirection("Направление", Float) = 0
        
        [Space(10)]
        [Toggle(_ALPHATEST_ON)] _AlphaClip("Alpha Clipping", Float) = 0
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        
        [Space(10)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 10
        [Enum(Off, 0, On, 1)] _ZWrite("Z Write", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("Z Test", Float) = 4
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma shader_feature_local _ALPHATEST_ON
            #pragma shader_feature_local _SPLITDIRECTION_HORIZONTAL _SPLITDIRECTION_VERTICAL _SPLITDIRECTION_DIAGONAL
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float fogFactor : TEXCOORD1;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            TEXTURE2D(_SecondTexture);
            SAMPLER(sampler_SecondTexture);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _SecondTexture_ST;
                float4 _SecondColor;
                float _SplitPosition;
                float _Cutoff;
                float _EdgeSoftness;
                float _NoiseScale;
                float _NoiseStrength;
            CBUFFER_END
            
            // Процедурный шум (Simplex-подобный)
            float2 hash22(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
            }
            
            float noise(float2 p)
            {
                const float K1 = 0.366025404; // (sqrt(3)-1)/2
                const float K2 = 0.211324865; // (3-sqrt(3))/6
                
                float2 i = floor(p + (p.x + p.y) * K1);
                float2 a = p - i + (i.x + i.y) * K2;
                float2 o = (a.x > a.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
                float2 b = a - o + K2;
                float2 c = a - 1.0 + 2.0 * K2;
                
                float3 h = max(0.5 - float3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
                float3 n = h * h * h * h * float3(dot(a, hash22(i + 0.0)), dot(b, hash22(i + o)), dot(c, hash22(i + 1.0)));
                
                return dot(n, float3(70.0, 70.0, 70.0));
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.color = input.color;
                output.fogFactor = ComputeFogFactor(output.positionCS.z);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // Сэмплируем обе текстуры
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                half4 secondColor = SAMPLE_TEXTURE2D(_SecondTexture, sampler_SecondTexture, input.uv) * _SecondColor;
                
                // Выбираем координату в зависимости от направления
                float coord;
                #if defined(_SPLITDIRECTION_VERTICAL)
                    coord = input.uv.x;
                #elif defined(_SPLITDIRECTION_DIAGONAL)
                    coord = (input.uv.x + input.uv.y) * 0.5;
                #else // HORIZONTAL
                    coord = input.uv.y;
                #endif
                
                // Генерируем шум для неровной границы
                float2 noiseCoord = input.uv * _NoiseScale;
                float noiseValue = noise(noiseCoord);
                
                // Искажаем границу шумом
                float distortedBorder = _SplitPosition + noiseValue * _NoiseStrength;
                
                // Создаём плавную границу с помощью smoothstep
                float blendFactor = smoothstep(
                    distortedBorder - _EdgeSoftness, 
                    distortedBorder + _EdgeSoftness, 
                    coord
                );
                
                // Смешиваем текстуры
                half4 finalColor = lerp(baseColor, secondColor, blendFactor);
                
                // Умножаем на vertex color
                finalColor *= input.color;
                
                #ifdef _ALPHATEST_ON
                    clip(finalColor.a - _Cutoff);
                #endif
                
                // Применяем туман
                finalColor.rgb = MixFog(finalColor.rgb, input.fogFactor);
                
                return finalColor;
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #pragma shader_feature_local _ALPHATEST_ON
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _SecondTexture_ST;
                float4 _SecondColor;
                float _SplitPosition;
                float _Cutoff;
                float _EdgeSoftness;
                float _NoiseScale;
                float _NoiseStrength;
            CBUFFER_END
            
            float3 _LightDirection;
            
            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;
                
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                
                output.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                
                return output;
            }
            
            half4 ShadowPassFragment(Varyings input) : SV_Target
            {
                #ifdef _ALPHATEST_ON
                    half alpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).a * _BaseColor.a;
                    clip(alpha - _Cutoff);
                #endif
                
                return 0;
            }
            ENDHLSL
        }
    }
    
    FallBack "Universal Render Pipeline/Unlit"
    CustomEditor "UnityEditor.ShaderGUI.GenericShaderGraphMaterialGUI"
}