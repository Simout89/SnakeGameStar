Shader "Custom/LiquidSprite"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // Параметры жидкости
        _LiquidColor ("Liquid Color", Color) = (0.2, 0.5, 1, 0.6)
        _FillAmount ("Fill Amount", Range(0, 1)) = 0.5
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1
        _WaveAmplitude ("Wave Amplitude", Range(0, 0.2)) = 0.05
        _WaveFrequency ("Wave Frequency", Range(1, 20)) = 5
        
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _LiquidColor;
            float _FillAmount;
            float _WaveSpeed;
            float _WaveAmplitude;
            float _WaveFrequency;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Получаем базовый цвет спрайта
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                // Если пиксель спрайта прозрачный, не рисуем ничего
                if (c.a < 0.01)
                    return c;
                
                // Вычисляем волну
                float wave = sin((IN.texcoord.x * _WaveFrequency) + (_Time.y * _WaveSpeed)) * _WaveAmplitude;
                
                // Граница жидкости с учётом волны
                float liquidLevel = 1.0 - _FillAmount + wave;
                
                // Если пиксель ниже уровня жидкости И находится в видимой части спрайта
                if (IN.texcoord.y < liquidLevel)
                {
                    // Смешиваем цвет жидкости с оригинальным спрайтом
                    c.rgb = lerp(c.rgb, _LiquidColor.rgb, _LiquidColor.a);
                    
                    // Добавляем блики на поверхности жидкости
                    float surfaceGlow = smoothstep(0.02, 0.0, abs(IN.texcoord.y - liquidLevel));
                    c.rgb += surfaceGlow * 0.3;
                }
                
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}