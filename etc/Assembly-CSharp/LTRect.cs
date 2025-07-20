using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
[Serializable]
public class LTRect
{
	// Token: 0x0600021B RID: 539 RVA: 0x0000F05C File Offset: 0x0000D25C
	public LTRect()
	{
		this.reset();
		this.rotateEnabled = (this.alphaEnabled = true);
		this._rect = new Rect(0f, 0f, 1f, 1f);
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
	public LTRect(Rect rect)
	{
		this._rect = rect;
		this.reset();
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000F13C File Offset: 0x0000D33C
	public LTRect(float x, float y, float width, float height)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000F1C4 File Offset: 0x0000D3C4
	public LTRect(float x, float y, float width, float height, float alpha)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000F248 File Offset: 0x0000D448
	public LTRect(float x, float y, float width, float height, float alpha, float rotation)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = rotation;
		this.rotateEnabled = (this.alphaEnabled = false);
		if (rotation != 0f)
		{
			this.rotateEnabled = true;
			this.resetForRotation();
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000220 RID: 544 RVA: 0x0000F2DD File Offset: 0x0000D4DD
	public bool hasInitiliazed
	{
		get
		{
			return this._id != -1;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000221 RID: 545 RVA: 0x0000F2EB File Offset: 0x0000D4EB
	public int id
	{
		get
		{
			return this._id | this.counter << 16;
		}
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000F2FD File Offset: 0x0000D4FD
	public void setId(int id, int counter)
	{
		this._id = id;
		this.counter = counter;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000F310 File Offset: 0x0000D510
	public void reset()
	{
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
		this.margin = Vector2.zero;
		this.sizeByHeight = false;
		this.useColor = false;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000F35C File Offset: 0x0000D55C
	public void resetForRotation()
	{
		Vector3 vector = new Vector3(GUI.matrix[0, 0], GUI.matrix[1, 1], GUI.matrix[2, 2]);
		if (this.pivot == Vector2.zero)
		{
			this.pivot = new Vector2((this._rect.x + this._rect.width * 0.5f) * vector.x + GUI.matrix[0, 3], (this._rect.y + this._rect.height * 0.5f) * vector.y + GUI.matrix[1, 3]);
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000225 RID: 549 RVA: 0x0000F422 File Offset: 0x0000D622
	// (set) Token: 0x06000226 RID: 550 RVA: 0x0000F42F File Offset: 0x0000D62F
	public float x
	{
		get
		{
			return this._rect.x;
		}
		set
		{
			this._rect.x = value;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000227 RID: 551 RVA: 0x0000F43D File Offset: 0x0000D63D
	// (set) Token: 0x06000228 RID: 552 RVA: 0x0000F44A File Offset: 0x0000D64A
	public float y
	{
		get
		{
			return this._rect.y;
		}
		set
		{
			this._rect.y = value;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000229 RID: 553 RVA: 0x0000F458 File Offset: 0x0000D658
	// (set) Token: 0x0600022A RID: 554 RVA: 0x0000F465 File Offset: 0x0000D665
	public float width
	{
		get
		{
			return this._rect.width;
		}
		set
		{
			this._rect.width = value;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600022B RID: 555 RVA: 0x0000F473 File Offset: 0x0000D673
	// (set) Token: 0x0600022C RID: 556 RVA: 0x0000F480 File Offset: 0x0000D680
	public float height
	{
		get
		{
			return this._rect.height;
		}
		set
		{
			this._rect.height = value;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600022D RID: 557 RVA: 0x0000F490 File Offset: 0x0000D690
	// (set) Token: 0x0600022E RID: 558 RVA: 0x0000F5A1 File Offset: 0x0000D7A1
	public Rect rect
	{
		get
		{
			if (LTRect.colorTouched)
			{
				LTRect.colorTouched = false;
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			}
			if (this.rotateEnabled)
			{
				if (this.rotateFinished)
				{
					this.rotateFinished = false;
					this.rotateEnabled = false;
					this.pivot = Vector2.zero;
				}
				else
				{
					GUIUtility.RotateAroundPivot(this.rotation, this.pivot);
				}
			}
			if (this.alphaEnabled)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alpha);
				LTRect.colorTouched = true;
			}
			if (this.fontScaleToFit)
			{
				if (this.useSimpleScale)
				{
					this.style.fontSize = (int)(this._rect.height * this.relativeRect.height);
				}
				else
				{
					this.style.fontSize = (int)this._rect.height;
				}
			}
			return this._rect;
		}
		set
		{
			this._rect = value;
		}
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000F5AA File Offset: 0x0000D7AA
	public LTRect setStyle(GUIStyle style)
	{
		this.style = style;
		return this;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000F5B4 File Offset: 0x0000D7B4
	public LTRect setFontScaleToFit(bool fontScaleToFit)
	{
		this.fontScaleToFit = fontScaleToFit;
		return this;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000F5BE File Offset: 0x0000D7BE
	public LTRect setColor(Color color)
	{
		this.color = color;
		this.useColor = true;
		return this;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000F5CF File Offset: 0x0000D7CF
	public LTRect setAlpha(float alpha)
	{
		this.alpha = alpha;
		return this;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000F5D9 File Offset: 0x0000D7D9
	public LTRect setLabel(string str)
	{
		this.labelStr = str;
		return this;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000F5E3 File Offset: 0x0000D7E3
	public LTRect setUseSimpleScale(bool useSimpleScale, Rect relativeRect)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = relativeRect;
		return this;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
	public LTRect setUseSimpleScale(bool useSimpleScale)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		return this;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000F61F File Offset: 0x0000D81F
	public LTRect setSizeByHeight(bool sizeByHeight)
	{
		this.sizeByHeight = sizeByHeight;
		return this;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000F62C File Offset: 0x0000D82C
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"x:",
			this._rect.x.ToString(),
			" y:",
			this._rect.y.ToString(),
			" width:",
			this._rect.width.ToString(),
			" height:",
			this._rect.height.ToString()
		});
	}

	// Token: 0x040001A0 RID: 416
	public Rect _rect;

	// Token: 0x040001A1 RID: 417
	public float alpha = 1f;

	// Token: 0x040001A2 RID: 418
	public float rotation;

	// Token: 0x040001A3 RID: 419
	public Vector2 pivot;

	// Token: 0x040001A4 RID: 420
	public Vector2 margin;

	// Token: 0x040001A5 RID: 421
	public Rect relativeRect = new Rect(0f, 0f, float.PositiveInfinity, float.PositiveInfinity);

	// Token: 0x040001A6 RID: 422
	public bool rotateEnabled;

	// Token: 0x040001A7 RID: 423
	[HideInInspector]
	public bool rotateFinished;

	// Token: 0x040001A8 RID: 424
	public bool alphaEnabled;

	// Token: 0x040001A9 RID: 425
	public string labelStr;

	// Token: 0x040001AA RID: 426
	public LTGUI.Element_Type type;

	// Token: 0x040001AB RID: 427
	public GUIStyle style;

	// Token: 0x040001AC RID: 428
	public bool useColor;

	// Token: 0x040001AD RID: 429
	public Color color = Color.white;

	// Token: 0x040001AE RID: 430
	public bool fontScaleToFit;

	// Token: 0x040001AF RID: 431
	public bool useSimpleScale;

	// Token: 0x040001B0 RID: 432
	public bool sizeByHeight;

	// Token: 0x040001B1 RID: 433
	public Texture texture;

	// Token: 0x040001B2 RID: 434
	private int _id = -1;

	// Token: 0x040001B3 RID: 435
	[HideInInspector]
	public int counter;

	// Token: 0x040001B4 RID: 436
	public static bool colorTouched;
}
