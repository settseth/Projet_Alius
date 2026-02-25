Shader "Custom/VideoAdditiveNoFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        // On dit à Unity que c'est transparent
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend One One // C'est ici qu'on fait l'effet Additif (le noir devient transparent)
        Cull Off // Permet de voir la vidéo des deux côtés du plan

        Pass
        {
            // LA LIGNE MAGIQUE QUI DÉSACTIVE LE BROUILLARD
            Fog { Mode Off } 

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _TintColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // On affiche la texture (la vidéo) et on peut même la teinter
                fixed4 col = tex2D(_MainTex, i.uv) * _TintColor;
                return col;
            }
            ENDCG
        }
    }
}