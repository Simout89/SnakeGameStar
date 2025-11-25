Shader "Custom/URP/CartoonFlamethrowerSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Fire Colors)]
        _FireColor1 ("Fire Color 1 (Core)", Color) = (1, 1, 0.9, 1)
        _FireColor2 ("Fire Color 2 (Middle)", Color) = (1, 0.6, 0.1, 1)
        _FireColor3 ("Fire Color 3 (Outer)", Color) = (1, 0.3, 0, 1)
        _OutlineColor ("Outline Color", Color) = (0.9, 0.4, 0, 1)
        
        [Header(Cartoon Settings)]
        _ColorBands ("Color Bands", Range(2, 10)) = 3
        _OutlineWidth ("Outline Width", Range(0, 0.2)) = 0.08
        _Sharpness ("Edge Sharpness", Range(0.5, 5)) = 2.0
        
        [Header(Flamethrower Settings)]
        _FlowSpeed ("Flow Speed", Range(0, 10)) = 4.0
        _FlowDirection ("Flow Direction X", Range(-1, 1)) = 1.0
        _StreamWidth ("Stream Width", Range(0.1, 1)) = 0.7
        _Turbulence ("Turbulence", Range(0, 0.5)) = 0.1
        _CoreStability ("Core Stability", Range(0.5, 1)) = 0.85
        
        [Header(Visual)]
        _Brightness ("Brightness", Range(0.5, 3)) = 1.3
        _NoiseScale ("Noise Scale", Range(0.5, 3)) = 1.2
        
        [Header(Rendering)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 10
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
        }
        
        Blend [_SrcBlend] [_DstBlend]
        ZWrite Off
        Cull Off
        
        Pass
        {
            Name "CartoonFlamethrowerSprite"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
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
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _FireColor1;
                float4 _FireColor2;
                float4 _FireColor3;
                float4 _OutlineColor;
                float _ColorBands;
                float _OutlineWidth;
                float _Sharpness;
                float _FlowSpeed;
                float _FlowDirection;
                float _StreamWidth;
                float _Turbulence;
                float _CoreStability;
                float _NoiseScale;
                float _Brightness;
            CBUFFER_END
            
            float noise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            float smoothNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);
                
                float a = noise(i);
                float b = noise(i + float2(1.0, 0.0));
                float c = noise(i + float2(0.0, 1.0));
                float d = noise(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }
            
            float streamNoise(float2 uv)
            {
                float n = 0.0;
                // Более мелкие детали для турбулентности
                n += smoothNoise(uv * 2.0) * 0.5;
                n += smoothNoise(uv * 4.0) * 0.3;
                n += smoothNoise(uv * 8.0) * 0.2;
                return n;
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color;
                output.fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                float time = _Time.y * _FlowSpeed;
                
                // Горизонтальный поток (огнемёт)
                float2 flowUV = uv;
                flowUV.x -= time * _FlowDirection;
                
                // Стабильное ядро потока
                float distFromCenter = abs(uv.y - 0.5) * 2.0;
                float coreStream = smoothstep(1.0, _CoreStability, distFromCenter);
                
                // Турбулентность на краях
                float2 noiseUV = flowUV * _NoiseScale;
                noiseUV.y += sin(flowUV.x * 5.0 + time) * 0.1;
                float turbulence = streamNoise(noiseUV);
                
                // Минимальная турбулентность в центре, больше на краях
                float turbulenceAmount = _Turbulence * (1.0 - coreStream);
                float streamDistortion = (turbulence - 0.5) * turbulenceAmount;
                
                // Применяем искажение
                float finalDist = distFromCenter + streamDistortion;
                
                // Форма струи огнемёта
                float streamShape = smoothstep(_StreamWidth, 0.0, finalDist);
                
                // Затухание по длине (опционально, можно убрать для бесконечной струи)
                float lengthFade = smoothstep(0.0, 0.1, uv.x) * smoothstep(1.0, 0.85, uv.x);
                streamShape *= lengthFade;
                
                // Добавляем детали с помощью шума
                float detail = streamNoise(noiseUV * 2.0);
                float fireValue = streamShape * (0.7 + detail * 0.3);
                
                // Cel shading - квантование
                fireValue = floor(fireValue * _ColorBands) / _ColorBands;
                
                // Чёткие границы для мультяшного стиля
                fireValue = pow(fireValue, _Sharpness);
                
                // Мультяшные цветовые зоны
                float3 fireColor;
                if(fireValue > 0.65)
                {
                    fireColor = _FireColor1.rgb; // Яркое ядро
                }
                else if(fireValue > 0.35)
                {
                    fireColor = lerp(_FireColor2.rgb, _FireColor1.rgb, (fireValue - 0.35) / 0.3);
                }
                else if(fireValue > 0.15)
                {
                    fireColor = lerp(_FireColor3.rgb, _FireColor2.rgb, (fireValue - 0.15) / 0.2);
                }
                else
                {
                    fireColor = _FireColor3.rgb * 0.7;
                }
                
                // Обводка
                float edgeMask = smoothstep(0.1, 0.2, fireValue) * smoothstep(0.35, 0.25, fireValue);
                fireColor = lerp(fireColor, _OutlineColor.rgb, edgeMask * _OutlineWidth * 10.0);
                
                // Применяем текстуру спрайта
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                
                // Яркость
                fireColor *= _Brightness;
                
                // Финальный цвет
                half4 finalColor;
                finalColor.rgb = fireColor * texColor.a;
                finalColor.a = smoothstep(0.05, 0.2, fireValue) * texColor.a * input.color.a;
                
                // Контраст для мультяшного вида
                finalColor.rgb = pow(finalColor.rgb, 0.85);
                
                // Туман
                finalColor.rgb = MixFog(finalColor.rgb, input.fogFactor);
                
                return finalColor;
            }
            ENDHLSL
        }
    }
    
    FallBack "Sprites/Default"
}