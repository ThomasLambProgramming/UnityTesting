Shader "Unlit/LerpExample"
{
    Properties
    {
        _Skin1 ("Skin 1", 2D) = "white" {}
        _Skin2 ("Skin 2", 2D) = "white" {}
        _Lerp ("Lerp Amount", Range(0.0, 1.0)) = 0.5
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;

                //Each texture needs to be put into the appdata so the transform tex function works (idk i think its a unity thing)
                //This is for tiling and offset i believe not sure how it works though. check later.
                float2 uv_s01 : TEXCOORD0;
                float2 uv_s02 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv_s01 : TEXCOORD0;
                float2 uv_s02 : TEXCOORD1;
            };

            float4 _Skin1_ST;
            float4 _Skin2_ST;
            sampler2D _Skin1;
            sampler2D _Skin2;
            float _Lerp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.uv_s01 = TRANSFORM_TEX(v.uv_s01, _Skin1);
                o.uv_s02 = TRANSFORM_TEX(v.uv_s02, _Skin2);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 skin1Col = tex2D(_Skin1, i.uv_s01);
                fixed4 skin2Col = tex2D(_Skin2, i.uv_s02);

                //All of this is just lerping the two colors from sampling 2 different textures.
                fixed4 render = lerp(skin1Col, skin2Col, _Lerp);
                return render;
            }
            ENDCG
        }
    }
}
