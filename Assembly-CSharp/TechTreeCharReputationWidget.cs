using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200095F RID: 2399
public class TechTreeCharReputationWidget : TechTreeElementBaseWidget
{
	// Token: 0x06003F43 RID: 16195 RVA: 0x0012F454 File Offset: 0x0012D654
	public override void Redraw()
	{
		base.Redraw();
		if (this.data.techDef.techDefType == TechDefType.CharRep)
		{
			WGODef dataOrNull = GameBalance.Me.GetDataOrNull<WGODef>(this.data.techDef.wgoRepLock.List[0].type);
			if (dataOrNull != null)
			{
				this.icon.sprite = dataOrNull.Portrait;
			}
			else
			{
				this.icon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.techDef.customIconId, null);
			}
		}
		else
		{
			this.icon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.data.techDef.customIconId, null);
		}
		this.icon.enabled = this.icon.sprite != null;
		this.idLabel.gameObject.SetActive(this.icon.sprite == null);
		this.icon.SetNativeSize();
		this.button.interactable = false;
		this.repLabel.transform.parent.gameObject.SetActive(true);
		this.icon.transform.parent.gameObject.SetActive(true);
		this.hiddenObj.SetActive(false);
		this.visibleObj.SetActive(false);
		this.availableObj.SetActive(false);
		this.unlockedObj.SetActive(false);
		TechState visualTechState = this.data.VisualTechState;
		if (this.data.techDef.EnoughResources)
		{
			this.repEnough.ApplyStyle(this.repLabel, false, null, null, null);
		}
		else
		{
			this.repNotEnough.ApplyStyle(this.repLabel, false, null, null, null);
		}
		if (this.data.techDef.techDefType == TechDefType.CharRep)
		{
			this.idLabel.text = this.data.techDef.CharReputationLock.List[0].type;
			this.repLabel.text = this.data.techDef.CharReputationLock.List[0].value.ToString();
		}
		else
		{
			this.idLabel.text = this.data.techDef.districtReputationLock.List[0].type;
			this.repLabel.text = this.data.techDef.districtReputationLock.List[0].value.ToString();
		}
		switch (visualTechState)
		{
		case TechState.Hidden:
			this.hiddenObj.SetActive(true);
			this.repLabel.transform.parent.gameObject.SetActive(false);
			this.icon.transform.parent.gameObject.SetActive(false);
			break;
		case TechState.Visible:
			this.visibleObj.SetActive(true);
			break;
		case TechState.Available:
			this.visibleObj.SetActive(true);
			break;
		case TechState.Unlocked:
			this.unlockedObj.SetActive(true);
			this.repLabel.transform.parent.gameObject.SetActive(false);
			break;
		}
		this.icon.BlueColorReplace(this.toReplace);
	}

	// Token: 0x06003F44 RID: 16196 RVA: 0x0012F7BB File Offset: 0x0012D9BB
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new TechTreeCharReputationWidgetData());
	}

	// Token: 0x040031CD RID: 12749
	[SerializeField]
	private Image icon;

	// Token: 0x040031CE RID: 12750
	[SerializeField]
	private TextMeshProUGUI idLabel;

	// Token: 0x040031CF RID: 12751
	[SerializeField]
	private TextMeshProUGUI repLabel;

	// Token: 0x040031D0 RID: 12752
	[SerializeField]
	private TextStyle repEnough;

	// Token: 0x040031D1 RID: 12753
	[SerializeField]
	private TextStyle repNotEnough;

	// Token: 0x040031D2 RID: 12754
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}
