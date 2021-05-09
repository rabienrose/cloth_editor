Shader "Shader/PolyChamo" {
	Properties
	{
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Pass
		{
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
			float4 _Color;
			vertexOutput vert(vertexInput input){
				vertexOutput output;
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			}
			float4 frag(vertexOutput input) : SV_Target{
				return _Color;
			}
			ENDCG
		}
	}
}