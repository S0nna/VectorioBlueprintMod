using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.Stats;

// Token: 0x020001A0 RID: 416
public class StatUI : MonoBehaviour
{
	// Token: 0x06000DBB RID: 3515 RVA: 0x0003CCF4 File Offset: 0x0003AEF4
	public void Set(Stat stat, StatInfo info, Color color)
	{
		this.background.color = color;
		string text = "";
		StatFloat statFloat = stat as StatFloat;
		if (statFloat != null)
		{
			text = statFloat.Value.ToString();
		}
		else
		{
			StatInt statInt = stat as StatInt;
			if (statInt != null)
			{
				text = statInt.Value.ToString();
			}
		}
		this.icon.sprite = info.icon;
		this.value.text = string.Concat(new string[]
		{
			"<size=7.5><b>",
			info.name.ToUpper(),
			":</b></size> <size=9>",
			text,
			info.suffix,
			"</size>"
		});
		if (stat.HasGamemodeModifier)
		{
			this.gamemodeModifier.text = stat.GamemodeModifier.GetFormattedString(info);
		}
		else
		{
			this.gamemodeModifier.text = "<color=white>~</color=white>";
		}
		if (stat.HasUpgradeModifier)
		{
			this.upgradeModifier.text = stat.UpgradeModifier.GetFormattedString(info);
		}
		else
		{
			this.upgradeModifier.text = "<color=white>~</color=white>";
		}
		if (stat.HasEnvironmentModifier)
		{
			this.environmentModifier.text = stat.EnvironmentModifier.GetFormattedString(info);
		}
		else
		{
			this.environmentModifier.text = "<color=white>~</color=white>";
		}
		if (this.useFullDetails)
		{
			if (stat.HasBuildingModifier)
			{
				this.buildingModifier.text = stat.BuildingModifier.GetFormattedString(info);
			}
			else
			{
				this.buildingModifier.text = "<color=white>~</color=white>";
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
	}

	// Token: 0x040009E6 RID: 2534
	public RectTransform rectTransform;

	// Token: 0x040009E7 RID: 2535
	public Image icon;

	// Token: 0x040009E8 RID: 2536
	public Image background;

	// Token: 0x040009E9 RID: 2537
	public TextMeshProUGUI value;

	// Token: 0x040009EA RID: 2538
	public TextMeshProUGUI gamemodeModifier;

	// Token: 0x040009EB RID: 2539
	public TextMeshProUGUI upgradeModifier;

	// Token: 0x040009EC RID: 2540
	public TextMeshProUGUI environmentModifier;

	// Token: 0x040009ED RID: 2541
	public bool useFullDetails;

	// Token: 0x040009EE RID: 2542
	public TextMeshProUGUI buildingModifier;
}
