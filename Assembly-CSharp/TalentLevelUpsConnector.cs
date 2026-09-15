using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000923 RID: 2339
public class TalentLevelUpsConnector : MonoBehaviour
{
	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x06003DAC RID: 15788 RVA: 0x001267EA File Offset: 0x001249EA
	private TalentLevelUpWidgetData ParentData
	{
		get
		{
			return this.parent.Data;
		}
	}

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x06003DAD RID: 15789 RVA: 0x001267F7 File Offset: 0x001249F7
	private TalentLevelUpWidgetData ChildData
	{
		get
		{
			return this.child.Data;
		}
	}

	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x06003DAE RID: 15790 RVA: 0x00126804 File Offset: 0x00124A04
	public TalentConnectorType TalentConnectorType
	{
		get
		{
			return this.connectorType;
		}
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x0012680C File Offset: 0x00124A0C
	private void CalculateAndDisplayLine()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		Vector2 vector = this.ParentData.downConnectorPos;
		Vector2 vector2 = this.ChildData.upConnectorPos;
		if (vector.x < vector2.x)
		{
			if (vector.y == vector2.y)
			{
				vector = this.ParentData.rightConnectorPos;
				vector2 = this.ChildData.leftConnectorPos;
				flag4 = true;
				float y = this.horizontalRight.RectTransform.sizeDelta.y;
				float num = vector2.x - vector.x;
				this.horizontalRight.RectTransform.sizeDelta = new Vector2(num, y) + this.horizontalRight.extraSize;
				base.transform.localPosition = this.ParentData.rightConnectorPos;
			}
			else
			{
				vector = this.ParentData.downConnectorPos;
				vector2 = this.ChildData.upConnectorPos;
				flag3 = true;
				float num2 = vector.y - vector2.y;
				float num3 = vector2.x - vector.x;
				this.angleDownRight.RectTransform.sizeDelta = new Vector2(num3, num2) + this.angleDownRight.extraSize;
				base.transform.localPosition = this.ParentData.downConnectorPos;
			}
		}
		else if (vector.x > vector2.x)
		{
			if (vector.y == vector2.y)
			{
				vector = this.ParentData.leftConnectorPos;
				vector2 = this.ChildData.rightConnectorPos;
				flag5 = true;
				float y2 = this.horizontalLeft.RectTransform.sizeDelta.y;
				float num4 = vector.x - vector2.x;
				this.horizontalLeft.RectTransform.sizeDelta = new Vector2(num4, y2) + this.horizontalLeft.extraSize;
				base.transform.localPosition = this.ParentData.leftConnectorPos;
			}
			else
			{
				vector = this.ParentData.downConnectorPos;
				vector2 = this.ChildData.upConnectorPos;
				flag2 = true;
				float num5 = vector.y - vector2.y;
				float num6 = vector.x - vector2.x;
				this.angleDownLeft.RectTransform.sizeDelta = new Vector2(num6, num5) + this.angleDownLeft.extraSize;
				base.transform.localPosition = this.ParentData.downConnectorPos;
			}
		}
		else
		{
			vector = this.ParentData.downConnectorPos;
			vector2 = this.ChildData.upConnectorPos;
			flag = true;
			float num7 = vector.y - vector2.y;
			this.verticalDown.RectTransform.sizeDelta = new Vector2(this.verticalDown.RectTransform.sizeDelta.x, num7) + this.verticalDown.extraSize;
			base.transform.localPosition = this.ParentData.downConnectorPos;
		}
		if (this.angleDownLeft.gameObject.activeSelf != flag2)
		{
			this.angleDownLeft.gameObject.SetActive(flag2);
		}
		if (this.angleDownRight.gameObject.activeSelf != flag3)
		{
			this.angleDownRight.gameObject.SetActive(flag3);
		}
		if (this.verticalDown.gameObject.activeSelf != flag)
		{
			this.verticalDown.gameObject.SetActive(flag);
		}
		if (this.horizontalLeft.gameObject.activeSelf != flag5)
		{
			this.horizontalLeft.gameObject.SetActive(flag5);
		}
		if (this.horizontalRight.gameObject.activeSelf != flag4)
		{
			this.horizontalRight.gameObject.SetActive(flag4);
		}
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00126BED File Offset: 0x00124DED
	public void Draw(TalentLevelUpWidget parent, TalentLevelUpWidget child, bool isBackground)
	{
		this.parent = parent;
		this.child = child;
		this.isBackground = isBackground;
		this.UpdateState();
		this.CalculateAndDisplayLine();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x00126C1C File Offset: 0x00124E1C
	public void UpdateState()
	{
		this.UpdateTechState();
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x00126C24 File Offset: 0x00124E24
	public void UpdateTechState()
	{
		switch (this.ParentData.State)
		{
		case TalentLevelUpDef.State.Unknown:
			this.angleDownLeft.SetupAsInactive(this.isBackground);
			this.angleDownRight.SetupAsInactive(this.isBackground);
			this.verticalDown.SetupAsInactive(this.isBackground);
			this.connectorType = TalentConnectorType.Inactive;
			break;
		case TalentLevelUpDef.State.Hidden:
			break;
		case TalentLevelUpDef.State.Visible:
			this.angleDownLeft.SetupAsInactive(this.isBackground);
			this.angleDownRight.SetupAsInactive(this.isBackground);
			this.verticalDown.SetupAsInactive(this.isBackground);
			this.connectorType = TalentConnectorType.Inactive;
			break;
		case TalentLevelUpDef.State.Available:
			this.angleDownLeft.SetupAsInactive(this.isBackground);
			this.angleDownRight.SetupAsInactive(this.isBackground);
			this.verticalDown.SetupAsInactive(this.isBackground);
			this.connectorType = TalentConnectorType.Inactive;
			break;
		case TalentLevelUpDef.State.Unlocked:
			this.angleDownLeft.SetupAsActive(this.isBackground);
			this.angleDownRight.SetupAsActive(this.isBackground);
			this.verticalDown.SetupAsActive(this.isBackground);
			this.connectorType = TalentConnectorType.Active;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.angleDownLeft.image.type = Image.Type.Sliced;
		this.angleDownRight.image.type = Image.Type.Sliced;
		this.verticalDown.image.type = Image.Type.Sliced;
	}

	// Token: 0x0400306F RID: 12399
	[SerializeField]
	private TalentLevelUpsConnectorElement angleDownLeft;

	// Token: 0x04003070 RID: 12400
	[SerializeField]
	private TalentLevelUpsConnectorElement angleDownRight;

	// Token: 0x04003071 RID: 12401
	[SerializeField]
	private TalentLevelUpsConnectorElement verticalDown;

	// Token: 0x04003072 RID: 12402
	[SerializeField]
	private TalentLevelUpsConnectorElement horizontalLeft;

	// Token: 0x04003073 RID: 12403
	[SerializeField]
	private TalentLevelUpsConnectorElement horizontalRight;

	// Token: 0x04003074 RID: 12404
	private TalentLevelUpWidget parent;

	// Token: 0x04003075 RID: 12405
	private TalentLevelUpWidget child;

	// Token: 0x04003076 RID: 12406
	private TalentConnectorType connectorType;

	// Token: 0x04003077 RID: 12407
	private bool isBackground;
}
