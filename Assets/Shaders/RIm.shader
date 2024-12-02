Shader "Custom/WaterReflectiveDistortionRim"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _FresnelColor ("Fresnel Color", Color) = (0,1,1,1)
        _FresnelPower ("Fresnel Power", Range(0, 5)) = 1.0
        _MainTex ("Main Texture", 2D) = "white" {}
        _DistortionTex ("Distortion Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.2
        _DarknessCenter ("Center Darkness Intensity", Range(0, 1)) = 0.5
        _ReflectionTex ("Reflection Cubemap", Cube) = "" {}
        _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.5
        _EmissionStrength ("Emission Strength", Range(0, 5)) = 1.0
        _RimColor ("Rim Color", Color) = (1,0,0,1)
        _RimPower ("Rim Power", Range(0.1, 10)) = 2.0
        _RimThickness ("Rim Thickness", Range(0.1, 2)) = 0.5
        _Amplitude ("Amplitude", Float) = 0.1
        _Frequency ("Frequency", Float) = 4.0
        _Speed ("Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _FresnelColor;
                float _FresnelPower;
                float _DistortionStrength;
                float _DarknessCenter;
                float _ReflectionStrength;
                float _EmissionStrength;
                float4 _RimColor;
                float _RimPower;
                float _RimThickness;
                float _Amplitude;
                float _Frequency;
                float _Speed;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_DistortionTex);
            SAMPLER(sampler_DistortionTex);

            TEXTURECUBE(_ReflectionTex);
            SAMPLER(sampler_ReflectionTex);

            Varyings vert(Attributes v)
            {
                Varyings o;

                // Obtener posición en espacio mundial
                float3 positionWS = TransformObjectToWorld(v.positionOS.xyz);

                // Generar deformación tipo ola
                float wave = sin(positionWS.x * _Frequency + _Time.y * _Speed) *
                             cos(positionWS.z * _Frequency + _Time.y * _Speed);
                wave *= _Amplitude;

                // Modificar la posición del vértice
                positionWS += wave * normalize(v.normalOS);

                // Transformar la posición de vuelta al clip space
                o.positionHCS = TransformWorldToHClip(positionWS);
                o.worldPos = positionWS;
                o.uv = v.uv;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.viewDirWS = GetCameraPositionWS() - positionWS;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Normalize UV and apply distortion
                float2 uv = i.uv;
                float2 distortion = SAMPLE_TEXTURE2D(_DistortionTex, sampler_DistortionTex, uv).rg * 2.0 - 1.0;
                distortion *= _DistortionStrength;
                uv += distortion;

                // Sample base texture
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * _BaseColor;

                // Center darkness
                float centerFactor = saturate(length(i.worldPos.xz) * _DarknessCenter);
                baseColor.rgb *= (1.0 - centerFactor);

                // Fresnel effect
                float fresnel = pow(1.0 - saturate(dot(normalize(i.viewDirWS), normalize(i.normalWS))), _FresnelPower);
                half4 fresnelColor = _FresnelColor * fresnel;

                // Reflection effect
                float3 reflectionDir = reflect(-normalize(i.viewDirWS), normalize(i.normalWS));
                half4 reflectionColor = SAMPLE_TEXTURECUBE(_ReflectionTex, sampler_ReflectionTex, reflectionDir);
                reflectionColor *= _ReflectionStrength;

                // Rim effect
                float rimFactor = pow(1.0 - saturate(dot(normalize(i.viewDirWS), normalize(i.normalWS))), _RimPower);
                rimFactor = smoothstep(1.0 - _RimThickness, 1.0, rimFactor); // Control thickness
                half4 rimColor = _RimColor * rimFactor;

                // Add emissive fresnel effect
                half4 emissiveColor = fresnelColor * _EmissionStrength;

                // Combine all effects
                half4 finalColor = baseColor + reflectionColor + rimColor;
                finalColor.rgb += emissiveColor.rgb; // Add emissive fresnel to final output

                return finalColor;
            }
            ENDHLSL
        }
    }
}