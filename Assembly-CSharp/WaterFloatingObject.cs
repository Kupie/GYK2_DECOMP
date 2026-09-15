using System;
using UnityEngine;

// Token: 0x02000530 RID: 1328
[ExecuteInEditMode]
public class WaterFloatingObject : MonoBehaviour
{
	// Token: 0x06002221 RID: 8737 RVA: 0x000A04CE File Offset: 0x0009E6CE
	private static Vector2 Floor(Vector2 v)
	{
		return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x000A04EB File Offset: 0x0009E6EB
	private static Vector3 Floor(Vector3 v)
	{
		return new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
	}

	// Token: 0x06002223 RID: 8739 RVA: 0x000A0513 File Offset: 0x0009E713
	private static float Frac(float x)
	{
		return x - Mathf.Floor(x);
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000A051D File Offset: 0x0009E71D
	private static Vector3 Frac(Vector3 v)
	{
		return new Vector3(WaterFloatingObject.Frac(v.x), WaterFloatingObject.Frac(v.y), WaterFloatingObject.Frac(v.z));
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000A0545 File Offset: 0x0009E745
	private static Vector3 Abs(Vector3 v)
	{
		return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000A056D File Offset: 0x0009E76D
	private static Vector3 Max(Vector3 v, float val)
	{
		return new Vector3(Mathf.Max(v.x, val), Mathf.Max(v.y, val), Mathf.Max(v.z, val));
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x000A0598 File Offset: 0x0009E798
	private static Vector3 Mod289(Vector3 x)
	{
		return x - WaterFloatingObject.Floor(x * 0.0034602077f) * 289f;
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000A05BA File Offset: 0x0009E7BA
	private static Vector2 Mod289(Vector2 x)
	{
		return x - WaterFloatingObject.Floor(x * 0.0034602077f) * 289f;
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x000A05DC File Offset: 0x0009E7DC
	private static Vector3 Permute(Vector3 x)
	{
		return WaterFloatingObject.Mod289(Vector3.Scale(x * 34f + Vector3.one, x));
	}

	// Token: 0x0600222A RID: 8746 RVA: 0x000A0600 File Offset: 0x0009E800
	public static float Snoise(Vector2 v)
	{
		Vector4 vector = new Vector4(0.21132487f, 0.36602542f, -0.57735026f, 0.024390243f);
		Vector2 vector2 = WaterFloatingObject.Floor(v + Vector2.Dot(v, new Vector2(vector.y, vector.y)) * Vector2.one);
		Vector2 vector3 = v - vector2 + Vector2.Dot(vector2, new Vector2(vector.x, vector.x)) * Vector2.one;
		Vector2 vector4 = ((vector3.x > vector3.y) ? new Vector2(1f, 0f) : new Vector2(0f, 1f));
		Vector4 vector5 = new Vector4(vector3.x, vector3.y, vector3.x, vector3.y) + new Vector4(vector.x, vector.x, vector.z, vector.z);
		vector5.x -= vector4.x;
		vector5.y -= vector4.y;
		vector2 = WaterFloatingObject.Mod289(vector2);
		Vector3 vector6 = WaterFloatingObject.Permute(WaterFloatingObject.Permute(new Vector3(vector2.y + 0f, vector2.y + vector4.y, vector2.y + 1f) + Vector3.one * vector2.x + new Vector3(0f, vector4.x, 1f)));
		float num = Vector2.Dot(vector3, vector3);
		Vector2 vector7 = new Vector2(vector5.x, vector5.y);
		Vector2 vector8 = new Vector2(vector5.z, vector5.w);
		float num2 = Vector2.Dot(vector7, vector7);
		float num3 = Vector2.Dot(vector8, vector8);
		Vector3 vector9 = WaterFloatingObject.Max(new Vector3(0.5f - num, 0.5f - num2, 0.5f - num3), 0f);
		vector9 = Vector3.Scale(vector9, vector9);
		vector9 = Vector3.Scale(vector9, vector9);
		Vector3 vector10 = new Vector3(vector.w, vector.w, vector.w);
		Vector3 vector11 = 2f * WaterFloatingObject.Frac(Vector3.Scale(vector6, vector10)) - Vector3.one;
		Vector3 vector12 = WaterFloatingObject.Abs(vector11) - new Vector3(0.5f, 0.5f, 0.5f);
		Vector3 vector13 = WaterFloatingObject.Floor(vector11 + new Vector3(0.5f, 0.5f, 0.5f));
		Vector3 vector14 = vector11 - vector13;
		vector9 = Vector3.Scale(vector9, Vector3.one * 1.7928429f - 0.85373473f * (Vector3.Scale(vector14, vector14) + Vector3.Scale(vector12, vector12)));
		float num4 = vector14.x * vector3.x + vector12.x * vector3.y;
		float num5 = vector14.y * vector5.x + vector12.y * vector5.y;
		float num6 = vector14.z * vector5.z + vector12.z * vector5.w;
		Vector3 vector15 = new Vector3(num4, num5, num6);
		return 130f * Vector3.Dot(vector9, vector15);
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x000A0945 File Offset: 0x0009EB45
	private void Awake()
	{
		this.startPhase = global::UnityEngine.Random.Range(0f, this.amplitude);
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x000A0960 File Offset: 0x0009EB60
	private void Update()
	{
		float num = WaterFloatingObject.Snoise(new Vector2(base.transform.position.x, base.transform.position.z) * this.waveFrequency) + this.startPhase;
		base.transform.localPosition = this.amplitude * new Vector3(0f, Mathf.Sin(Time.time * this.waveSpeed + num), 0f);
	}

	// Token: 0x04001EBB RID: 7867
	public float amplitude = 0.04f;

	// Token: 0x04001EBC RID: 7868
	public float waveFrequency = 1f;

	// Token: 0x04001EBD RID: 7869
	public float waveSpeed = 1f;

	// Token: 0x04001EBE RID: 7870
	private float startPhase;
}
