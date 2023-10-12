// Custom Eye Blink Effect Shader for First-Person View
Shader "Custom/EyeBlinkEffect"
{
    Properties
    {
        _MainTex("Eye Texture", 2D) = "white" {}
        _BlinkAmount("Blink Amount", Range(0, 1)) = 0.0
        _BlinkSpeed("Blink Speed", Range(0, 10)) = 2.0
        _FadeInCurve("Fade In Curve", Range(0, 1)) = 0.5
        _FadeOutCurve("Fade Out Curve", Range(0, 1)) = 0.5
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                float _BlinkAmount;
                float _BlinkSpeed;
                float _FadeInCurve;
                float _FadeOutCurve;

                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = v.vertex;
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 color = tex2D(_MainTex, i.uv);

                    // Calculate blink effect based on _BlinkAmount
                    half blinkEffect = 1.0 - smoothstep(0.0, 1.0, _BlinkAmount);
                    half time = _Time.y * _BlinkSpeed;
                    blinkEffect *= smoothstep(_FadeOutCurve, 1.0, time) * smoothstep(0.0, _FadeInCurve, time);

                    // Apply the blink effect to the color
                    color.rgb *= blinkEffect;

                    return color;
                }
                ENDCG
            }
        }
}
