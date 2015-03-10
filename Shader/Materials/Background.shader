Shader "Custom/Background" {
	Properties{	
	_MainTex("Main",2D) = "White"{}
	}
	SubShader{
		Pass{
			Material{Diffuse(1,1,1,0) Ambient(1,1,1,0)}
			Lighting On
			SetTexture[_MainTex]{
				matrix[_Rotation] 
				combine texture * primary double,texture
			}
		}
	}	
}
