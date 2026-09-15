using System;
using UnityEngine;

// Token: 0x0200015B RID: 347
public interface IBuildPointerObject
{
	// Token: 0x0600084C RID: 2124
	bool HasRotation();

	// Token: 0x0600084D RID: 2125
	void Rotate();

	// Token: 0x0600084E RID: 2126
	bool TryDoBuildAction();

	// Token: 0x0600084F RID: 2127
	void SetupSelectionCells(BuildSelectionCell[] prefabCells, Transform parent);

	// Token: 0x06000850 RID: 2128
	void UpdateSelectionCellsState();

	// Token: 0x06000851 RID: 2129
	void UpdatePosition(Vector3 position);

	// Token: 0x06000852 RID: 2130
	void ClearSelectionCells();
}
