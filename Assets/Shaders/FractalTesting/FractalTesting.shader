Shader "Unlit/FractalTesting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", Range(0.0, 0.5)) = 0.3
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
            float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
                //Frac removes the ints, so the uv coords go up 3 times as much while still being in 0.0-1.0 range to repeat, (unsure about loss of quality though or if it samples less).
                //ask alex about sample rates and what happens to the detail when doing a i.uv * 3, because I feel like it would lose 3x the detail. hmmmm.

            fixed4 frag (v2f i) : SV_Target
            {
                float2 fractalPos = frac(i.uv * 3);
                
                float circle = length(fractalPos - 0.5);

                float wCircle = floor(0.3 / max(0.01, circle));

                return fixed4(wCircle.xxx, 1);

                //This also does the same thing as the above in less operations (unsure about how your meant to go about if statements.
                //seems like we really want to avoid them.
                //if (circle > _Size)
                //{
                //    return fixed4(0,0,0,1);
                //}
                //    return fixed4(1,1,1,1);
            }
            ENDCG
        }
    }
}
