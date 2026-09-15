using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C13 RID: 3091
	[Name("Smart Res", 0)]
	[Category("Game/Actions")]
	public class Flow_SmartRes : GKCustomFlowNode
	{
		// Token: 0x06004F53 RID: 20307 RVA: 0x00175D45 File Offset: 0x00173F45
		protected override void RegisterPorts()
		{
			this.smartRes = base.AddValueOutput<SmartRes>("smartRes".CapitalizeFirst(), delegate
			{
				SmartRes smartRes = new SmartRes();
				if (this.options.Count > 0)
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						SmartResType value = this.options[i].resType;
						if (value != SmartResType.GameRes)
						{
							if (value == SmartResType.Item)
							{
								if (smartRes.items == null)
								{
									smartRes.items = new List<ItemCount>();
								}
								smartRes.items.Add(new ItemCount(this.options[i].id, Mathf.RoundToInt(this.options[i].value)));
							}
						}
						else
						{
							if (smartRes.gameRes == null)
							{
								smartRes.gameRes = new GameRes();
							}
							smartRes.gameRes.Add(this.options[i].id, this.options[i].value);
						}
					}
				}
				else
				{
					SmartResType value = this.resType.value;
					if (value != SmartResType.GameRes)
					{
						if (value == SmartResType.Item)
						{
							smartRes.items = new List<ItemCount>();
							smartRes.items.Add(new ItemCount(this.paramName.value, Mathf.RoundToInt(this.paramValue.value)));
						}
					}
					else
					{
						smartRes.gameRes = new GameRes();
						smartRes.gameRes.Add(new GameResAtom(this.paramName.value, this.paramValue.value));
					}
				}
				return smartRes;
			}, "");
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x00028294 File Offset: 0x00026494
		private bool SyncLists()
		{
			return false;
		}

		// Token: 0x040040B0 RID: 16560
		[SerializeField]
		private List<Flow_SmartRes.Option> options = new List<Flow_SmartRes.Option>();

		// Token: 0x040040B1 RID: 16561
		private ValueInput<SmartResType> resType;

		// Token: 0x040040B2 RID: 16562
		private ValueInput<string> paramName;

		// Token: 0x040040B3 RID: 16563
		private ValueInput<float> paramValue;

		// Token: 0x040040B4 RID: 16564
		private ValueOutput<SmartRes> smartRes;

		// Token: 0x02000C14 RID: 3092
		[Serializable]
		private class Option
		{
			// Token: 0x040040B5 RID: 16565
			public SmartResType resType;

			// Token: 0x040040B6 RID: 16566
			public string id = string.Empty;

			// Token: 0x040040B7 RID: 16567
			public float value;
		}
	}
}
