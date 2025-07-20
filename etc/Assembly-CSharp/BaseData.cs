using System;
using UnityEngine;

// Token: 0x020000A0 RID: 160
[Serializable]
public class BaseData : ScriptableObject
{
	// Token: 0x0600064B RID: 1611 RVA: 0x0001F80B File Offset: 0x0001DA0B
	protected void UpdateID()
	{
		this.id = this.prefix.ToLower() + this.Name.Replace(' ', '_').ToLower();
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x0600064C RID: 1612 RVA: 0x0001F837 File Offset: 0x0001DA37
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001F83F File Offset: 0x0001DA3F
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x0001F848 File Offset: 0x0001DA48
	public string FormattedName
	{
		get
		{
			return char.ToUpper(this.name[0]).ToString() + this.name.Substring(1).ToLower();
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x0001F884 File Offset: 0x0001DA84
	public string Description
	{
		get
		{
			return this.description;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001F88C File Offset: 0x0001DA8C
	public string ShortDescription
	{
		get
		{
			if (!this.useShortDesc)
			{
				return this.description;
			}
			return this.shortDesc;
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00003212 File Offset: 0x00001412
	public void SetBaseData(string name, string description, string prefix = "")
	{
	}

	// Token: 0x040003C3 RID: 963
	[SerializeField]
	private string id;

	// Token: 0x040003C4 RID: 964
	[SerializeField]
	private new string name;

	// Token: 0x040003C5 RID: 965
	[SerializeField]
	private bool usePrefix;

	// Token: 0x040003C6 RID: 966
	[SerializeField]
	private string prefix = "";

	// Token: 0x040003C7 RID: 967
	[SerializeField]
	[TextArea]
	private string description;

	// Token: 0x040003C8 RID: 968
	[SerializeField]
	private bool useShortDesc = true;

	// Token: 0x040003C9 RID: 969
	[SerializeField]
	private string shortDesc;
}
