using System;
using FlowCanvas;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public abstract class GKCustomFlowNode : CustomFlowNode
{
	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x060020E2 RID: 8418 RVA: 0x0008522C File Offset: 0x0008342C
	public PlayerData PlayerData
	{
		get
		{
			return MainGame.PlayerData;
		}
	}

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x060020E3 RID: 8419 RVA: 0x00084092 File Offset: 0x00082292
	public WorldData WorldData
	{
		get
		{
			return MainGame.Instance.GameSave.worldData;
		}
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x060020E4 RID: 8420 RVA: 0x00051F44 File Offset: 0x00050144
	public virtual int MinWidth
	{
		get
		{
			return -1;
		}
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x060020E5 RID: 8421 RVA: 0x0009BAB4 File Offset: 0x00099CB4
	protected WgoData SelfWgoData
	{
		get
		{
			WgoDataScript componentFromNode = CustomFlowNode.GetComponentFromNode<WgoDataScript>(this);
			if (componentFromNode != null)
			{
				return componentFromNode.wgoData;
			}
			Debug.LogError("GKCustomFlowNode: Not found WgoData");
			return null;
		}
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x0009BAE4 File Offset: 0x00099CE4
	protected WgoData WgoDataParamOrSelf(ValueInput<WgoData> param)
	{
		WgoData wgoData;
		if (param.value != null)
		{
			wgoData = param.value;
		}
		else
		{
			wgoData = this.SelfWgoData;
		}
		return wgoData;
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x0009BB0C File Offset: 0x00099D0C
	protected T ParamValueOrSelf<T>(ValueInput<T> param) where T : MonoBehaviour
	{
		T t = default(T);
		if (param.value != null)
		{
			t = param.value;
		}
		else
		{
			t = CustomFlowNode.GetComponentFromNode<T>(this);
			if (t == null)
			{
				Debug.LogError("GKCustomFlowNode: Not Found Object T");
			}
		}
		return t;
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x0009BB60 File Offset: 0x00099D60
	protected bool TryGetParamValue<T>(ValueInput<T> valueInput, out T value) where T : class
	{
		value = default(T);
		if (valueInput == null)
		{
			Debug.LogError(string.Format("{0}: null ValueInput for type {1}", "GKCustomFlowNode", typeof(T)));
			return false;
		}
		if (valueInput.value == null)
		{
			Debug.LogError(string.Format("{0}: Not Found Object {1}", "GKCustomFlowNode", typeof(T)));
			return false;
		}
		value = valueInput.value;
		return true;
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x0009BBD4 File Offset: 0x00099DD4
	protected GameObject ParamValueOrSelf(ValueInput<GameObject> param)
	{
		GameObject gameObject;
		if (param.value != null)
		{
			gameObject = param.value;
		}
		else
		{
			gameObject = CustomFlowNode.GetGameObjectFromNode(this);
			if (gameObject == null)
			{
				Debug.LogError("GKCustomFlowNode: Not Found GameObject");
			}
		}
		return gameObject;
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x0009BC15 File Offset: 0x00099E15
	protected override void TerminateScript()
	{
		if (base.graph == null)
		{
			Debug.LogError("Node belongs to no graph", base.graph);
			return;
		}
		GameScriptUtility.TerminateScript(base.graphAgent.gameObject);
	}

	// Token: 0x04001D98 RID: 7576
	public const string CUSTOM_ICONS = "Assets/GFX/ParadoxNotionCustomIcons/";
}
