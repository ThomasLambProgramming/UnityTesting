Shader "Custom/PointURP"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
		#pragma target 4.5
        

        #include "PointURPGpu.hlsl"
        struct Input
        {
            float3 worldPos;
        };

        fixed4 _Color;

        void ConfigureSurface(Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = fixed4(IN.worldPos,0);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
