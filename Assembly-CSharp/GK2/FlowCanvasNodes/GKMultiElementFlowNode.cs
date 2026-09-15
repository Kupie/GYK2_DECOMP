using System;
using System.Collections.Generic;
using FlowCanvas;
using LinqTools;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C3F RID: 3135
	public abstract class GKMultiElementFlowNode<T> : GKCustomFlowNode where T : Element
	{
		// Token: 0x06004FDF RID: 20447 RVA: 0x001783C8 File Offset: 0x001765C8
		protected void RegisterWgoIdsPorts()
		{
			this.wgosIds = base.AddValueInput<string[]>("WGOs List", "");
			this.wgosIds.SetDefaultAndSerializedValue(new string[0]);
			this.elements = base.AddValueInput<T[]>("Elements", "");
			this.elements.SetDefaultAndSerializedValue(Array.Empty<T>());
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x00178424 File Offset: 0x00176624
		public void AddNewElement(T el)
		{
			List<T> list = this.elements.value.ToList<T>();
			list.Add(el);
			this.elements.serializedValue = list.ToArray();
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x0017845C File Offset: 0x0017665C
		public void AddNewWgo(string WGOId = "")
		{
			List<string> list = this.wgosIds.value.ToList<string>();
			list.Add(WGOId);
			this.wgosIds.serializedValue = list.ToArray();
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00178494 File Offset: 0x00176694
		public void AddNewWgoIfAbsent(string WGOId = "")
		{
			if (!this.wgosIds.value.Contains(WGOId) && !(WGOId == "[Player]") && !(WGOId == "[Wisp]") && !(WGOId == "[Self]"))
			{
				this.AddNewWgo(WGOId);
			}
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x001784E4 File Offset: 0x001766E4
		protected virtual void SeparateNode(int i)
		{
			GKMultiElementFlowNode<T> gkmultiElementFlowNode = base.flowGraph.AddNode<GKMultiElementFlowNode<T>>(base.position + Vector2.right * 200f);
			foreach (string text in this.wgosIds.value)
			{
				gkmultiElementFlowNode.AddNewWgoIfAbsent(text);
			}
			List<T> list = this.elements.value.ToList<T>();
			for (int k = i + 1; k < this.elements.value.Length; k++)
			{
				gkmultiElementFlowNode.AddNewElement(this.elements.value[k]);
				list.RemoveAt(i + 1);
			}
			this.elements.serializedValue = list.ToArray();
			this.RemoveUnusedWGOs();
			gkmultiElementFlowNode.RemoveUnusedWGOs();
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x001785AC File Offset: 0x001767AC
		protected void RemoveUnusedWGOs()
		{
			List<string> list = new List<string>();
			foreach (T element in this.elements.value)
			{
				if (element.wgoId != "[Self]" && element.wgoId != "[Player]" && element.wgoId != "[Wisp]" && !list.Contains(element.wgoId))
				{
					list.Add(element.wgoId);
				}
			}
			List<string> list2 = this.wgosIds.value.ToList<string>();
			for (int j = 0; j < list2.Count; j++)
			{
				if (!list.Contains(list2[j]))
				{
					list2.RemoveAt(j);
					j--;
				}
			}
			this.wgosIds.serializedValue = list2.ToArray();
		}

		// Token: 0x04004153 RID: 16723
		public const string ID_PLAYER = "[Player]";

		// Token: 0x04004154 RID: 16724
		public const string ID_WISP = "[Wisp]";

		// Token: 0x04004155 RID: 16725
		public const string ID_SELF = "[Self]";

		// Token: 0x04004156 RID: 16726
		public static Color CLR_PLAYER = Color.yellow;

		// Token: 0x04004157 RID: 16727
		public static Color CLR_WISP = Color.cyan;

		// Token: 0x04004158 RID: 16728
		public static Color CLR_SELF = Color.green;

		// Token: 0x04004159 RID: 16729
		protected string[] wgoIdsArray;

		// Token: 0x0400415A RID: 16730
		protected ValueInput<string[]> wgosIds;

		// Token: 0x0400415B RID: 16731
		protected ValueInput<T[]> elements;
	}
}
