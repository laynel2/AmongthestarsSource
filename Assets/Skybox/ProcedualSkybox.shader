Shader "Unlit/ProcedualSkybox"
{
    Properties
    {
		//Colours
        _MainTex ("Texture", 2D) = "white" {}
		_HorizonColour ("Horizon Colour", color) = (0.5,0.5,1,1)
		_SkyColour("Sky Colour", color) = (1,0.2,0.8,1)


		//Stars
		_CellSize("StarSeed", Range(1, 50)) = 2
		_TilingX("tilingX", float) = 1
		_TilingY("tilingY", float) = 1

		//Sun
		_SunSize("SunSize", Range(1, 5)) = 2
		_SunCol("Sun Colour", color) = (0.8,0.8,0.2,1)
		_DarkCol("Dark Side Colour", color) = (0.8,0.8,0.2,1)
		_MaskOffset("Mask Offset", Vector) = (0,0,0)
		_MaskSize("Mask Size", Range(1, 8)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float3 worldpos: TEXCOORD0;
				half3 viewDir: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _SkyColour;
			float4 _HorizonColour;

			//Stars
			float _TilingX;
			float _TilingY;
			float _CellSize;

			//sun
			float _SunSize;
			float3 _SunDirection;
			float3 _SunCol;
			float3 _MaskOffset;
			float _MaskSize;
			float4 _DarkCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldpos = mul(unity_ObjectToWorld, v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
                return o;
            }

			#define PI 3.141592653589793
			inline float2 RadialCoords(float3 a_coords)
			{
				float3 a_coords_n = normalize(a_coords);
				float lon = atan2(a_coords_n.x, a_coords_n.z);
				float lat = acos(a_coords_n.y);
				float2 sphereCoords = float2(lon, lat) * (1.0 / PI);
				return float2(sphereCoords.x * 0.5 + 0.5, 1 - sphereCoords.y);
			}

			float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233)) {
				float2 smallValue = sin(value);
				float random = dot(smallValue, dotDir);
				random = frac(sin(random) * 143758.5453);
				return random;
			}

			float2 rand2dTo2d(float2 value) {
				return float2(
					rand2dTo1d(value, float2(12.989, 78.233)),
					rand2dTo1d(value, float2(39.346, 11.135))
					);
			}

			float voronoiNoise(float2 value) {
				float2 baseCell = floor(value);

				float minDistToCell = 10;
				[unroll]
				for (int x = -1; x <= 1; x++) {
					[unroll]
					for (int y = -1; y <= 1; y++) {
						float2 cell = baseCell + float2(x, y);
						float2 cellPosition = cell + rand2dTo2d(cell);
						float2 toCell = cellPosition - value;
						float distToCell = length(toCell);
						if (distToCell < minDistToCell) {
							minDistToCell = distToCell;
						}
					}
				}
				return minDistToCell;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float sunDot = acos(dot(_SunDirection, normalize(i.viewDir)));
				sunDot = 1- (step(1/(_SunSize * _SunSize), sunDot));
				float sunMask = acos(dot(normalize(_SunDirection + _MaskOffset), normalize(i.viewDir)));
				sunMask = 1 - (step(1 / (_MaskSize * _MaskSize), sunMask));

				float finalsunDot = saturate(sunDot - sunMask);
				float3 sunCol = lerp((0,0,0), _SunCol, finalsunDot);
				float mask = sunDot - finalsunDot;

				float2 equiUV = RadialCoords(i.worldpos);
				float4 lerpCol = lerp(_HorizonColour, _SkyColour, equiUV.y);

				float2 value = i.worldpos.xz / _CellSize;
				value.x *= _TilingX;
				value.y *= _TilingY;
				float noise = pow((1 - saturate(voronoiNoise(value))), 100);
				noise *= 1- sunDot;
				lerpCol += noise;
				lerpCol.xyz += sunCol;
				lerpCol.xyz += _DarkCol * mask * _DarkCol.a;

				float4 debug = (0, 0, 0, 1);
                return lerpCol;
            }
            ENDCG
        }
    }
}
