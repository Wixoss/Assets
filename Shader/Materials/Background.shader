Shader "Custom/Background" {
	Properties{	
	_MainTex("Main",2D) = "White"{}
	_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader{
		Pass{
			Material{Diffuse(1,1,1,0) Ambient(1,1,1,0)}
			Lighting On
			SetTexture[_MainTex]
			{
				constantColor [_Color]
				matrix[_Rotation] 
				combine texture * constant
				//combine texture * primary double,texture
			}
		}
	}	
}
