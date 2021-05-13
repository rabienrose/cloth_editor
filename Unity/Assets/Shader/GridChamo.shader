Shader "Shader/GridChamo" {
	Properties
	{
		_GridColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Pass
		{
			// Blend Off
			// Cull Off
			// ZTest Always
			// ZWrite Off
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert  
			#pragma fragment frag 

			struct vertexInput {
				float4 vertex : POSITION;
			};
			struct vertexOutput {
				float4 pos : SV_POSITION;
			};
			float4 _GridColor;
			vertexOutput vert(vertexInput input){
				vertexOutput output;
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			}
			float4 frag(vertexOutput input) : SV_Target{
				return _GridColor;
			}
			ENDCG
		}
	}
}