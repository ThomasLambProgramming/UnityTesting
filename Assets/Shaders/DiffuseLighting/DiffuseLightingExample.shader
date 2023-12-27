Shader "Unlit/DiffuseLightingExample"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightIntensity("Light Intensity", Range(0,1)) = 0.2
        
        _SpecularTex ("Specular Texture", 2D) = "black" {}
        _SpecularIntensity ("Specular Intensity", Range(0,1)) = 1
        
        //Damn what da hell is it going to 128 for!
        _SpecularPower ("Specular Power", Range(1,128)) = 64
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags 
            {
                "RenderType"="Opaque" 
                "LightMode"="ForwardBase"
            }
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
                float3 normal_WorldConverted : TEXCOORD1;
                float3 vertex_World : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _SpecularTex;
            float4 _MainTex_ST;

            //The _ST add tiling and offset ot the texture, but the transformation done by this is not needed as textures or specular makes dont need them due to their consistent nature.
            //float4 _SpecularTex_ST;

            float _LightIntensity;
            float _SpecularIntensity;
            float _SpecularPower;
            
            
            //Unity internal variable i believe. check more on this later.
            //This also refers to i believe the scene lighting color so might have to check what the difference between this and the reflection color is or
            //if they are called the same thing.
            float4 _LightColor0;
            
            //Dont forget that these will be in world space, make sure that they are converted accordingly before using in function.
            float3 LambertShading
            (
                float3 colorReflect,
                float lightIntensity,
                float3 surfNormal,
                float3 lightDirection
            )
            {
                return colorReflect * lightIntensity * max(0, dot(surfNormal, lightDirection));
            }
            float3 SpecularShading
            (
                float3 colorReflection,
                float specularIntensity,
                float3 normal,
                float3 lightDirection,
                float3 viewDirection,
                float specularPower
            )
            {
                float3 halfwayVector = normalize(lightDirection + viewDirection);
                return colorReflection * specularIntensity * pow(max(0,dot(normal, halfwayVector)), specularPower);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                //float4 because of mat 4x4 by a float4 to convert then normalize the whole float and return its xyz value
                o.normal_WorldConverted = normalize(mul(unity_ObjectToWorld, float4(v.normal,0))).xyz;
                o.normal_WorldConverted = UnityObjectToWorldNormal(v.normal);

                //the two above seem to be the same thing but the code behind them is slightly different, ask dale about why this is.
                o.vertex_World = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed3 colorRef = _LightColor0.rgb;

                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                half3 diffuseLighting = LambertShading(colorRef, _LightIntensity, i.normal_WorldConverted, lightDirection);

                //Direction from the vertex position TO the camera position as it is similar to the light bouncing forward TO the camera from where it will be observed.
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.vertex_World);
                //get the specular and give it the color from the light source 0 (generally main directional light in the scene.)
                fixed3 specCol = tex2D(_SpecularTex, i.uv) * colorRef;
                half3 specularColour = SpecularShading(specCol, _SpecularIntensity, i.normal_WorldConverted, lightDirection, viewDirection, _SpecularPower);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col.rgb *= diffuseLighting;
                col.rgb += specularColour;
                return col;
            }
            ENDCG
        }
    }
}
