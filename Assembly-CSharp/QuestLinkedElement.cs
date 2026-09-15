using System;

// Token: 0x020004AE RID: 1198
[Serializable]
public class QuestLinkedElement : IAutoParsable
{
	// Token: 0x06001FF5 RID: 8181 RVA: 0x00097638 File Offset: 0x00095838
	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.id))
		{
			return string.Empty;
		}
		string text = this.id;
		if (this.count != 0)
		{
			text += string.Format("={0}", this.count);
		}
		text = "[" + text + "]";
		switch (this.type)
		{
		case QuestElementInfoType.Item:
			text = "item" + text;
			break;
		case QuestElementInfoType.Building:
			text = "building" + text;
			break;
		case QuestElementInfoType.Craft:
			text = "craft" + text;
			break;
		case QuestElementInfoType.GameRes:
			text = "gameres" + text;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return text;
	}

	// Token: 0x04001CA5 RID: 7333
	public QuestElementInfoType type;

	// Token: 0x04001CA6 RID: 7334
	public string id;

	// Token: 0x04001CA7 RID: 7335
	public int count;
}
