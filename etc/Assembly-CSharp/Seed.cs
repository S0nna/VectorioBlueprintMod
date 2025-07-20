using System;

// Token: 0x0200022C RID: 556
public class Seed
{
	// Token: 0x06001047 RID: 4167 RVA: 0x0004C98C File Offset: 0x0004AB8C
	public Seed(int newSeed)
	{
		this.seed = newSeed;
		this.currentValue = (long)(this.seed % Seed.mod);
		this.currentValue += 17L;
		this.currentValue *= this.currentValue + 13L;
		this.currentValue += (long)(1003 * this.seed);
		this.currentValue %= (long)Seed.mod;
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x0004CA10 File Offset: 0x0004AC10
	public int Next()
	{
		this.currentValue += 17L;
		this.currentValue *= this.currentValue + 13L;
		this.currentValue += (long)(1003 * this.seed);
		this.currentValue %= (long)Seed.mod;
		return (int)this.currentValue;
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x0004CA78 File Offset: 0x0004AC78
	public int Get(int x, int y)
	{
		return (int)(((long)(x * x + 4 * y + this.seed + 997) % (long)Seed.mod * (long)(x ^ this.seed + y ^ this.seed + 7) + (long)(3 * this.seed + 17 * y + this.seed * this.seed + 10007)) % (long)Seed.mod);
	}

	// Token: 0x04000E49 RID: 3657
	private int seed;

	// Token: 0x04000E4A RID: 3658
	private static int mod = 1000000007;

	// Token: 0x04000E4B RID: 3659
	private long currentValue;
}
