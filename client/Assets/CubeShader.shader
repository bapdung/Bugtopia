Shader "Custom/FrontFaceTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Transparency ("Transparency", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Transparency;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Detect the front face (Z-axis normal pointing forward)
                if (i.worldNormal.z > 0.9) // Adjust for tolerance
                {
                    // Apply transparency to front face
                    return fixed4(1.0, 1.0, 1.0, _Transparency);
                }
                else
                {
                    // Render other faces opaque
                    return fixed4(1.0, 1.0, 1.0, 1.0);
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
