Shader "Hidden/ScreenSpaceCloudShadow"
{
    Properties
    {
		_CloudTexture ("Cloud Texture", 2D) = "white" {}
		_CloudFactor ("Cloud Factor (Speed, Scale, Cover, Alpha)", Vector) = (0.01, 0.05, 0.1, 5)
		_ShadowStrength ("Shadow Strength", Float) = 1.0
		_ShadowColor ("Shadow Color", Color) = (0.0, 0.0, 0.0, 1.0)
    }
    SubShader
    {
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"ForceNoShadowCasting"="True"
		}

        // No culling or depth
        Cull Off ZWrite Off ZTest Always
		Blend Zero OneMinusSrcColor

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 ray : TEXCOORD1;
            };

			sampler2D _CloudTexture;
			sampler2D _CameraDepthTexture;
			uniform fixed4 _CloudFactor; //speed, scale, cover, alpha
			uniform fixed _ShadowStrength;
			uniform fixed4 _ShadowColor;

            v2f vert (a2v v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				float3 pos_world = mul(unity_ObjectToWorld, v.vertex);
				o.ray = pos_world - _WorldSpaceCameraPos.xyz;

                return o;
            }

			float2 hash(float2 p)
			{
				p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5,183.3)));
				return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
			}

			float noise(in float2 p)
			{
				const float K1 = 0.366025404; // (sqrt(3)-1)/2;
				const float K2 = 0.211324865; // (3-sqrt(3))/6;
				float2 i = floor(p + (p.x + p.y) * K1);	
				float2 a = p - i + (i.x + i.y) * K2;
				float2 o = (a.x > a.y) ? float2(1.0, 0.0) : float2(0.0, 1.0); //float2 of = 0.5 + 0.5*float2(sign(a.x-a.y), sign(a.y-a.x));
				float2 b = a - o + K2;
				float2 c = a - 1.0 + 2.0 * K2;
				float3 h = max(0.5 - float3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
				float3 n = h * h * h * h * float3(dot(a, hash(i)), dot(b, hash(i + o)), dot(c, hash(i + 1.0)));
				return dot(n, float3(70.0, 70.0, 70.0));	
			}

			float fbm(float2 n)
			{
				const float2x2 m = float2x2(1.6,  1.2, -1.2, 1.6);
				float total = 0.0, amplitude = 0.1;
				for (int i = 0; i < 7; i++) {
					total += noise(n) * amplitude;
					n = mul(m, n);
					amplitude *= 0.4;
				}
				return total;
			}
            
            float getCloudStrength(float2 texcoord)
            {
				const float2x2 m = float2x2( 1.6,  1.2, -1.2,  1.6 );
				float cloudSpeed = _CloudFactor.x;
				float cloudScale = _CloudFactor.y;
				float cloudCover = _CloudFactor.z;
				float cloudAlpha = _CloudFactor.w;
				float2 p = texcoord;
				float2 uv = p;
				float2 uv_delta = cloudSpeed * _Time.y;
				float q = fbm(uv * cloudScale * 0.5);
    
				//ridged noise shape
				float r = 0.0;
				uv *= cloudScale;
				uv -= q - uv_delta;
				float weight = 0.8;
				for (int i = 0; i < 4; i++)
				{
					r += abs(weight * noise(uv));
					uv = mul(m, uv) + uv_delta;
					weight *= 0.7;
				}
    
				//noise shape
				float f = 0.0;
				uv = p;
				uv *= cloudScale;
				uv -= q - uv_delta;
				weight = 0.7;
				for (int i = 0; i < 8; i++)
				{
					f += weight * noise(uv);
					uv = mul(m, uv) + uv_delta;
					weight *= 0.6;
				}
    
				f *= r + f;
    
				//noise colour
				float c = 0.0;
				uv_delta = uv_delta * 2.0;
				uv = p;
				uv *= cloudScale * 2.0;
				uv -= q - uv_delta;
				weight = 0.4;
				for (int i = 0; i < 7; i++)
				{
					c += weight * noise(uv);
					uv = mul(m, uv) + uv_delta;
					weight *= 0.6;
				}
    
				//noise ridge colour
				float c1 = 0.0;
				uv_delta = uv_delta * 3.0;
				uv = p;
				uv *= cloudScale * 3.0;
				uv -= q - uv_delta;
				weight = 0.4;
				for (int i=0; i<7; i++)
				{
					c1 += abs(weight * noise(uv));
					uv = mul(m, uv) + uv_delta;
					weight *= 0.6;
				}
	
				c += c1;   
				f = cloudCover + cloudAlpha * f * r;
				f = saturate(f + c);

				return f;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));
				float3 pos_world = i.ray * depth + _WorldSpaceCameraPos.xyz;
				float cloudSpeed = _CloudFactor.x;
				float cloudScale = _CloudFactor.y;
				float cloudCover = _CloudFactor.z;
				float cloudAlpha = _CloudFactor.w;

				//fixed2 uv_cloud = pos_world.xz;
				//fixed cloudStrength = getCloudStrength(uv_cloud);
				fixed2 uv_cloud = pos_world.xz * cloudScale + cloudSpeed * _Time.y;
				fixed cloudStrength = tex2D(_CloudTexture, uv_cloud);
				cloudStrength = cloudCover + cloudAlpha * cloudStrength;
				cloudStrength *= (1 - depth);

				fixed shadowStrength = _ShadowStrength;
				shadowStrength *= cloudStrength;
				shadowStrength = saturate(shadowStrength);

                fixed4 shadowCol = _ShadowColor;
				shadowCol = 1 - shadowCol;
				shadowCol *= shadowStrength;
                shadowCol.a = 1;
                return shadowCol;
            }
            ENDCG
        }
    }
}
