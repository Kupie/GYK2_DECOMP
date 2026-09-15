using System;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class GroundLayoutObject : MonoBehaviour
{
	// Token: 0x0600214E RID: 8526 RVA: 0x0009D008 File Offset: 0x0009B208
	protected virtual void Redraw()
	{
		float num = 50f;
		float num2 = 0.96f;
		float num3 = num2 * (float)this.xSizeInTiles;
		float num4 = num2 * (float)this.zSizeInTiles;
		float num5 = num3 / 2f;
		float num6 = num4 / 2f;
		Sprite sprite = this.right.sprite;
		Sprite sprite2 = this.top.sprite;
		Sprite sprite3 = this.left.sprite;
		Sprite sprite4 = this.bot.sprite;
		Vector2 vector = new Vector2((float)sprite.texture.width, (float)sprite.texture.height) / num;
		Vector2 vector2 = new Vector2((float)sprite2.texture.width, (float)sprite2.texture.height) / num;
		Vector2 vector3 = new Vector2((float)sprite3.texture.width, (float)sprite3.texture.height) / num;
		Vector2 vector4 = new Vector2((float)sprite4.texture.width, (float)sprite4.texture.height) / num;
		vector *= new Vector2(1f, num4 / vector.y);
		vector2 *= new Vector2(num3 / vector2.x, 1f);
		vector3 *= new Vector2(1f, num4 / vector3.y);
		vector4 *= new Vector2(num3 / vector4.x, 1f);
		Vector3 vector5 = default(Vector3);
		Vector3 vector6 = default(Vector3);
		Vector3 vector7 = default(Vector3);
		Vector3 vector8 = default(Vector3);
		switch (this.growType)
		{
		case GroundLayoutObject.GrowType.Center:
			vector5 = new Vector3(num5, 0f, 0f);
			vector6 = new Vector3(0f, 0f, num6);
			vector7 = new Vector3(-num5, 0f, 0f);
			vector8 = new Vector3(0f, 0f, -num6);
			break;
		case GroundLayoutObject.GrowType.RightUp:
			vector5 = new Vector3(0f, 0f, -num6);
			vector6 = new Vector3(-num5, 0f, 0f);
			vector7 = new Vector3(-num3, 0f, -num6);
			vector8 = new Vector3(-num5, 0f, -num4);
			break;
		case GroundLayoutObject.GrowType.LeftUp:
			vector5 = new Vector3(num3, 0f, -num6);
			vector6 = new Vector3(num5, 0f, 0f);
			vector7 = new Vector3(0f, 0f, -num6);
			vector8 = new Vector3(num5, 0f, -num4);
			break;
		case GroundLayoutObject.GrowType.LeftDown:
			vector5 = new Vector3(num3, 0f, num6);
			vector6 = new Vector3(num5, 0f, num4);
			vector7 = new Vector3(0f, 0f, num6);
			vector8 = new Vector3(num5, 0f, 0f);
			break;
		case GroundLayoutObject.GrowType.RightDown:
			vector5 = new Vector3(0f, 0f, num6);
			vector6 = new Vector3(-num5, 0f, num4);
			vector7 = new Vector3(-num3, 0f, num6);
			vector8 = new Vector3(-num5, 0f, 0f);
			break;
		}
		Vector3 vector9 = new Vector3(1f, 1f, 1.25f);
		this.right.transform.localPosition = Vector3.Scale(vector5, vector9);
		this.top.transform.localPosition = Vector3.Scale(vector6, vector9);
		this.left.transform.localPosition = Vector3.Scale(vector7, vector9);
		this.bot.transform.localPosition = Vector3.Scale(vector8, vector9);
		this.right.size = vector;
		this.top.size = vector2;
		this.left.size = vector3;
		this.bot.size = vector4;
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x0009D3EB File Offset: 0x0009B5EB
	private void DecSizeX()
	{
		this.xSizeInTiles--;
		this.Redraw();
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x0009D401 File Offset: 0x0009B601
	private void IncSizeX()
	{
		this.xSizeInTiles++;
		this.Redraw();
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x0009D417 File Offset: 0x0009B617
	private void DecSizeY()
	{
		this.zSizeInTiles--;
		this.Redraw();
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x0009D42D File Offset: 0x0009B62D
	private void IncSizeY()
	{
		this.zSizeInTiles++;
		this.Redraw();
	}

	// Token: 0x04001DE0 RID: 7648
	[SerializeField]
	protected SpriteRenderer right;

	// Token: 0x04001DE1 RID: 7649
	[SerializeField]
	protected SpriteRenderer top;

	// Token: 0x04001DE2 RID: 7650
	[SerializeField]
	protected SpriteRenderer left;

	// Token: 0x04001DE3 RID: 7651
	[SerializeField]
	protected SpriteRenderer bot;

	// Token: 0x04001DE4 RID: 7652
	[Space]
	[SerializeField]
	protected GroundLayoutObject.GrowType growType;

	// Token: 0x04001DE5 RID: 7653
	[SerializeField]
	protected int xSizeInTiles = 64;

	// Token: 0x04001DE6 RID: 7654
	[SerializeField]
	protected int zSizeInTiles = 64;

	// Token: 0x02000505 RID: 1285
	protected enum GrowType
	{
		// Token: 0x04001DE8 RID: 7656
		Center,
		// Token: 0x04001DE9 RID: 7657
		RightUp,
		// Token: 0x04001DEA RID: 7658
		LeftUp,
		// Token: 0x04001DEB RID: 7659
		LeftDown,
		// Token: 0x04001DEC RID: 7660
		RightDown
	}
}
