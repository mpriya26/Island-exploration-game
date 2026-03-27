shader_type spatial;
render_mode world_vertex_coords;

// https://github.com/MaxBittker/glsl-voronoi-noise
vec3 hash(vec3 p) {
  return fract(sin(vec3(dot(p, vec3(1.0, 57.0, 113.0)),
                        dot(p, vec3(57.0, 113.0, 1.0)),
                        dot(p, vec3(113.0, 1.0, 57.0)))) *
               43758.5453);
}

vec3 voronoi3d(const in vec3 x) {
  vec3 p = floor(x);
  vec3 f = fract(x);

  float id = 0.0;
  vec2 res = vec2(100.0);
  for (int k = -1; k <= 1; k++) {
    for (int j = -1; j <= 1; j++) {
      for (int i = -1; i <= 1; i++) {
        vec3 b = vec3(float(i), float(j), float(k));
        vec3 r = vec3(b) - f + hash(p + b);
        float d = dot(r, r);

        float cond = max(sign(res.x - d), 0.0);
        float nCond = 1.0 - cond;

        float cond2 = nCond * max(sign(res.y - d), 0.0);
        float nCond2 = 1.0 - cond2;

        id = (dot(p + b, vec3(1.0, 57.0, 113.0)) * cond) + (id * nCond);
        res = vec2(d, res.x) * cond + res * nCond;

        res.y = cond2 * d + nCond2 * res.y;
      }
    }
  }

  return vec3(sqrt(res), abs(id));
}


const mat2 myt = mat2(vec2(.12121212, .13131313), vec2(-.13131313, .12121212));
const vec2 mys = vec2(1e4, 1e6);


vec2 rhash(vec2 uv) {
  uv *= myt;
  uv *= mys;
  return fract(fract(uv / mys) * uv);
}


float voronoi2d(const in vec2 point) {
  vec2 p = floor(point);
  vec2 f = fract(point);
  float res = 0.0;
  for (int j = -1; j <= 1; j++) {
    for (int i = -1; i <= 1; i++) {
      vec2 b = vec2(float(i), float(j));
      vec2 r = vec2(b) - f + rhash(p + b);
      res += 1. / pow(dot(r, r), 8.);
    }
  }
  return pow(1. / res, 0.0625);
}


uniform float waveHeightScale = 0.5;
uniform float scale = 0.1;
uniform float size = 500.0;
uniform float speed = 1.0;
uniform vec3 color : source_color;
uniform vec3 darkColor : source_color;
uniform vec3 foamColor : source_color;

uniform float fractalMult = 0.1;
uniform float fractalScale = 0.1;
uniform int fractalLayers = 1;

uniform sampler2D uvNoise;
uniform sampler2D foam;
uniform float foamThreshold;



vec2 calcPos(in vec3 worldPos, in vec2 uv)
{
	return worldPos.xz + (size * (((uv + (0.01 * texture(uvNoise, uv).xy)) * 2.0) - 1.0));
}



float fractalWave(in vec2 uv, in vec2 pos, float fScale)
{
	return pow(voronoi3d(vec3(fScale * pos, (5.0 * texture(foam, uv).x) + (speed * TIME))).x, 2.0);
	//return pow(voronoi2d(vec2(fScale * pos)), 2.0);
}



float wave(in vec2 uv, in vec2 pos, float startScale, float fMult, int frac)
{
	float res = 0.0;
	float fScale = startScale;
	float height = 1.0;
	
	for (int i = 0; i < frac; i++)
	{
		res += height * fractalWave(uv, pos, fScale);
		fScale *= fMult;
		height *= fractalScale;
	}
	
	return waveHeightScale * res;
}



vec3 waveHeight(in vec2 uv, in vec2 pos)
{
	return vec3(pos.x, wave(uv, pos, scale, fractalMult, fractalLayers), pos.y);
}



void vertex()
{
	VERTEX = waveHeight(UV, calcPos(NODE_POSITION_WORLD, UV));
}



void fragment()
{
	vec3 wHeight = waveHeight(UV, calcPos(NODE_POSITION_WORLD, UV));
	float wFactor = wHeight.y;
	
	vec3 dx = dFdx(wHeight);
	vec3 dy = dFdy(wHeight);
	vec3 worldNormal = normalize(cross(dy, dx));
	
	ALBEDO = mix(darkColor, color, wFactor);
	NORMAL = (VIEW_MATRIX * vec4(worldNormal, 0.0)).xyz;
	
	ROUGHNESS = 0.1;
}
