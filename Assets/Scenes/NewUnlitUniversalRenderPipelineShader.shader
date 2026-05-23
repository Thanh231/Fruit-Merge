Shader "Custom/URP_SpriteGlow"
{
    Properties
    {
        // [PerRendererData] giúp tự động lấy ảnh từ SpriteRenderer mà không cần kéo thả tay
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        
        // Màu cơ bản để tint (nhuộm màu) cho Sprite nếu muốn
        _Color ("Tint", Color) = (1,1,1,1)
        
        // [HDR] kích hoạt bảng màu phát sáng
        [HDR] _GlowColor ("Glow Color (HDR)", Color) = (1,1,1,1)
        
        // Thanh trượt điều chỉnh độ rực rỡ
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 2.0
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        
        // Chế độ hòa trộn tiêu chuẩn cho nền trong suốt (Alpha)
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Tương thích chuẩn với URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Gói các biến vào CBUFFER để tương thích với SRP Batcher (hết bị báo lỗi vàng)
            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _GlowColor;
                float _GlowIntensity;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.color = input.color * _Color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Lấy màu gốc của Sprite
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                
                // Nhân màu gốc với màu Glow và độ mạnh (Intensity)
                half3 finalColor = texColor.rgb * input.color.rgb * _GlowColor.rgb * _GlowIntensity;
                
                // Trả về màu đã phát sáng + giữ nguyên viền trong suốt
                return half4(finalColor, texColor.a * input.color.a);
            }
            ENDHLSL
        }
    }
}