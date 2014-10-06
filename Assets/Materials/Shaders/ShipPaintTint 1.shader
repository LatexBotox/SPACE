Shader "Unlit Wat" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
CGPROGRAM
#pragma surface surf Lambert


fixed4 _Color;

struct Input {
	float3 viewDir;
};


void surf (Input IN, inout SurfaceOutput o) {
	o.Albedo = _Color;
}
ENDCG
}

//FallBack "Specular"
}
