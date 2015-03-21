Shader "Custom/Flash" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FlowTex ("Light(RGB)" ,2D) = "black"{}
		_Speed ("Speed", float) = 2
		_Color ("Light Color",Color) = (0,0,0,0)
		_Light ("Lightness", Range(0.0,2.0)) = 1.0
	}
	SubShader {
		//Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		Tags {"RenderType" = "Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _FlowTex;
		float4 _Color;
		float _Speed;
		float _Light;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			float2 uv = IN.uv_MainTex;
			uv.y /= 2;
			uv.x = -uv.x;
			uv.y = -uv.y;
			uv.y += _Time.y *_Speed;

			float flow = tex2D (_FlowTex, uv).a;
            
			o.Emission = c.rgb + float3 (flow,flow,flow) * _Light * _Color;
			//o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
