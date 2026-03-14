Shader "Custom/CameraFadeShader"
{
    Properties
    {
        _Fade ("Fade", Range(0,1)) = 0
        _Softness ("Softness", Range(0.001,1)) = 0.2
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Fade;
            float _Softness;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.vertex.xy / _ScreenParams.xy;

                float2 center = float2(0.5, 0.5);

                float2 offset = uv - center;

                offset.x *= _ScreenParams.x / _ScreenParams.y;

                float dist = length(offset);

                float edge = smoothstep(_Fade - _Softness, _Fade, dist);

                return fixed4(0,0,0, edge);
            }

            ENDCG
        }
    }
}
