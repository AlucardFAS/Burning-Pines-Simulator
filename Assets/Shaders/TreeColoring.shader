Shader "Custom/NewShader" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RedColor ("color for red part", Color) = (1,1,0,1)
        _GreenColor ("color for green part", Color) = (1,1,1,1)
        _BlueColor ("color for blue part", Color) = (0,0,0,1)

    }
    SubShader {
        Tags { "Queue"="Transparent" }
 
        CGPROGRAM
        #pragma surface surf Lambert alpha
 
        sampler2D _MainTex;
        half4 _RedColor;
        half4 _GreenColor;
        half4 _BlueColor;

        struct Input {
            float2 uv_MainTex;
			
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Emission = fixed4(_RedColor * c.r + _GreenColor * c.g + _BlueColor * c.b);  
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}