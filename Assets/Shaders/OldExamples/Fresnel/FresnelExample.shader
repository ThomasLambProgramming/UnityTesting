Shader "Unlit/FresnelExample"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FresnelPower ("Fresnel Power", Range(1,5)) = 1
        _FresnelIntensity ("Fresnel Intensity", Range(0,1)) = 1
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal_world : TEXCOORD1;
                float3 vertex_world : TEXCOORD2;
            };

            void unity_FresnelEffect_float(
                in float3 normal,
                in float3 viewDirection,
                in float fresnelPow,
                out float fresnel
            )
            {
                fresnel = pow(1 - saturate(dot(viewDirection, normal)), fresnelPow);
            }

            

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FresnelIntensity;
            float _FresnelPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal_world = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0))).xyz;
                o.vertex_world = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 normal = i.normal_world;
                float3 viewDirection = normalize(_WorldSpaceCameraPos - i.vertex_world);
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float fresnel = 0;
                unity_FresnelEffect_float(normal,viewDirection,_FresnelPower, fresnel);

                col += fresnel * _FresnelIntensity;
                return col;
            }
            ENDCG
        }
    }
}
