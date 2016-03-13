sampler s0;
float fFillAmount;

///<summery>
/// Draws while colling down item
///</summery>
float4 CoolDown(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	// If the pixel isn't transparant
	if (color.a && coords.y >= fFillAmount)
	{
		color.rgb -= 0.2;
	}

    return (color);
}

///<summery>
/// Draws while player doesn't have enough mana
///</summery>
float4 NotEnoughMana(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	// If the pixel isn't transparant
	if (color.a)
	{
		color.b += 0.2;
	}

    return (color);
}

technique Technique1
{
    pass CoolDown
    {
        PixelShader = compile ps_2_0 CoolDown();
    }

	pass NotEnoughMana
    {
        PixelShader = compile ps_2_0 NotEnoughMana();
    }
}
