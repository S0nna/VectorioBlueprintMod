using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000154 RID: 340
public class LevelButton : MonoBehaviour
{
	// Token: 0x06000B12 RID: 2834 RVA: 0x0002F4CC File Offset: 0x0002D6CC
	public void Set(Level level, int number, bool unlocked)
	{
		this.title.text = "LEVEL " + number.ToString();
		this.description.text = level.description;
		this.levelIcon.sprite = ((level.cosmeticID != "") ? this.cosmeticIcon : this.normalIcon);
		this.description.color = ((level.variableID != "") ? this.rewardColor : this.noRewardColor);
		this.levelIcon.color = (unlocked ? this.unlockedColor : this.normalColor);
		this.level.text = number.ToString();
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x0002F58A File Offset: 0x0002D78A
	public void ToggleLine(bool toggle)
	{
		this.line.SetActive(toggle);
	}

	// Token: 0x04000757 RID: 1879
	public GameObject line;

	// Token: 0x04000758 RID: 1880
	public TextMeshProUGUI title;

	// Token: 0x04000759 RID: 1881
	public TextMeshProUGUI description;

	// Token: 0x0400075A RID: 1882
	public TextMeshProUGUI level;

	// Token: 0x0400075B RID: 1883
	public Image levelIcon;

	// Token: 0x0400075C RID: 1884
	public Sprite normalIcon;

	// Token: 0x0400075D RID: 1885
	public Sprite cosmeticIcon;

	// Token: 0x0400075E RID: 1886
	public Color normalColor;

	// Token: 0x0400075F RID: 1887
	public Color unlockedColor;

	// Token: 0x04000760 RID: 1888
	public Color rewardColor;

	// Token: 0x04000761 RID: 1889
	public Color noRewardColor;
}
