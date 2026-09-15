using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200094B RID: 2379
public class QuestTreeConnector : MonoBehaviour
{
	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x0012BB73 File Offset: 0x00129D73
	private QuestTreeElementWidgetData ParentData
	{
		get
		{
			return this.parent.Data as QuestTreeElementWidgetData;
		}
	}

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x06003EC8 RID: 16072 RVA: 0x0012BB85 File Offset: 0x00129D85
	private QuestTreeElementWidgetData ChildData
	{
		get
		{
			return this.child.Data as QuestTreeElementWidgetData;
		}
	}

	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x0012BB97 File Offset: 0x00129D97
	public QuestTreeConnectorType ConnectorType
	{
		get
		{
			return this.connectorType;
		}
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x0012BBA0 File Offset: 0x00129DA0
	private void CalculateAndDisplayLine()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		Vector2 vector = this.ParentData.downConnectorPos;
		Vector2 vector2 = this.ChildData.upConnectorPos;
		Vector2Int treePos = this.ParentData.questData.Definition.TreePos;
		Vector2Int treePos2 = this.ChildData.questData.Definition.TreePos;
		if (treePos.x < treePos2.x)
		{
			if (treePos.y == treePos2.y)
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
		else if (treePos.x > treePos2.x)
		{
			if (treePos.y == treePos2.y)
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

	// Token: 0x06003ECB RID: 16075 RVA: 0x0012BFAF File Offset: 0x0012A1AF
	public void Draw(LazyScrollableElement parent, LazyScrollableElement child, bool isBackground)
	{
		this.parent = parent;
		this.child = child;
		this.isBackground = isBackground;
		this.UpdateState();
		this.CalculateAndDisplayLine();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x0012BFDE File Offset: 0x0012A1DE
	public void UpdateState()
	{
		this.UpdateTechState();
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x0012BFE8 File Offset: 0x0012A1E8
	public void UpdateTechState()
	{
		switch (this.ParentData.displayViewStatus)
		{
		case QuestViewStatus.Hidden:
			break;
		case QuestViewStatus.Unknown:
			this.angleDownLeft.SetupAsInactive(this.isBackground);
			this.angleDownRight.SetupAsInactive(this.isBackground);
			this.verticalDown.SetupAsInactive(this.isBackground);
			this.connectorType = QuestTreeConnectorType.Inactive;
			break;
		case QuestViewStatus.Visible:
			this.angleDownLeft.SetupAsInactive(this.isBackground);
			this.angleDownRight.SetupAsInactive(this.isBackground);
			this.verticalDown.SetupAsInactive(this.isBackground);
			this.connectorType = QuestTreeConnectorType.Inactive;
			break;
		case QuestViewStatus.Revealed:
			this.angleDownLeft.SetupAsInactive(this.isBackground);
			this.angleDownRight.SetupAsInactive(this.isBackground);
			this.verticalDown.SetupAsInactive(this.isBackground);
			this.connectorType = QuestTreeConnectorType.Inactive;
			break;
		case QuestViewStatus.Completed:
			this.angleDownLeft.SetupAsActive(this.isBackground);
			this.angleDownRight.SetupAsActive(this.isBackground);
			this.verticalDown.SetupAsActive(this.isBackground);
			this.connectorType = QuestTreeConnectorType.Active;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.angleDownLeft.image.type = Image.Type.Sliced;
		this.angleDownRight.image.type = Image.Type.Sliced;
		this.verticalDown.image.type = Image.Type.Sliced;
	}

	// Token: 0x0400314E RID: 12622
	[SerializeField]
	private QuestTreeConnectorElement angleDownLeft;

	// Token: 0x0400314F RID: 12623
	[SerializeField]
	private QuestTreeConnectorElement angleDownRight;

	// Token: 0x04003150 RID: 12624
	[SerializeField]
	private QuestTreeConnectorElement verticalDown;

	// Token: 0x04003151 RID: 12625
	[SerializeField]
	private QuestTreeConnectorElement horizontalLeft;

	// Token: 0x04003152 RID: 12626
	[SerializeField]
	private QuestTreeConnectorElement horizontalRight;

	// Token: 0x04003153 RID: 12627
	private LazyScrollableElement parent;

	// Token: 0x04003154 RID: 12628
	private LazyScrollableElement child;

	// Token: 0x04003155 RID: 12629
	private QuestTreeConnectorType connectorType;

	// Token: 0x04003156 RID: 12630
	private bool isBackground;
}
