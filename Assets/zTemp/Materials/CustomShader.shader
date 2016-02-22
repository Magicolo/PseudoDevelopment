Shader "Custom/CustomShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Amount("Extrusion", Range(-100,100)) = 0
	}
		
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vertex 
			#pragma fragment fragment

			struct vertexInput
			{
				float4 position : POSITION;
			};

			struct vertexOutput
			{
				float4 position : POSITION;
			};

			vertexOutput vertex(vertexInput input)
			{
				vertexOutput output;
				output.position = mul(UNITY_MATRIX_MVP, input.position);

				return output;
			}

			half4 fragment(vertexOutput output) : COLOR
			{
				return half4(1, 1, 1, 1);
			}

			ENDCG
		}
	}
		Fallback "Diffuse"
}
