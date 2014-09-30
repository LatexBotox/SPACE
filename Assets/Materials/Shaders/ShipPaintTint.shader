Shader "Specular Painted" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_PaintColor ("Paint Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss/Tint (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong


sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
fixed4 _PaintColor;
half _Shininess;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
	float3 viewDir;
};


void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = tex.rgb * _Color.rgb * tex.a + tex.rgb * _PaintColor.rgb * (1.0-tex.a) * _PaintColor.a;
	o.Gloss = tex.a;
	o.Alpha = 1;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	float3 orthoView = (0,0,1);
}
ENDCG
}

//FallBack "Specular"
}
