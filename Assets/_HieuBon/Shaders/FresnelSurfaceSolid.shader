Shader "FresnelSurfaceSolid" {
	Properties
    {
        _AlbedoColor ("Albedo Color", Color) = (0.15094, 0.131719, 0.131719, 1) 

        _EmissionColor ("Emission Color", Color) = (0.06603, 0.06603, 0.06603, 1)

        _FresnelColor ("Fresnel Color", Color) = (0.35849, 0.34327, 0.34327, 1)

        _FresnelScale ("Fresnel Scale", Float) = 2

        _FresnelPower ("Fresnel Power", Range(0.01, 100)) = 5

        _Smooth ("Smooth", Range(0, 1)) = 0

        _Metal ("Metal", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" } 
        LOD 200

        CGPROGRAM
        
        #pragma surface surf Standard fullforwardshadows

        fixed4 _AlbedoColor;
        fixed4 _EmissionColor;
        fixed4 _FresnelColor;
        float _FresnelScale;
        float _FresnelPower;
        float _Smooth;
        float _Metal;

        struct Input
        {
            float3 viewDir; 
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float NdotV = 1.0 - saturate(dot(o.Normal, IN.viewDir));

            float fresnel = _FresnelScale * pow(NdotV, _FresnelPower);
            fresnel = saturate(fresnel);

            o.Albedo = _AlbedoColor.rgb;

            o.Emission = _EmissionColor.rgb + (_FresnelColor.rgb * fresnel);

            o.Metallic = _Metal;
            o.Smoothness = _Smooth;

            // o.Alpha = _AlbedoColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}