Shader "Bumped Specular Tinted" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_OffColor ("Off Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong vertex:vert


sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
fixed4 _OffColor;
half _Shininess;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
	float3 viewDir;
	half rim;
};

void vert (inout appdata_full v, out Input o) {
  UNITY_INITIALIZE_OUTPUT(Input,o);
  float3 orthoView = (0,0,1);
  o.rim =  0.9+saturate(dot (orthoView, abs(mul(UNITY_MATRIX_MVP, v.normal))));
}

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = tex.rgb * _OffColor.rgb * tex.a + tex.rgb * _Color.rgb * (1.0-tex.a);
	o.Gloss = tex.a;
	o.Alpha = 1;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
//	
//	half rim = 1.0 - saturate(dot (orthoView, o.Normal));
	o.Emission = _Color * pow(IN.rim, 15)*0.1+_OffColor*tex.a*0.7;
}
ENDCG
}

FallBack "Specular"
}
