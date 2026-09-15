using System;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;

// Token: 0x0200083C RID: 2108
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UIHierarchySorter : MonoBehaviour
{
	// Token: 0x060035D8 RID: 13784 RVA: 0x00102C87 File Offset: 0x00100E87
	public void ReinitChildrenAndSortComponents()
	{
		this.sortComponents = this.HierarchyTarget.GetComponentsInChildren<UISortComponent>(true).ToList<UISortComponent>();
		this.Sort();
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x060035D9 RID: 13785 RVA: 0x00102CA6 File Offset: 0x00100EA6
	public RectTransform HierarchyTarget
	{
		get
		{
			return this.hierarchyTarget;
		}
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x00102CAE File Offset: 0x00100EAE
	private void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}
		this.Sort();
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x00102CC0 File Offset: 0x00100EC0
	public void Sort()
	{
		if (this.sortComponents == null || this.sortComponents.Count == 0)
		{
			return;
		}
		if (this.hierarchyTarget == null)
		{
			this.hierarchyTarget = base.transform as RectTransform;
		}
		this.sortComponents.Sort(delegate(UISortComponent a, UISortComponent b)
		{
			float num = a.transform.position.y + a.FloorLine;
			return (b.transform.position.y + b.FloorLine).CompareTo(num);
		});
		for (int i = 0; i < this.sortComponents.Count; i++)
		{
			if (this.hierarchyTarget != null)
			{
				this.sortComponents[i].transform.SetParent(this.hierarchyTarget.transform);
			}
			this.sortComponents[i].transform.SetSiblingIndex(i);
		}
	}

	// Token: 0x04002B23 RID: 11043
	[SerializeField]
	private bool editorOnly;

	// Token: 0x04002B24 RID: 11044
	[SerializeField]
	private RectTransform hierarchyTarget;

	// Token: 0x04002B25 RID: 11045
	private List<UISortComponent> sortComponents;
}
