using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000858 RID: 2136
[DefaultExecutionOrder(1000)]
public class UITutorialArrow : LazySingleton<UITutorialArrow>, ILazyGUIElement
{
	// Token: 0x060036BD RID: 14013 RVA: 0x00109178 File Offset: 0x00107378
	public void Init()
	{
		base.gameObject.SetActive(false);
		MainGame.OnGoToMainMenu = (Action)Delegate.Combine(MainGame.OnGoToMainMenu, new Action(this.UnAttach));
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = 51;
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x001091CA File Offset: 0x001073CA
	public void Attach(WgoData wgo)
	{
		if (wgo == null)
		{
			return;
		}
		this.wgoData = wgo;
		base.gameObject.SetActive(true);
		MainGame.PlayerData.tutorialArrowWgoId = this.wgoData.UniqueId;
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x001091F8 File Offset: 0x001073F8
	public void Attach(string wgoCustomTag)
	{
		this.Attach(MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(wgoCustomTag));
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x00109215 File Offset: 0x00107415
	public void UnAttach()
	{
		MainGame.PlayerData.tutorialArrowWgoId = null;
		this.wgoData = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x00109238 File Offset: 0x00107438
	private void LateUpdate()
	{
		Vector3 bubblePos = this.wgoData.BubblePos;
		base.transform.position = CameraSystem.WorldToScreenPoint(bubblePos);
		Vector2 vector = base.transform.localPosition;
		Vector2 vector2 = vector;
		this.width = (float)Screen.width / this.widthScreenDivider;
		this.height = (float)Screen.height / this.heightScreenDivider;
		bool flag = false;
		if (vector.x > this.width)
		{
			vector.x = this.width;
			flag = true;
		}
		else if (vector.x < -this.width)
		{
			vector.x = -this.width;
			flag = true;
		}
		if (vector.y > this.height)
		{
			vector.y = this.height;
			flag = true;
		}
		else if (vector.y < -this.height)
		{
			vector.y = -this.height;
			flag = true;
		}
		if (flag)
		{
			base.transform.localPosition = vector;
			base.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(vector2.y, vector2.x) * 57.29578f + 90f);
		}
		else
		{
			base.transform.localRotation = Quaternion.identity;
		}
		this.insideObj.localPosition = new Vector3(0f, Mathf.Sin(Time.time * this.timeK) * this.timeK2 + this.timeK3);
	}

	// Token: 0x04002BA7 RID: 11175
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04002BA8 RID: 11176
	[SerializeField]
	private Transform insideObj;

	// Token: 0x04002BA9 RID: 11177
	[SerializeField]
	private float timeK = 7f;

	// Token: 0x04002BAA RID: 11178
	[SerializeField]
	private float timeK2 = 4f;

	// Token: 0x04002BAB RID: 11179
	[SerializeField]
	private float timeK3 = 4f;

	// Token: 0x04002BAC RID: 11180
	[SerializeField]
	private float widthScreenDivider = 6.6f;

	// Token: 0x04002BAD RID: 11181
	[SerializeField]
	private float heightScreenDivider = 6.4f;

	// Token: 0x04002BAE RID: 11182
	private float width;

	// Token: 0x04002BAF RID: 11183
	private float height;

	// Token: 0x04002BB0 RID: 11184
	private WgoData wgoData;
}
