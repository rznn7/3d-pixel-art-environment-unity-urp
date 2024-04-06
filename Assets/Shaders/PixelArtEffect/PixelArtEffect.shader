Shader "Custom/PixelArtEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TargetResolution ("Targer Resolution", Vector) = (320, 240, 0, 0)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS: POSITION;
                float2 uv: TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS: SV_POSITION;
                float2 uv: TEXCOORD0;
            };

            Varyings vert(Attributes input) {
                Varyings output;

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;

                return output;
            }

            sampler2D _MainTex;
            float4 _TargetResolution;

            half4 frag(Varyings input): SV_TARGET {
                // Résolution cible pour la pixellisation, par exemple 320x240 pour un effet retro
                float2 targetRes = float2(320.0, 240.0);

                // Calculer le ratio de la résolution cible par rapport à la résolution actuelle de la RenderTexture
                float2 ratio = _ScreenParams.xy / targetRes;

                // Ajuster les coordonnées UV pour "snapper" aux pixels virtuels de la résolution cible
                float2 adjustedUV = floor(input.uv * targetRes) / targetRes;

                // Optionnel : Ajustement pour centrer les pixels agrandis
                adjustedUV += (1.0 / targetRes) * 0.5;

                // Échantillonner la texture avec les coordonnées UV ajustées
                half4 color = tex2D(_MainTex, adjustedUV);

                return color;
            }
            ENDHLSL
        }
    }
}