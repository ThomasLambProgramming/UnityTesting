Shader "Unlit/Culltesting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // [Enum(Off, 0, Back, 1, Front, 2)] CullMask ("Culling Mask", int) = 1
        
        [Enum(UnityEngine.Rendering.CullMode)] 
        _CullMask ("Culling Mask", Float) = 0
        
        _FrontColor ("Front Color", Color) = (0,0,0,0) 
        _BackColor ("Back Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Pass
        {
            Cull off
            
            CGPROGRAM

            float4 _FrontColor;
            float4 _BackColor;
            
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            //SV_IsFrontFace only works when cull is off. Although now it draws both sides so there is that factor to put in as well;
            fixed4 frag (v2f i, bool face : SV_IsFrontFace) : SV_Target
            {
                // sample the texture
                
                fixed4 col = tex2D(_MainTex, i.uv);

                if (face)
                    col = _FrontColor;
                else
                {
                    col = _BackColor;
                }
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        
        //Other way of doing it with just using the enum from the unity draw. (i like sv isfront face better since i can then just have it on the frag       
        //Tags { "RenderType"="Opaque" }
        //LOD 100
        //
        //Pass
        //{
        //    Cull [_CullMask]
        //    CGPROGRAM
        //    
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    // make fog work
        //    #pragma multi_compile_fog
        //
        //    #include "UnityCG.cginc"
        //
        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float2 uv : TEXCOORD0;
        //    };
        //
        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        UNITY_FOG_COORDS(1)
        //        float4 vertex : SV_POSITION;
        //    };
        //
        //    sampler2D _MainTex;
        //    float4 _MainTex_ST;
        //
        //    v2f vert (appdata v)
        //    {
        //        v2f o;
        //        o.vertex = UnityObjectToClipPos(v.vertex);
        //        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //        UNITY_TRANSFER_FOG(o,o.vertex);
        //        return o;
        //    }
        //
        //    fixed4 frag (v2f i) : SV_Target
        //    {
        //        // sample the texture
        //        fixed4 col = tex2D(_MainTex, i.uv);
        //        // apply fog
        //        UNITY_APPLY_FOG(i.fogCoord, col);
        //        return col;
        //    }
        //    ENDCG
        //}
    }
}
