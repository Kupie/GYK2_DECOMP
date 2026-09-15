using System;

// Token: 0x020006B8 RID: 1720
public static class LazyTerrainMath
{
	// Token: 0x06002DBD RID: 11709 RVA: 0x000DAEEC File Offset: 0x000D90EC
	public static uint FloatToUint(float value)
	{
		return BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x000DAEFA File Offset: 0x000D90FA
	public static float UintToFloat(uint value)
	{
		return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x000DAF08 File Offset: 0x000D9108
	public static float PackShaderValue(int x, int y, int textureId)
	{
		return (float)x + (float)y * 100f + (float)textureId * 10000f + 1f;
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x000DAF24 File Offset: 0x000D9124
	public static void UnpackShaderValue(float v, out int x, out int y, out int textureId)
	{
		textureId = (int)(v / 10000f);
		y = (int)(v % 10000f / 100f);
		x = (int)(v % 100f);
	}
}
