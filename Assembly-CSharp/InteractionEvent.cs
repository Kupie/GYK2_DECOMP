using System;
using System.Linq;

// Token: 0x02000593 RID: 1427
[Serializable]
public class InteractionEvent
{
	// Token: 0x060024C1 RID: 9409 RVA: 0x000AC894 File Offset: 0x000AAA94
	public InteractionEvent(string str, bool isFake = false)
	{
		this.str = str;
		this.isFake = isFake;
		string text = str.Split('_', StringSplitOptions.None).Last<string>();
		InteractionEvent.Type type;
		if (!(text == "talk"))
		{
			if (!(text == "pray"))
			{
				if (!(text == "reward"))
				{
					if (!(text == "fishing"))
					{
						if (!(text == "work"))
						{
							type = InteractionEvent.Type.Talk;
						}
						else
						{
							type = InteractionEvent.Type.Work;
						}
					}
					else
					{
						type = InteractionEvent.Type.Fishing;
					}
				}
				else
				{
					type = InteractionEvent.Type.Reward;
				}
			}
			else
			{
				type = InteractionEvent.Type.Pray;
			}
		}
		else
		{
			type = InteractionEvent.Type.Talk;
		}
		this.type = type;
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x060024C2 RID: 9410 RVA: 0x000AC924 File Offset: 0x000AAB24
	public string CustomIcon
	{
		get
		{
			string text;
			switch (this.type)
			{
			case InteractionEvent.Type.None:
				text = string.Empty;
				break;
			case InteractionEvent.Type.Talk:
				text = "icon_speech_bubble";
				break;
			case InteractionEvent.Type.Pray:
				text = "icon_pray_bubble";
				break;
			case InteractionEvent.Type.Reward:
				text = "icon_coins_bubble";
				break;
			case InteractionEvent.Type.Fishing:
				text = "icon_fishing_bubble";
				break;
			case InteractionEvent.Type.Work:
				text = "icon_view_bubble";
				break;
			default:
				text = string.Empty;
				break;
			}
			return text;
		}
	}

	// Token: 0x0400206D RID: 8301
	public InteractionEvent.Type type;

	// Token: 0x0400206E RID: 8302
	public string str;

	// Token: 0x0400206F RID: 8303
	public bool isFake;

	// Token: 0x02000594 RID: 1428
	public enum Type
	{
		// Token: 0x04002071 RID: 8305
		None,
		// Token: 0x04002072 RID: 8306
		Talk,
		// Token: 0x04002073 RID: 8307
		Pray,
		// Token: 0x04002074 RID: 8308
		Reward,
		// Token: 0x04002075 RID: 8309
		Fishing,
		// Token: 0x04002076 RID: 8310
		Work
	}
}
