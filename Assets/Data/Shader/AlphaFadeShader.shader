Shader "Custom/BackGround/AlphaFadeShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 1)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

            /****************************************************************
            *                            Pass 1
            *****************************************************************
            * - Zwrite on
            * - Rendering off (ColorMask 0)
            * - �н� 1�� ���� : ���� ���� ������ �߻����� �ʵ���
            *                   Z���ۿ� ������ֱ�
            *****************************************************************/
            ZWrite On
            ColorMask 0

            CGPROGRAM
            #pragma surface surf nolight noambient noforwardadd nolightmap novertexlights noshadow

            struct Input
            {
                float4 color:COLOR; // Input�� ������� �ʱ� ���� �ۼ�
            };

        // ���ǽ� �Լ� : �ƹ� �͵� ���� ����
        void surf(Input IN, inout SurfaceOutput o) {}

        // ������ �Լ� : �ƹ� �͵� ���� ����
        float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
        {
            return float4(0,0,0,0);
        }
        ENDCG

            /****************************************************************
            *                            Pass 2
            *****************************************************************
            * - Zwrite off
            * - �н� 2 : ���� �н�. ���⼭ ���� ���. ���� ���⵵ ����
            *****************************************************************/
            ZWrite Off

            CGPROGRAM
            #pragma surface surf Lambert alpha:fade

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            float _Alpha;

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Alpha = _Alpha;
            }
            ENDCG
        }
            Fallback "Transparent" // �׸��� ���� X
}