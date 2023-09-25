Shader "Custom/Vignette"
{
    Properties
    {
        vignette_color("Color",Color) = (0,0,0,1)
        power( "Power", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2_f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2_f vert(const appdata v)
            {
                v2_f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = v.uv;
                o.uv.zw = o.vertex.xy;
                return o;
            }

            float4 vignette_color;
            float power;

            fixed4 frag(v2_f i) : SV_Target
            {
                const float2 vp = i.uv.zw;
                const float4 vignette = 0;
                return lerp(vignette, vignette_color, dot(vp, vp) * power);
            }
            ENDCG
        }
    }
}