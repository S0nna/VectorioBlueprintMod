using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

// Token: 0x020000CC RID: 204
[CreateAssetMenu(fileName = "New Tech", menuName = "Vectorio/Tech Node")]
[Node.NodeWidthAttribute(380)]
public class ResearchTechData : Node
{
	// Token: 0x060006C6 RID: 1734 RVA: 0x000202B5 File Offset: 0x0001E4B5
	public T GetReward<T>() where T : BaseData
	{
		return this.reward as T;
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x000202C8 File Offset: 0x0001E4C8
	private void OnRewardUpdated()
	{
		if (this.reward is EntityData)
		{
			this.rewardType = ResearchTechData.RewardType.Entity;
			return;
		}
		if (this.reward is ResourceData)
		{
			this.rewardType = ResearchTechData.RewardType.Resource;
			return;
		}
		if (this.reward is RecipeData)
		{
			this.rewardType = ResearchTechData.RewardType.Recipe;
			return;
		}
		if (this.reward is ResearchTreeData)
		{
			this.rewardType = ResearchTechData.RewardType.Tree;
			return;
		}
		this.rewardType = ResearchTechData.RewardType.None;
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00020330 File Offset: 0x0001E530
	protected override void Init()
	{
		base.Init();
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x00020338 File Offset: 0x0001E538
	public override object GetValue(NodePort port)
	{
		return this.output;
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x00020340 File Offset: 0x0001E540
	public bool HasInputNodes()
	{
		return base.GetPort("input").ConnectionCount > 0;
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x00020355 File Offset: 0x0001E555
	public bool HasOutputNodes()
	{
		return base.GetPort("output").ConnectionCount > 0;
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0002036A File Offset: 0x0001E56A
	public bool IsTechInTree(ResearchTreeData tree)
	{
		return (ResearchTreeData)this.graph == tree;
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00020380 File Offset: 0x0001E580
	public ResearchTechData GetInputNode()
	{
		NodePort port = base.GetPort("input");
		if (port.ConnectionCount == 0)
		{
			return null;
		}
		return port.GetConnection(0).node as ResearchTechData;
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x000203B4 File Offset: 0x0001E5B4
	public List<ResearchTechData> GetOutputNodes()
	{
		NodePort port = base.GetPort("output");
		if (port.ConnectionCount == 0)
		{
			return new List<ResearchTechData>();
		}
		List<ResearchTechData> list = new List<ResearchTechData>();
		foreach (NodePort nodePort in port.GetConnections())
		{
			ResearchTechData researchTechData = (ResearchTechData)nodePort.node;
			if (researchTechData == null)
			{
				Debug.Log("[RESEARCH] Could not convert node to research tech data!");
			}
			else
			{
				list.Add(researchTechData);
			}
		}
		return list;
	}

	// Token: 0x0400046F RID: 1135
	public Texture2D preview;

	// Token: 0x04000470 RID: 1136
	public BaseData reward;

	// Token: 0x04000471 RID: 1137
	public ResearchTechData.RewardType rewardType;

	// Token: 0x04000472 RID: 1138
	public List<Cost> costs;

	// Token: 0x04000473 RID: 1139
	public TutorialData tutorial;

	// Token: 0x04000474 RID: 1140
	public string tutorialID;

	// Token: 0x04000475 RID: 1141
	public bool encrypted;

	// Token: 0x04000476 RID: 1142
	[Node.InputAttribute(Node.ShowBackingValue.Unconnected, Node.ConnectionType.Multiple, Node.TypeConstraint.None, false, backingValue = Node.ShowBackingValue.Never, connectionType = Node.ConnectionType.Override)]
	public ResearchTechData input;

	// Token: 0x04000477 RID: 1143
	[Node.OutputAttribute(Node.ShowBackingValue.Never, Node.ConnectionType.Multiple, Node.TypeConstraint.None, false)]
	public ResearchTechData output;

	// Token: 0x020000CD RID: 205
	public enum RewardType
	{
		// Token: 0x04000479 RID: 1145
		None,
		// Token: 0x0400047A RID: 1146
		Entity,
		// Token: 0x0400047B RID: 1147
		Resource,
		// Token: 0x0400047C RID: 1148
		Recipe,
		// Token: 0x0400047D RID: 1149
		Tree
	}
}
