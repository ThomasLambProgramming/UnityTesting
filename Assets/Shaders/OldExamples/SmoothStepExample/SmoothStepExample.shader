Shader "Unlit/SmoothStepExample"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                // add the edge
                float edge = 0.5;
                // add the amount of interpolation
                float smooth = 0.1;
                // add the return value in RGB
                // sstep = step (edge, i.uv.y);

                //smoothstep is min, max, value.

                //As 0.5 is greater than the min+max before i.uv.y > (0.5 - smoothing) = 1 (white), as the uv is getting to 0.4-0.6 the edge is actually being calculated as the min and max increase the edge is slowly brought from 0.4-0.6 in value. 
                fixed3 sstep = 1 - smoothstep((i.uv.y - smooth), (i.uv.y + smooth), edge);
                //the 1 - is so its the same as the step example with black on the bottom rising up.
                //This line below does the same as above. the below makes so much more sense then using the edge, the edge acts as a location more than anything, adding on to this the smooth just extends the range of the smoothing (which makes sense)
                //Alrighty i understand that now!

                //Smoothstep in the book seems to be used as the above quite alot, where its more of a smoothstep(uvvalue - smoothing distance, uvvalue + smoothing distance, middlepoint) (just have to wrap my head around that but i dont think it will be that bad).
                
                //sstep = smoothstep(0.4, 0.6, i.uv.y);
                // fixed4 col = tex2D (_MainTex, i.uv);
                return fixed4(sstep, 1);
            }
            ENDCG
        }
    }
}
