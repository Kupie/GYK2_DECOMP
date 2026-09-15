using System;
using FlowCanvas;
using FlowCanvas.Nodes;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200016C RID: 364
	public abstract class CustomFlowNode : FlowControlNode
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x00027F98 File Offset: 0x00026198
		public static GameObject GetGameObjectFromNode(FlowNode flowNode)
		{
			Component graphAgent = flowNode.graphAgent;
			if (graphAgent == null)
			{
				return null;
			}
			return graphAgent.gameObject;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00027FC0 File Offset: 0x000261C0
		public static T GetComponentFromNode<T>(FlowNode flowNode) where T : MonoBehaviour
		{
			GameObject gameObjectFromNode = CustomFlowNode.GetGameObjectFromNode(flowNode);
			if (gameObjectFromNode != null)
			{
				return gameObjectFromNode.GetComponent<T>();
			}
			return default(T);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00027FED File Offset: 0x000261ED
		public ValueInput<T> GetValueInputPort<T>(string portId)
		{
			return base.GetInputPort(portId) as ValueInput<T>;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00027FFB File Offset: 0x000261FB
		public ValueInput GetValueInputPort(string portId)
		{
			return base.GetInputPort(portId) as ValueInput;
		}

		// Token: 0x06000800 RID: 2048
		protected abstract void TerminateScript();
	}
}
