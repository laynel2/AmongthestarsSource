Shader ".MyShaders/ToonNR" {
        Properties {
            _MainTex ("Texture", 2D) = "white" {}
            _CelShade("Cel shading amount", float ) = 1
            _NormTex ("Normal", 2D) = "bump" {}
            _Test ("Test", float) = 0.5
            _NormalIn ("Normal Intensity", Range (0,8)) = 1
			_Colour ("Colour", Color) = (1,1,1,1)
        }
        SubShader {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
          #pragma surface surf Ramp

          sampler2D _Ramp;
           sampler2D _NormTex;
          float _CelShade;
          float _Test;
          float _NormalIn;

          half4 LightingRamp (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
          {
            
          	half dotNL = dot (s.Normal, lightDir);
          	half cel = floor(dotNL * _CelShade) / (_CelShade - _Test);

          	half4 c;
          	c.rgb = s.Albedo * _LightColor0.rgb * cel * atten;

          	c.a = s.Alpha;
          	return c;
          }

          struct Input{
          float2 uv_MainTex;
          float2 uv_NormTex;
          };
        
        sampler2D _MainTex;
		float4 _Colour;

        
        void surf (Input IN, inout SurfaceOutput o) {
        float3 Nmap = UnpackNormal (tex2D(_NormTex, IN.uv_NormTex)).rgb;

        Nmap.x *= _NormalIn;
        Nmap.y *= _NormalIn;

        	o.Normal = normalize (Nmap);
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Colour;
        }
        ENDCG
        }
        Fallback "Diffuse"
    }