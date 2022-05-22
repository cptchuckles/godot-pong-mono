shader_type canvas_item;

void fragment()
{
	vec4 col = texture(TEXTURE, UV);
	vec2 posFromCenter = UV - vec2(0.5, 0.5);
	if(length(posFromCenter) > 0.5) col.a = 0.0;
	COLOR = col;
}