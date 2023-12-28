Shader "Unlit/ZoomEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ZoomAmount ("ZoomAmount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ZoomAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //the 0.5 apparently makes it centered. i get for the 1.0 uv coord but 0.5 would then be 0.25
                //im fucking stupid, its ceil = 1 at all times fml
                float u = ceil(i.uv.x) * 0.5;
                float v = ceil(i.uv.y) * 0.5;

                //we can do a uv ceil but we can also just set both to 0.5 to achieve the same result.  
                float uLerp = lerp(i.uv.x, u, _ZoomAmount);
                float vLerp = lerp(i.uv.y, v, _ZoomAmount);
                // sample the texture
                fixed4 col = tex2D(_MainTex, float2(uLerp, vLerp));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
