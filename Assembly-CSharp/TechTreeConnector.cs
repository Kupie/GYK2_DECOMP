using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000954 RID: 2388
public class TechTreeConnector : MonoBehaviour
{
	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06003EFC RID: 16124 RVA: 0x0012D379 File Offset: 0x0012B579
	private TreeElementBaseWidgetData ParentData
	{
		get
		{
			return this.parent.Data as TreeElementBaseWidgetData;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06003EFD RID: 16125 RVA: 0x0012D38B File Offset: 0x0012B58B
	private TreeElementBaseWidgetData ChildData
	{
		get
		{
			return this.child.Data as TreeElementBaseWidgetData;
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06003EFE RID: 16126 RVA: 0x0012D39D File Offset: 0x0012B59D
	public bool IsConnectorActive
	{
		get
		{
			return this.isConnectorActive;
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x06003EFF RID: 16127 RVA: 0x0012D3A5 File Offset: 0x0012B5A5
	public bool IsBackground
	{
		get
		{
			return this.isBackground;
		}
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x0012D3B0 File Offset: 0x0012B5B0
	private void CalculateAndDisplayLine()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		Vector2 vector = this.ParentData.rightConnectorPos;
		Vector2 vector2 = this.ChildData.leftConnectorPos;
		if (vector.x == vector2.x)
		{
			if (vector.y < vector2.y)
			{
				vector = this.ParentData.upConnectorPos;
				vector2 = this.ChildData.downConnectorPos;
				flag3 = true;
				float num = vector2.y - vector.y;
				this.verticalUp.RectTransform.sizeDelta = new Vector2(this.verticalUp.RectTransform.sizeDelta.x, num) + this.verticalUp.extraSize;
				base.transform.localPosition = this.ParentData.upConnectorPos;
			}
			else if (vector.y > vector2.y)
			{
				vector = this.ParentData.downConnectorPos;
				vector2 = this.ChildData.upConnectorPos;
				flag4 = true;
				float num2 = vector.y - vector2.y;
				this.verticalDown.RectTransform.sizeDelta = new Vector2(this.verticalDown.RectTransform.sizeDelta.x, num2) + this.verticalDown.extraSize;
				base.transform.localPosition = this.ParentData.downConnectorPos;
			}
		}
		else
		{
			if (vector.y == vector2.y)
			{
				flag5 = true;
				this.horizontal.RectTransform.sizeDelta = new Vector2(vector2.x - vector.x, this.horizontal.RectTransform.sizeDelta.y);
			}
			else if (vector.y < vector2.y)
			{
				flag = true;
				float num3 = Mathf.Abs(vector2.y - vector.y);
				float num4 = vector2.x - vector.x;
				this.angleDownUp.RectTransform.sizeDelta = new Vector2(num4, num3) + this.angleDownUp.extraSize;
			}
			else if (vector.y > vector2.y)
			{
				flag2 = true;
				float num5 = Mathf.Abs(vector.y - vector2.y);
				float num6 = vector2.x - vector.x;
				this.angleUpDown.RectTransform.sizeDelta = new Vector2(num6, num5) + this.angleUpDown.extraSize;
			}
			base.transform.localPosition = this.ParentData.rightConnectorPos;
		}
		if (this.angleDownUp.gameObject.activeSelf != flag)
		{
			this.angleDownUp.gameObject.SetActive(flag);
		}
		if (this.verticalUp.gameObject.activeSelf != flag3)
		{
			this.verticalUp.gameObject.SetActive(flag3);
		}
		if (this.verticalDown.gameObject.activeSelf != flag4)
		{
			this.verticalDown.gameObject.SetActive(flag4);
		}
		if (this.angleUpDown.gameObject.activeSelf != flag2)
		{
			this.angleUpDown.gameObject.SetActive(flag2);
		}
		if (this.horizontal.gameObject.activeSelf != flag5)
		{
			this.horizontal.gameObject.SetActive(flag5);
		}
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x0012D718 File Offset: 0x0012B918
	public void Draw(LazyScrollableElement parent, LazyScrollableElement child, bool isBackground)
	{
		this.isBackground = isBackground;
		this.parent = parent;
		this.child = child;
		this.useRepWidgetSourceSprites = parent.Data is TechTreeCharReputationWidgetData;
		this.UpdateState();
		this.CalculateAndDisplayLine();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x0012D766 File Offset: 0x0012B966
	public void UpdateState()
	{
		if (this.ParentData.techDef != null)
		{
			this.UpdateTechState();
			return;
		}
		this.UpdateQuestState();
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x0012D784 File Offset: 0x0012B984
	public void UpdateTechState()
	{
		switch (this.ParentData.techDef.TechState)
		{
		case TechState.Hidden:
			this.angleDownUp.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.angleUpDown.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.horizontal.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalUp.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalDown.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.isConnectorActive = false;
			break;
		case TechState.Visible:
			this.angleDownUp.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.angleUpDown.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.horizontal.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalUp.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalDown.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.isConnectorActive = false;
			break;
		case TechState.Available:
			this.angleDownUp.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.angleUpDown.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.horizontal.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalUp.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalDown.SetupAsInactive(this.isBackground, this.useRepWidgetSourceSprites);
			this.isConnectorActive = false;
			break;
		case TechState.Unlocked:
			this.angleDownUp.SetupAsActive(this.isBackground, this.useRepWidgetSourceSprites);
			this.angleUpDown.SetupAsActive(this.isBackground, this.useRepWidgetSourceSprites);
			this.horizontal.SetupAsActive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalUp.SetupAsActive(this.isBackground, this.useRepWidgetSourceSprites);
			this.verticalDown.SetupAsActive(this.isBackground, this.useRepWidgetSourceSprites);
			this.isConnectorActive = true;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.angleDownUp.image.type = Image.Type.Sliced;
		this.angleUpDown.image.type = Image.Type.Sliced;
		this.horizontal.image.type = Image.Type.Sliced;
		this.verticalUp.image.type = Image.Type.Sliced;
		this.verticalDown.image.type = Image.Type.Sliced;
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x0012DA14 File Offset: 0x0012BC14
	public void UpdateQuestState()
	{
		switch (this.ParentData.questData.ViewStatus)
		{
		case QuestViewStatus.Visible:
			this.angleDownUp.SetupAsActive(this.isBackground, false);
			this.angleUpDown.SetupAsActive(this.isBackground, false);
			this.horizontal.SetupAsActive(this.isBackground, false);
			this.verticalUp.SetupAsActive(this.isBackground, false);
			this.verticalDown.SetupAsActive(this.isBackground, false);
			this.isConnectorActive = false;
			break;
		case QuestViewStatus.Revealed:
			this.angleDownUp.SetupAsActive(this.isBackground, false);
			this.angleUpDown.SetupAsActive(this.isBackground, false);
			this.horizontal.SetupAsActive(this.isBackground, false);
			this.verticalUp.SetupAsActive(this.isBackground, false);
			this.verticalDown.SetupAsActive(this.isBackground, false);
			this.isConnectorActive = false;
			break;
		case QuestViewStatus.Completed:
			this.angleDownUp.SetupAsActive(this.isBackground, false);
			this.angleUpDown.SetupAsActive(this.isBackground, false);
			this.horizontal.SetupAsActive(this.isBackground, false);
			this.verticalUp.SetupAsActive(this.isBackground, false);
			this.verticalDown.SetupAsActive(this.isBackground, false);
			this.isConnectorActive = true;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.angleDownUp.image.type = Image.Type.Sliced;
		this.angleUpDown.image.type = Image.Type.Sliced;
		this.horizontal.image.type = Image.Type.Sliced;
		this.verticalUp.image.type = Image.Type.Sliced;
		this.verticalDown.image.type = Image.Type.Sliced;
	}

	// Token: 0x04003189 RID: 12681
	[SerializeField]
	private TechTreeConnectorElement angleDownUp;

	// Token: 0x0400318A RID: 12682
	[SerializeField]
	private TechTreeConnectorElement angleUpDown;

	// Token: 0x0400318B RID: 12683
	[SerializeField]
	private TechTreeConnectorElement horizontal;

	// Token: 0x0400318C RID: 12684
	[SerializeField]
	private TechTreeConnectorElement verticalDown;

	// Token: 0x0400318D RID: 12685
	[SerializeField]
	private TechTreeConnectorElement verticalUp;

	// Token: 0x0400318E RID: 12686
	private LazyScrollableElement parent;

	// Token: 0x0400318F RID: 12687
	private LazyScrollableElement child;

	// Token: 0x04003190 RID: 12688
	private bool isConnectorActive;

	// Token: 0x04003191 RID: 12689
	private bool isBackground;

	// Token: 0x04003192 RID: 12690
	private bool useRepWidgetSourceSprites;
}
