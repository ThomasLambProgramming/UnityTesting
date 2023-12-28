Shader "Unlit/FloorExample"
{
    //According to the shader book this is the basis on how toon shaders work.
    //I somewhat understand it but still adding steps and the division count still doesnt fully click in my head.
    
    
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [IntRange] _Sections("Sections", Range (2, 32)) = 5
        _Gamma ("Gamma", Range(0,1)) = 0
        [IntRange] _DivisionAmount ("Division Amount", Range (1, 1000)) = 100
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
            float _Gamma;
            float _Sections;
            float _DivisionAmount;

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
                float fv = floor(i.uv.y * _Sections) * (_Sections / _DivisionAmount);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * fv + _Gamma;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
