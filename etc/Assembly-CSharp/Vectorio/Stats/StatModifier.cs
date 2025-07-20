using System;
using Vectorio.Formatting;

namespace Vectorio.Stats
{
	// Token: 0x02000267 RID: 615
	public class StatModifier
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x000524CC File Offset: 0x000506CC
		public StatModifier(float value, bool isMultiplier, ulong? sourceID)
		{
			this.Value = value;
			this.IsMultiplier = isMultiplier;
			this.SourceID = sourceID;
			if (this.IsMultiplier)
			{
				this.Reduces = (value < 1f);
				return;
			}
			this.Reduces = (value < 0f);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0005251C File Offset: 0x0005071C
		public StatModifier(float value, bool isMultiplier) : this(value, isMultiplier, null)
		{
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0005253C File Offset: 0x0005073C
		public string GetFormattedString(StatInfo info)
		{
			string text = "";
			if (this.Reduces)
			{
				if (info.lowerBetter)
				{
					text += "<color=green>";
				}
				else
				{
					text += "<color=red>";
				}
				text += "-";
			}
			else
			{
				if (info.lowerBetter)
				{
					text += "<color=red>";
				}
				else
				{
					text += "<color=green>";
				}
				text += "+";
			}
			if (this.IsMultiplier)
			{
				text = text + ((int)(this.Value * 100f)).ToString() + "%</color>";
			}
			else
			{
				text = text + Formatter.Round(this.Value, 1) + "</color>";
			}
			return text;
		}

		// Token: 0x04000F65 RID: 3941
		public float Value;

		// Token: 0x04000F66 RID: 3942
		public readonly bool IsMultiplier;

		// Token: 0x04000F67 RID: 3943
		public readonly ulong? SourceID;

		// Token: 0x04000F68 RID: 3944
		public readonly bool Reduces;
	}
}
