using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.PhasmaUI;
using XNode;

// Token: 0x0200015A RID: 346
public class ResearchTreeButton : Vectorio.PhasmaUI.Button
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x000314EB File Offset: 0x0002F6EB
	public ResearchTreeData Tree
	{
		get
		{
			return this._tree;
		}
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x000314F4 File Offset: 0x0002F6F4
	public void Setup(ResearchTreeData tree, Color backgroundColor)
	{
		this._tree = tree;
		this.icon.sprite = tree.icon;
		this.title.text = tree.name.ToUpper();
		this.background.color = backgroundColor;
		this.UpdateButton();
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x00031544 File Offset: 0x0002F744
	public void UpdateButton()
	{
		if (!Singleton<Research>.Instance.IsTreeUnlocked(this._tree))
		{
			this.subtitle.color = Color.red;
			this.subtitle.text = "LOCKED";
			return;
		}
		int num = 0;
		int num2 = 0;
		foreach (Node node in this._tree.nodes)
		{
			ResearchTechData researchTechData = node as ResearchTechData;
			if (researchTechData != null)
			{
				num++;
				if (Singleton<Research>.Instance.IsTechUnlocked(researchTechData))
				{
					num2++;
				}
			}
		}
		if (num2 >= num)
		{
			this.subtitle.color = Color.green;
			this.subtitle.text = "COMPLETE";
			return;
		}
		this.subtitle.color = Color.white;
		this.subtitle.text = num2.ToString() + " / " + num.ToString() + " COMPLETE";
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x00031648 File Offset: 0x0002F848
	public override void OnClick()
	{
		if (Singleton<Research>.Instance.IsTreeUnlocked(this._tree))
		{
			UI_Singleton<UI_ResearchWindow>.Instance.SetTree(this._tree);
			base.OnClick();
		}
	}

	// Token: 0x040007C5 RID: 1989
	private ResearchTreeData _tree;

	// Token: 0x040007C6 RID: 1990
	public Image background;

	// Token: 0x040007C7 RID: 1991
	public Image icon;

	// Token: 0x040007C8 RID: 1992
	public TextMeshProUGUI title;

	// Token: 0x040007C9 RID: 1993
	public TextMeshProUGUI subtitle;
}
