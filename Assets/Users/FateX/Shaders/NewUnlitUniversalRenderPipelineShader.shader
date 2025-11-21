Shader "Custom/Transferred_Flash_Dissolve_v3"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}

        // Flash
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
        _FlashEmission ("Flash Emission", Range(0,10)) = 1

        // Dissolve
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _NoiseScale ("Noise Scale", Float) = 30
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _FlashColor;
            float _FlashAmount;
            float _FlashEmission;

            float _DissolveAmount;
            float _NoiseScale;

            // -------- Simple Noise --------
            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            float noise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);

                float a =     hash21(i);
                float b =     hash21(i + float2(1,0));
                float c =     hash21(i + float2(0,1));
                float d =     hash21(i + float2(1,1));

                float2 u = f * f * (3 - 2 * f);

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 main = tex2D(_MainTex, i.uv);

                // ------------ FLASH (с emission) ------------
                float3 flashCol = _FlashColor.rgb * _FlashEmission;
                float4 flashMix = lerp(main, float4(flashCol, main.a), _FlashAmount);

                // ------------ DISSOLVE (0 = видно, 1 = исчезает) ------------
                float n = noise(i.uv * _NoiseScale);

                // исправлено как ты просил
                float mask = step(_DissolveAmount, n);

                float alpha = main.a * mask;

                return float4(flashMix.rgb * alpha, alpha);
            }

            ENDCG
        }
    }
}
