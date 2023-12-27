Shader "Unlit/AmbentReflectionExample"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ReflectionTex ("Reflection Texture", Cube) = "white" {}
        _ReflectionInt ("Reflection Intensity", Range(0, 1)) = 1
        _ReflectionMet ("Reflection Metallic", Range(0, 1)) = 0
        _ReflectionDet ("Reflection Detail", Range(1, 9)) = 1
        _ReflectionExp ("Reflection Exposure", Range(1, 3)) = 1
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

            float3 AmbientReflection(
                samplerCUBE colorRefl,
                float reflectionIntensity,
                half reflectionTexelDensity,
                float3 normal,
                float3 viewDir,
                float reflectionExp
            )
            {
                float3 reflection_World = reflect(viewDir, normal);
                float4 cubemap = texCUBElod(colorRefl, float4(reflection_World, reflectionTexelDensity));
                return reflectionIntensity * cubemap.rgb * (cubemap.a * reflectionExp);
            }
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal_world : TEXCOORD1;
                float3 vertex_world : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            samplerCUBE _ReflectionTex;
            float _ReflectionInt;
            half _ReflectionDet;
            float _ReflectionExp;
            float _ReflectionMet;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal_world = normalize(mul(unity_ObjectToWorld, float4(v.normal,0))).xyz;
                o.vertex_world = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                half3 normal = i.normal_world;
                //This unity function is equivalent to the camera position and the object verticices calculation
                half3 viewDir = normalize(UnityWorldSpaceViewDir(i.vertex_world));
                half3 reflection = AmbientReflection(
                    _ReflectionTex,
                    _ReflectionInt,
                    _ReflectionDet,
                    normal,
                    //Apparently the view dir is in the reverse direction that we want
                    //from the included unity function so we negate to flip the value around
                    //to be correct to sample the cubemap correctly. fig 7.0.5c if confused.
                    -viewDir,
                    _ReflectionExp);
                
                //Apply the metallalic amount.
                col.rgb *= reflection * _ReflectionMet;
                return col;
            }
//unity_SpecCube[n] contians the unity default reflection probe object.
//This is the unity function version, the unity_sample_texture part autofils the skybox data if present.
//copy pasted because i was on a train and cant be bothered typing this all out on a laptop.
 /*           fixed4 frag (v2f i) : SV_Target
{
fixed4 col = tex2D(_MainTex, i.uv);
half3 normal = i.normal_world;
half3 viewDir = normalize(UnityWorldSpaceViewDir(i.vertex_world));
half3 reflect_world = reflect(-viewDir, normal);
// the process mentioned above is replaced by the function
// UNITY_SAMPLE_TEXCUBE
half3 reflectionData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0,
reflect_world );
half3 reflectionColor = DecodeHDR(reflectionData , unity_SpecCube0_HDR);
col.rgb = reflectionColor;
return col;
}*/


            ENDCG
        }
    }
}
