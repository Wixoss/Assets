Shader "Yman/Deck" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		//Tags {"Queue" = "Transparent"}
		LOD 200
		
		CGPROGRAM
		//#pragma surface surf Lambert alpha
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Emission = c.rgb * _Color.rgb;
			o.Alpha = _Color.a;
		}

//		inline float4 LightingHalfLambert(SurfaceOutput s, fixed3 lightDir,half3 viewDir, fixed atten)
//		{
//			float dif = max(0, dot(s.Normal, lightDir));
//			float halfLam = dif * 0.5 + 0.5;
//			float4 col;
//			col.rgb = s.Albedo * _LightColor0.rgb * halfLam * atten * 2;
//			col.a = s.Alpha;
//			return col;
//		}

		ENDCG
	} 
	FallBack "Diffuse"
}
