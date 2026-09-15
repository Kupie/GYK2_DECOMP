using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C30 RID: 3120
	[Name("Pay Mercenary Price", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_MercenariesPrice : GKCustomFlowNode
	{
		// Token: 0x06004FB1 RID: 20401 RVA: 0x00177901 File Offset: 0x00175B01
		protected override void RegisterPorts()
		{
			this.answerData = base.AddValueOutput<AnswerData>("answerData".CapitalizeFirst(), delegate
			{
				AnswerData answerData = new AnswerData();
				MercenariesDef data = GameBalance.Me.GetData<MercenariesDef>(MainGame.Instance.GameSave.militaryBaseData.MercenariesPaymentId);
				if (data != null)
				{
					SmartRes smartRes = new SmartRes();
					if (data.needItems.Count > 0)
					{
						using (List<NeedItemData>.Enumerator enumerator = data.needItems.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								NeedItemData needItemData = enumerator.Current;
								smartRes.items.Add(new ItemCount(needItemData.id, needItemData.GetCount(null)));
							}
							goto IL_00A5;
						}
					}
					smartRes.gameRes.Add("money", (float)data.money);
					IL_00A5:
					answerData.costRes = smartRes;
				}
				return answerData;
			}, "");
		}

		// Token: 0x0400412B RID: 16683
		private ValueOutput<AnswerData> answerData;
	}
}
