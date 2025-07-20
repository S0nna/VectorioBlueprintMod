using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FOW.Demos
{
	// Token: 0x02000390 RID: 912
	public class TeamsDemo : MonoBehaviour
	{
		// Token: 0x060017AA RID: 6058 RVA: 0x00073229 File Offset: 0x00071429
		private void Awake()
		{
			this.team = 2;
			this.changeTeams();
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00073238 File Offset: 0x00071438
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.changeTeams();
			}
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0007324C File Offset: 0x0007144C
		private void changeTeams()
		{
			this.team++;
			this.team %= 3;
			this.teamText.text = string.Format("VIEWING AS TEAM {0}", this.team + 1);
			foreach (FogOfWarRevealer fogOfWarRevealer in this.team1Members)
			{
				fogOfWarRevealer.enabled = false;
				fogOfWarRevealer.GetComponent<FogOfWarHider>().enabled = true;
			}
			foreach (FogOfWarRevealer fogOfWarRevealer2 in this.team2Members)
			{
				fogOfWarRevealer2.enabled = false;
				fogOfWarRevealer2.GetComponent<FogOfWarHider>().enabled = true;
			}
			foreach (FogOfWarRevealer fogOfWarRevealer3 in this.team3Members)
			{
				fogOfWarRevealer3.enabled = false;
				fogOfWarRevealer3.GetComponent<FogOfWarHider>().enabled = true;
			}
			switch (this.team)
			{
			case 0:
				this.teamText.color = this.team1Color;
				using (List<FogOfWarRevealer>.Enumerator enumerator = this.team1Members.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FogOfWarRevealer fogOfWarRevealer4 = enumerator.Current;
						fogOfWarRevealer4.enabled = true;
						fogOfWarRevealer4.GetComponent<FogOfWarHider>().enabled = false;
					}
					return;
				}
				break;
			case 1:
				break;
			case 2:
				goto IL_1BD;
			default:
				return;
			}
			this.teamText.color = this.team2Color;
			using (List<FogOfWarRevealer>.Enumerator enumerator = this.team2Members.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FogOfWarRevealer fogOfWarRevealer5 = enumerator.Current;
					fogOfWarRevealer5.enabled = true;
					fogOfWarRevealer5.GetComponent<FogOfWarHider>().enabled = false;
				}
				return;
			}
			IL_1BD:
			this.teamText.color = this.team3Color;
			foreach (FogOfWarRevealer fogOfWarRevealer6 in this.team3Members)
			{
				fogOfWarRevealer6.enabled = true;
				fogOfWarRevealer6.GetComponent<FogOfWarHider>().enabled = false;
			}
		}

		// Token: 0x04001752 RID: 5970
		public Text teamText;

		// Token: 0x04001753 RID: 5971
		public Color team1Color = Color.blue;

		// Token: 0x04001754 RID: 5972
		public List<FogOfWarRevealer> team1Members = new List<FogOfWarRevealer>();

		// Token: 0x04001755 RID: 5973
		public Color team2Color = Color.green;

		// Token: 0x04001756 RID: 5974
		public List<FogOfWarRevealer> team2Members = new List<FogOfWarRevealer>();

		// Token: 0x04001757 RID: 5975
		public Color team3Color = Color.red;

		// Token: 0x04001758 RID: 5976
		public List<FogOfWarRevealer> team3Members = new List<FogOfWarRevealer>();

		// Token: 0x04001759 RID: 5977
		private int team;
	}
}
