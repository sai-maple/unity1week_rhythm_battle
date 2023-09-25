Shader "Custom/Repeat"
{
    Properties
    {
        color1("Color 1",Color) = (0,0,0,1)
        color2("Color 2",Color) = (1,1,1,1)
        speed("Speed" , Float) = 1
        slice("Slice",Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //頂点シェーダーに渡ってくる頂点データ
            struct appdata
            {
                float4 vertex : POSITION;
            };

            //フラグメントシェーダーへ渡すデータ
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 world_pos : WORLD_POS;
            };

            float4 color1;
            float4 color2;
            float speed;
            float slice;

            //頂点シェーダー
            v2f vert(appdata v)
            {
                v2f o;
                //unity_ObjectToWorld × 頂点座標(v.vertex) = 描画しようとしてるピクセルのワールド座標　らしい
                //mulは行列の掛け算をやってくれる関数
                o.world_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex); //3D空間座標→スクリーン座標変換
                return o;
            }

            //フラグメントシェーダー
            fixed4 frag(v2f i) : SV_Target
            {
                const float dot_result = dot(i.world_pos, normalize(float2(1, 1)));
                const float repeat = abs(dot_result - _Time.w * speed);
                const float interpolation = step(fmod(repeat, 1), slice);
                fixed4 col = lerp(color1, color2, interpolation);
                return col;
            }
            ENDCG
        }
    }
}