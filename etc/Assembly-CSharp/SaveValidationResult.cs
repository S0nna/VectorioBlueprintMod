using System;

// Token: 0x0200009D RID: 157
public struct SaveValidationResult
{
	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001F5E9 File Offset: 0x0001D7E9
	// (set) Token: 0x0600062F RID: 1583 RVA: 0x0001F5F1 File Offset: 0x0001D7F1
	public bool IsSaveOutdated { readonly get; set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001F5FA File Offset: 0x0001D7FA
	// (set) Token: 0x06000631 RID: 1585 RVA: 0x0001F602 File Offset: 0x0001D802
	public bool CanSaveBeLoaded { readonly get; set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001F60B File Offset: 0x0001D80B
	// (set) Token: 0x06000633 RID: 1587 RVA: 0x0001F613 File Offset: 0x0001D813
	public string SaveStatus { readonly get; set; }
}
