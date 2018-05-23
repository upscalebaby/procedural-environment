Shader "Custom/TerrainShader" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
 
SubShader {
	Cull off
    Pass {
        ColorMaterial AmbientAndDiffuse
        Lighting On
        SetTexture [_MainTex] {
            constantColor [_Color]
            Combine previous * constant DOUBLE, previous * constant
        } 
    }
}
 
Fallback "VertexLit"
}