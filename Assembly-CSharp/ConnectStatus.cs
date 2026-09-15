using System;

// Token: 0x0200074B RID: 1867
public enum ConnectStatus
{
	// Token: 0x04002742 RID: 10050
	Undefined,
	// Token: 0x04002743 RID: 10051
	Success,
	// Token: 0x04002744 RID: 10052
	ServerFull,
	// Token: 0x04002745 RID: 10053
	LoggedInAgain,
	// Token: 0x04002746 RID: 10054
	UserRequestedDisconnect,
	// Token: 0x04002747 RID: 10055
	GenericDisconnect,
	// Token: 0x04002748 RID: 10056
	Reconnecting,
	// Token: 0x04002749 RID: 10057
	IncompatibleBuildType,
	// Token: 0x0400274A RID: 10058
	HostEndedSession,
	// Token: 0x0400274B RID: 10059
	StartHostFailed,
	// Token: 0x0400274C RID: 10060
	StartClientFailed
}
