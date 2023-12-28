Shader "Unlit/TanTesting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Sections ("Sections", Range(2,10)) = 10
        _Speed ("Scroll Speed", Float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
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

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Sections;
            float _Speed;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //Because of the value being greater than 1 we want to limit it, Time is added to cause movement
                float4 tanColor = clamp(0, abs(tan((i.uv.y - _Time.x * _Speed) * _Sections)), 1);
                tanColor *= _Color;
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv) * tanColor;
                
                return col;
            }
            ENDCG
        }
    }
}
