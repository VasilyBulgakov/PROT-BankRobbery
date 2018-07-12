// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GridFromCenter" {
    Properties {
        _CellSize ("Grid cell size", Float) = 0.5
        _LineWidth ("Grid line width", Float) = 0.1
        _GridColor ("Color", Color) = (1, 1, 1, 1)
		_OffsetX ("Offset axis X", Float) = 0.0001
		_OffsetY ("Offset axis Y", Float) = 0.0001
		_OffsetZ ("Offset axis Z", Float) = 0.0001
		_Floor ("Value floor on axis Y", Float) = -1.0
		_Camera ("Camera", Vector) = (0, 0, 0, 0)
		_WidthScan ("Value high scaning", Float) = 10.0
		_StartLevel ("Start level scan", Float) = -0.5

		_RadiusScan ("Value radius scaning", Float) = 0.7

		_IsSphere ("Scaning sphere, 0 - is not sphere", Int) = 0


    }
    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        pass
        {
            Cull Back
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
  
            struct VertIn
            {
                float4 vertex : POSITION;
            };
  
            struct VertOut
            {
                float4 position : POSITION;
                float3 locpos : TEXCOORD0;
            };
 
            float _CellSize;
            float _LineWidth;
            float4 _GridColor;

			float _OffsetX;
			float _OffsetY;
			float _OffsetZ;

			float4 _Camera;
			float _Floor;
			float _WidthScan;
			float _StartLevel;
			float _RadiusScan;
			int _IsSphere;

            VertOut vert(VertIn input)
            {
                VertOut output;
                output.position = UnityObjectToClipPos(input.vertex);
                output.locpos = float3(input.vertex.x + _OffsetX, input.vertex.y + _OffsetY, input.vertex.z + _OffsetZ);//
                return output;
            }
 
            float getGridFact(float pos)
            {
                float snapPos = round(pos / _CellSize) * _CellSize;
                float dist = abs(snapPos - pos);
                return 1 - min(1.f, dist * 2.f / _LineWidth);
            }
 
            float4 frag(VertOut i) : COLOR
            {
                float factX = getGridFact(i.locpos.x);
				float factY = getGridFact(i.locpos.y);
                float factZ = getGridFact(i.locpos.z);
				if(_IsSphere == 0)
				{
					if((i.locpos.y < _StartLevel + _WidthScan/2) && (i.locpos.y > _StartLevel - _WidthScan/2)
					&& pow(i.locpos.z - _Camera.z, 2) + pow(i.locpos.x - _Camera.x, 2) < pow(_RadiusScan,2))  
				
					//&& (i.locpos.z < _Camera.z + _RadiusScan) && (i.locpos.z > _Camera.z - _RadiusScan)
					//&& (i.locpos.x < _Camera.x + _RadiusScan) && (i.locpos.x > _Camera.x - _RadiusScan)
					{
						if(i.locpos.y - _OffsetY > _Floor)
						{
							return _GridColor * float4(1.f, 1.f, 1.f,  factX + factZ + factY - (factX * factY * factZ));
							//return _GridColor * float4(1.f, 1.f, 1.f, factZ);
						}
						else
						{
							return float4(1.f, 1.f, 1.f, 0);
						}
					}
					else
					{
				 		return float4(1.f, 1.f, 1.f, 0);
					}
				}
				else
				{
					if(pow(i.locpos.x - _Camera.x, 2) + pow(i.locpos.y - _Camera.y, 2) + pow(i.locpos.z - _Camera.z, 2) < pow(_RadiusScan,2))  
				
					//&& (i.locpos.z < _Camera.z + _RadiusScan) && (i.locpos.z > _Camera.z - _RadiusScan)
					//&& (i.locpos.x < _Camera.x + _RadiusScan) && (i.locpos.x > _Camera.x - _RadiusScan)
					{
						if(i.locpos.y - _OffsetY > _Floor)
						{
							return _GridColor * float4(1.f, 1.f, 1.f,  factX + factZ + factY - (factX * factY * factZ));
							//return _GridColor * float4(1.f, 1.f, 1.f, factZ);
						}
						else
						{
							return float4(1.f, 1.f, 1.f, 0);
						}
					}
					else
					{
				 		return float4(1.f, 1.f, 1.f, 0);
					}
				}

            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}
