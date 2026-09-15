using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A05 RID: 2565
public class UINotesWindow : LazyWindow<UINotesWindowData>
{
	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x06004510 RID: 17680 RVA: 0x00146AA9 File Offset: 0x00144CA9
	private Vector2 Direction
	{
		get
		{
			return LazyInput.GetDirection2();
		}
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x00146AB0 File Offset: 0x00144CB0
	public override void Init()
	{
		base.Init();
		this.scrollRect.vertical = true;
		this.scrollRect.horizontal = false;
		float num = ((this.textLabel != null && this.textLabel.fontSize > 0f) ? (this.textLabel.fontSize * 1.25f) : 20f);
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, num);
	}

	// Token: 0x06004512 RID: 17682 RVA: 0x00146B24 File Offset: 0x00144D24
	public override void Draw(UINotesWindowData data)
	{
		base.Draw(data);
		this.nameLabel.text = LLBase.L(data.NoteItem.id);
		this.textLabel.text = LLBase.L(data.NoteItem.id + "_note");
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		this.lazyButton.Draw(this.btnData);
		this.Fit();
	}

	// Token: 0x06004513 RID: 17683 RVA: 0x00146BC0 File Offset: 0x00144DC0
	private void Fit()
	{
		float num = this.root.rect.height * this.maxHeightPercent;
		float num2;
		this.textLabel.FitToAspectOrOverflowAndGetScrollHeight(this.aspectRatio, this.minHeight, num, out num2, 5f);
		if (num - this.textLabel.rectTransform.sizeDelta.y <= 10f)
		{
			this.scrollEnabled = true;
			this.scrollRect.verticalNormalizedPosition = 0f;
			this.scrollRect.gameObject.SetActive(true);
			Vector2 vector = new Vector2(this.textLabel.rectTransform.sizeDelta.x, num2);
			this.scrollRect.GetComponent<RectTransform>().sizeDelta = vector;
			this.textLabel.transform.SetParent(this.scrollRect.content);
			this.textLabel.rectTransform.anchoredPosition = new Vector2(this.textLabel.rectTransform.anchoredPosition.x, 0f);
		}
		else
		{
			this.scrollEnabled = false;
			this.scrollRect.gameObject.SetActive(false);
			this.textLabel.transform.SetParent(this.commonParent);
		}
		this.root.RefreshContentFitter();
	}

	// Token: 0x06004514 RID: 17684 RVA: 0x00146D04 File Offset: 0x00144F04
	protected override void Update()
	{
		base.Update();
		if (this.scrollEnabled && LazyInput.IsGamepadActive)
		{
			this.hSpeed = this.Direction.x * (Mathf.Abs(this.hSpeed) + 0.1f);
			this.ySpeed = this.Direction.y * (Mathf.Abs(this.ySpeed) + 0.1f);
			this.vPos = this.scrollRect.verticalNormalizedPosition + this.ySpeed * this.gamepadScrollSpeed;
			this.hPos = this.scrollRect.horizontalNormalizedPosition + this.hSpeed * this.gamepadScrollSpeed;
			this.ySpeed = Mathf.Lerp(this.ySpeed, 0f, 0.1f);
			this.hSpeed = Mathf.Lerp(this.hSpeed, 0f, 0.1f);
			if (this.scrollRect.movementType == ScrollRect.MovementType.Clamped)
			{
				this.vPos = Mathf.Clamp01(this.vPos);
				this.hPos = Mathf.Clamp01(this.hPos);
			}
			this.scrollRect.verticalNormalizedPosition = this.vPos;
			this.scrollRect.horizontalNormalizedPosition = this.hPos;
		}
	}

	// Token: 0x06004515 RID: 17685 RVA: 0x00146E38 File Offset: 0x00145038
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (this.scrollEnabled)
		{
			list.Add(new LazyGameKeyTip(GameKey.RightStickAsGameKey, "tip_scroll", true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06004516 RID: 17686 RVA: 0x00146E7C File Offset: 0x0014507C
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x06004517 RID: 17687 RVA: 0x00146EAE File Offset: 0x001450AE
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Open(new UINotesWindowData(GameBalance.Me.GetData<ItemDef>("test_note")));
	}

	// Token: 0x040035E0 RID: 13792
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x040035E1 RID: 13793
	[SerializeField]
	private TextMeshProUGUI textLabel;

	// Token: 0x040035E2 RID: 13794
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x040035E3 RID: 13795
	[SerializeField]
	private float maxHeightPercent = 0.8f;

	// Token: 0x040035E4 RID: 13796
	[SerializeField]
	private float aspectRatio = 1.7777778f;

	// Token: 0x040035E5 RID: 13797
	[SerializeField]
	private RectTransform root;

	// Token: 0x040035E6 RID: 13798
	[SerializeField]
	private float minHeight = 120f;

	// Token: 0x040035E7 RID: 13799
	[SerializeField]
	private RectTransform commonParent;

	// Token: 0x040035E8 RID: 13800
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x040035E9 RID: 13801
	[SerializeField]
	private float gamepadScrollSpeed = 0.03f;

	// Token: 0x040035EA RID: 13802
	private float ySpeed;

	// Token: 0x040035EB RID: 13803
	private float hSpeed;

	// Token: 0x040035EC RID: 13804
	private float hPos;

	// Token: 0x040035ED RID: 13805
	private float vPos;

	// Token: 0x040035EE RID: 13806
	private bool scrollEnabled;

	// Token: 0x040035EF RID: 13807
	private UIDialogWindowData.ButtonData btnData;
}
