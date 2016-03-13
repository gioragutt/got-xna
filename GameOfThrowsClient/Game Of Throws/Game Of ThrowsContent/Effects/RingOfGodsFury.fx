float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.

sampler s0;

///<summery>
/// Draws a negative
///</summery>
float4 Negative(float2 coords : TEXCOORD0) : COLOR0
{
    return (1 - tex2D(s0, coords));
}

technique Technique1
{
    pass Negative
    {
        PixelShader = compile ps_2_0 Negative();
    }
}
