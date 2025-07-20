using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000046 RID: 70
public class LTDescr
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060002AE RID: 686 RVA: 0x00010525 File Offset: 0x0000E725
	// (set) Token: 0x060002AF RID: 687 RVA: 0x0001052D File Offset: 0x0000E72D
	public Vector3 from
	{
		get
		{
			return this.fromInternal;
		}
		set
		{
			this.fromInternal = value;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x00010536 File Offset: 0x0000E736
	// (set) Token: 0x060002B1 RID: 689 RVA: 0x0001053E File Offset: 0x0000E73E
	public Vector3 to
	{
		get
		{
			return this.toInternal;
		}
		set
		{
			this.toInternal = value;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x00010547 File Offset: 0x0000E747
	// (set) Token: 0x060002B3 RID: 691 RVA: 0x0001054F File Offset: 0x0000E74F
	public LTDescr.ActionMethodDelegate easeInternal { get; set; }

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x00010558 File Offset: 0x0000E758
	// (set) Token: 0x060002B5 RID: 693 RVA: 0x00010560 File Offset: 0x0000E760
	public LTDescr.ActionMethodDelegate initInternal { get; set; }

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x00010569 File Offset: 0x0000E769
	public Transform toTrans
	{
		get
		{
			return this.optional.toTrans;
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00010578 File Offset: 0x0000E778
	public override string ToString()
	{
		string[] array = new string[27];
		array[0] = ((this.trans != null) ? ("name:" + this.trans.gameObject.name) : "gameObject:null");
		array[1] = " toggle:";
		array[2] = this.toggle.ToString();
		array[3] = " passed:";
		array[4] = this.passed.ToString();
		array[5] = " time:";
		array[6] = this.time.ToString();
		array[7] = " delay:";
		array[8] = this.delay.ToString();
		array[9] = " direction:";
		array[10] = this.direction.ToString();
		array[11] = " from:";
		array[12] = this.from.ToString();
		array[13] = " to:";
		array[14] = this.to.ToString();
		array[15] = " diff:";
		int num = 16;
		Vector3 vector = this.diff;
		array[num] = vector.ToString();
		array[17] = " type:";
		array[18] = this.type.ToString();
		array[19] = " ease:";
		array[20] = this.easeType.ToString();
		array[21] = " useEstimatedTime:";
		array[22] = this.useEstimatedTime.ToString();
		array[23] = " id:";
		array[24] = this.id.ToString();
		array[25] = " hasInitiliazed:";
		array[26] = this.hasInitiliazed.ToString();
		return string.Concat(array);
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00010737 File Offset: 0x0000E937
	[Obsolete("Use 'LeanTween.cancel( id )' instead")]
	public LTDescr cancel(GameObject gameObject)
	{
		if (gameObject == this.trans.gameObject)
		{
			LeanTween.removeTween((int)this._id, this.uniqueId);
		}
		return this;
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060002BA RID: 698 RVA: 0x0001075E File Offset: 0x0000E95E
	public int uniqueId
	{
		get
		{
			return (int)(this._id | this.counter << 16);
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060002BB RID: 699 RVA: 0x00010770 File Offset: 0x0000E970
	public int id
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060002BC RID: 700 RVA: 0x00010778 File Offset: 0x0000E978
	// (set) Token: 0x060002BD RID: 701 RVA: 0x00010780 File Offset: 0x0000E980
	public LTDescrOptional optional
	{
		get
		{
			return this._optional;
		}
		set
		{
			this._optional = value;
		}
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0001078C File Offset: 0x0000E98C
	public void reset()
	{
		this.toggle = (this.useRecursion = (this.usesNormalDt = true));
		this.trans = null;
		this.spriteRen = null;
		this.passed = (this.delay = (this.lastVal = 0f));
		this.hasUpdateCallback = (this.useEstimatedTime = (this.useFrames = (this.hasInitiliazed = (this.onCompleteOnRepeat = (this.destroyOnComplete = (this.onCompleteOnStart = (this.useManualTime = (this.hasExtraOnCompletes = false))))))));
		this.easeType = LeanTweenType.linear;
		this.loopType = LeanTweenType.once;
		this.loopCount = 0;
		this.direction = (this.directionLast = (this.overshoot = (this.scale = 1f)));
		this.period = 0.3f;
		this.speed = -1f;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeLinear);
		this.from = (this.to = Vector3.zero);
		this._optional.reset();
	}

	// Token: 0x060002BF RID: 703 RVA: 0x000108AF File Offset: 0x0000EAAF
	public LTDescr setFollow()
	{
		this.type = TweenAction.FOLLOW;
		return this;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x000108BA File Offset: 0x0000EABA
	public LTDescr setMoveX()
	{
		this.type = TweenAction.MOVE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.easeMethod().x, this.trans.position.y, this.trans.position.z);
		};
		return this;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x000108E8 File Offset: 0x0000EAE8
	public LTDescr setMoveY()
	{
		this.type = TweenAction.MOVE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.trans.position.x, this.easeMethod().x, this.trans.position.z);
		};
		return this;
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00010916 File Offset: 0x0000EB16
	public LTDescr setMoveZ()
	{
		this.type = TweenAction.MOVE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.trans.position.x, this.trans.position.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00010944 File Offset: 0x0000EB44
	public LTDescr setMoveLocalX()
	{
		this.type = TweenAction.MOVE_LOCAL_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.easeMethod().x, this.trans.localPosition.y, this.trans.localPosition.z);
		};
		return this;
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00010972 File Offset: 0x0000EB72
	public LTDescr setMoveLocalY()
	{
		this.type = TweenAction.MOVE_LOCAL_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.trans.localPosition.x, this.easeMethod().x, this.trans.localPosition.z);
		};
		return this;
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x000109A0 File Offset: 0x0000EBA0
	public LTDescr setMoveLocalZ()
	{
		this.type = TweenAction.MOVE_LOCAL_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.trans.localPosition.x, this.trans.localPosition.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x000109CE File Offset: 0x0000EBCE
	private void initFromInternal()
	{
		this.fromInternal.x = 0f;
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x000109E0 File Offset: 0x0000EBE0
	public LTDescr setOffset(Vector3 offset)
	{
		this.toInternal = offset;
		return this;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x000109EA File Offset: 0x0000EBEA
	public LTDescr setMoveCurved()
	{
		this.type = TweenAction.MOVE_CURVED;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.path.orientToPath)
			{
				this.trans.position = this._optional.path.point(LTDescr.val);
				return;
			}
			if (this._optional.path.orientToPath2d)
			{
				this._optional.path.place2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.path.place(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00010A18 File Offset: 0x0000EC18
	public LTDescr setMoveCurvedLocal()
	{
		this.type = TweenAction.MOVE_CURVED_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.path.orientToPath)
			{
				this.trans.localPosition = this._optional.path.point(LTDescr.val);
				return;
			}
			if (this._optional.path.orientToPath2d)
			{
				this._optional.path.placeLocal2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.path.placeLocal(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00010A46 File Offset: 0x0000EC46
	public LTDescr setMoveSpline()
	{
		this.type = TweenAction.MOVE_SPLINE;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.spline.orientToPath)
			{
				this.trans.position = this._optional.spline.point(LTDescr.val);
				return;
			}
			if (this._optional.spline.orientToPath2d)
			{
				this._optional.spline.place2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.spline.place(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00010A74 File Offset: 0x0000EC74
	public LTDescr setMoveSplineLocal()
	{
		this.type = TweenAction.MOVE_SPLINE_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.spline.orientToPath)
			{
				this.trans.localPosition = this._optional.spline.point(LTDescr.val);
				return;
			}
			if (this._optional.spline.orientToPath2d)
			{
				this._optional.spline.placeLocal2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.spline.placeLocal(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00010AA3 File Offset: 0x0000ECA3
	public LTDescr setScaleX()
	{
		this.type = TweenAction.SCALE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.easeMethod().x, this.trans.localScale.y, this.trans.localScale.z);
		};
		return this;
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00010AD2 File Offset: 0x0000ECD2
	public LTDescr setScaleY()
	{
		this.type = TweenAction.SCALE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.trans.localScale.x, this.easeMethod().x, this.trans.localScale.z);
		};
		return this;
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00010B01 File Offset: 0x0000ED01
	public LTDescr setScaleZ()
	{
		this.type = TweenAction.SCALE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.trans.localScale.x, this.trans.localScale.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00010B30 File Offset: 0x0000ED30
	public LTDescr setRotateX()
	{
		this.type = TweenAction.ROTATE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.x;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.easeMethod().x, this.trans.eulerAngles.y, this.trans.eulerAngles.z);
		};
		return this;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00010B5F File Offset: 0x0000ED5F
	public LTDescr setRotateY()
	{
		this.type = TweenAction.ROTATE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.y;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.trans.eulerAngles.x, this.easeMethod().x, this.trans.eulerAngles.z);
		};
		return this;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00010B8E File Offset: 0x0000ED8E
	public LTDescr setRotateZ()
	{
		this.type = TweenAction.ROTATE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.z;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.trans.eulerAngles.x, this.trans.eulerAngles.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00010BBD File Offset: 0x0000EDBD
	public LTDescr setRotateAround()
	{
		this.type = TweenAction.ROTATE_AROUND;
		this.initInternal = delegate()
		{
			this.fromInternal.x = 0f;
			this._optional.origRotation = this.trans.rotation;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Vector3 localPosition = this.trans.localPosition;
			Vector3 point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this._optional.axis, -this._optional.lastVal);
			Vector3 b = localPosition - this.trans.localPosition;
			this.trans.localPosition = localPosition - b;
			this.trans.rotation = this._optional.origRotation;
			point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this._optional.axis, LTDescr.val);
			this._optional.lastVal = LTDescr.val;
		};
		return this;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00010BEC File Offset: 0x0000EDEC
	public LTDescr setRotateAroundLocal()
	{
		this.type = TweenAction.ROTATE_AROUND_LOCAL;
		this.initInternal = delegate()
		{
			this.fromInternal.x = 0f;
			this._optional.origRotation = this.trans.localRotation;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Vector3 localPosition = this.trans.localPosition;
			this.trans.RotateAround(this.trans.TransformPoint(this._optional.point), this.trans.TransformDirection(this._optional.axis), -this._optional.lastVal);
			Vector3 b = localPosition - this.trans.localPosition;
			this.trans.localPosition = localPosition - b;
			this.trans.localRotation = this._optional.origRotation;
			Vector3 point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this.trans.TransformDirection(this._optional.axis), LTDescr.val);
			this._optional.lastVal = LTDescr.val;
		};
		return this;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00010C1B File Offset: 0x0000EE1B
	public LTDescr setAlpha()
	{
		this.type = TweenAction.ALPHA;
		this.initInternal = delegate()
		{
			SpriteRenderer component = this.trans.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.fromInternal.x = component.color.a;
			}
			else if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				this.fromInternal.x = this.trans.GetComponent<Renderer>().material.color.a;
			}
			else if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color = this.trans.GetComponent<Renderer>().material.GetColor("_TintColor");
				this.fromInternal.x = color.a;
			}
			else if (this.trans.childCount > 0)
			{
				foreach (object obj in this.trans)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Renderer>() != null)
					{
						Color color2 = transform.gameObject.GetComponent<Renderer>().material.color;
						this.fromInternal.x = color2.a;
						break;
					}
				}
			}
			this.easeInternal = delegate()
			{
				LTDescr.val = this.easeMethod().x;
				if (this.spriteRen != null)
				{
					this.spriteRen.color = new Color(this.spriteRen.color.r, this.spriteRen.color.g, this.spriteRen.color.b, LTDescr.val);
					LTDescr.alphaRecursiveSprite(this.trans, LTDescr.val);
					return;
				}
				LTDescr.alphaRecursive(this.trans, LTDescr.val, this.useRecursion);
			};
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (this.spriteRen != null)
			{
				this.spriteRen.color = new Color(this.spriteRen.color.r, this.spriteRen.color.g, this.spriteRen.color.b, LTDescr.val);
				LTDescr.alphaRecursiveSprite(this.trans, LTDescr.val);
				return;
			}
			LTDescr.alphaRecursive(this.trans, LTDescr.val, this.useRecursion);
		};
		return this;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00010C4A File Offset: 0x0000EE4A
	public LTDescr setTextAlpha()
	{
		this.type = TweenAction.TEXT_ALPHA;
		this.initInternal = delegate()
		{
			this.uiText = this.trans.GetComponent<Text>();
			this.fromInternal.x = ((this.uiText != null) ? this.uiText.color.a : 1f);
		};
		this.easeInternal = delegate()
		{
			LTDescr.textAlphaRecursive(this.trans, this.easeMethod().x, this.useRecursion);
		};
		return this;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00010C79 File Offset: 0x0000EE79
	public LTDescr setAlphaVertex()
	{
		this.type = TweenAction.ALPHA_VERTEX;
		this.initInternal = delegate()
		{
			this.fromInternal.x = (float)this.trans.GetComponent<MeshFilter>().mesh.colors32[0].a;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Mesh mesh = this.trans.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Color32[] array = new Color32[vertices.Length];
			if (array.Length == 0)
			{
				Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
				array = new Color32[mesh.vertices.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = color;
				}
				mesh.colors32 = array;
			}
			Color32 color2 = mesh.colors32[0];
			color2 = new Color((float)color2.r, (float)color2.g, (float)color2.b, LTDescr.val);
			for (int j = 0; j < vertices.Length; j++)
			{
				array[j] = color2;
			}
			mesh.colors32 = array;
		};
		return this;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00010CA8 File Offset: 0x0000EEA8
	public LTDescr setColor()
	{
		this.type = TweenAction.COLOR;
		this.initInternal = delegate()
		{
			SpriteRenderer component = this.trans.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.setFromColor(component.color);
				return;
			}
			if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				Color color = this.trans.GetComponent<Renderer>().material.color;
				this.setFromColor(color);
				return;
			}
			if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color2 = this.trans.GetComponent<Renderer>().material.GetColor("_TintColor");
				this.setFromColor(color2);
				return;
			}
			if (this.trans.childCount > 0)
			{
				foreach (object obj in this.trans)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Renderer>() != null)
					{
						Color color3 = transform.gameObject.GetComponent<Renderer>().material.color;
						this.setFromColor(color3);
						break;
					}
				}
			}
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.spriteRen != null)
			{
				this.spriteRen.color = color;
				LTDescr.colorRecursiveSprite(this.trans, color);
			}
			else if (this.type == TweenAction.COLOR)
			{
				LTDescr.colorRecursive(this.trans, color, this.useRecursion);
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
				return;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColorObject != null)
			{
				this._optional.onUpdateColorObject(color, this._optional.onUpdateParam);
			}
		};
		return this;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00010CD7 File Offset: 0x0000EED7
	public LTDescr setCallbackColor()
	{
		this.type = TweenAction.CALLBACK_COLOR;
		this.initInternal = delegate()
		{
			this.diff = new Vector3(1f, 0f, 0f);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.spriteRen != null)
			{
				this.spriteRen.color = color;
				LTDescr.colorRecursiveSprite(this.trans, color);
			}
			else if (this.type == TweenAction.COLOR)
			{
				LTDescr.colorRecursive(this.trans, color, this.useRecursion);
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
				return;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColorObject != null)
			{
				this._optional.onUpdateColorObject(color, this._optional.onUpdateParam);
			}
		};
		return this;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00010D06 File Offset: 0x0000EF06
	public LTDescr setTextColor()
	{
		this.type = TweenAction.TEXT_COLOR;
		this.initInternal = delegate()
		{
			this.uiText = this.trans.GetComponent<Text>();
			this.setFromColor((this.uiText != null) ? this.uiText.color : Color.white);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			this.uiText.color = color;
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
			}
			if (this.useRecursion && this.trans.childCount > 0)
			{
				LTDescr.textColorRecursive(this.trans, color);
			}
		};
		return this;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00010D35 File Offset: 0x0000EF35
	public LTDescr setCanvasAlpha()
	{
		this.type = TweenAction.CANVAS_ALPHA;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			if (this.uiImage != null)
			{
				this.fromInternal.x = this.uiImage.color.a;
				return;
			}
			this.rawImage = this.trans.GetComponent<RawImage>();
			if (this.rawImage != null)
			{
				this.fromInternal.x = this.rawImage.color.a;
				return;
			}
			this.fromInternal.x = 1f;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (this.uiImage != null)
			{
				Color color = this.uiImage.color;
				color.a = LTDescr.val;
				this.uiImage.color = color;
			}
			else if (this.rawImage != null)
			{
				Color color2 = this.rawImage.color;
				color2.a = LTDescr.val;
				this.rawImage.color = color2;
			}
			if (this.useRecursion)
			{
				LTDescr.alphaRecursive(this.rectTransform, LTDescr.val, 0);
				LTDescr.textAlphaChildrenRecursive(this.rectTransform, LTDescr.val, true);
			}
		};
		return this;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00010D64 File Offset: 0x0000EF64
	public LTDescr setCanvasGroupAlpha()
	{
		this.type = TweenAction.CANVASGROUP_ALPHA;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.GetComponent<CanvasGroup>().alpha;
		};
		this.easeInternal = delegate()
		{
			this.trans.GetComponent<CanvasGroup>().alpha = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00010D93 File Offset: 0x0000EF93
	public LTDescr setCanvasColor()
	{
		this.type = TweenAction.CANVAS_COLOR;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			if (this.uiImage == null)
			{
				this.rawImage = this.trans.GetComponent<RawImage>();
				this.setFromColor((this.rawImage != null) ? this.rawImage.color : Color.white);
				return;
			}
			this.setFromColor(this.uiImage.color);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.uiImage != null)
			{
				this.uiImage.color = color;
			}
			else if (this.rawImage != null)
			{
				this.rawImage.color = color;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
			}
			if (this.useRecursion)
			{
				LTDescr.colorRecursive(this.rectTransform, color);
			}
		};
		return this;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00010DC2 File Offset: 0x0000EFC2
	public LTDescr setCanvasMoveX()
	{
		this.type = TweenAction.CANVAS_MOVE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.x;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(this.easeMethod().x, anchoredPosition3D.y, anchoredPosition3D.z);
		};
		return this;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00010DF1 File Offset: 0x0000EFF1
	public LTDescr setCanvasMoveY()
	{
		this.type = TweenAction.CANVAS_MOVE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.y;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, this.easeMethod().x, anchoredPosition3D.z);
		};
		return this;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00010E20 File Offset: 0x0000F020
	public LTDescr setCanvasMoveZ()
	{
		this.type = TweenAction.CANVAS_MOVE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.z;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, anchoredPosition3D.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00010E4F File Offset: 0x0000F04F
	private void initCanvasRotateAround()
	{
		this.lastVal = 0f;
		this.fromInternal.x = 0f;
		this._optional.origRotation = this.rectTransform.rotation;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00010E82 File Offset: 0x0000F082
	public LTDescr setCanvasRotateAround()
	{
		this.type = TweenAction.CANVAS_ROTATEAROUND;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initCanvasRotateAround);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			RectTransform rectTransform = this.rectTransform;
			Vector3 localPosition = rectTransform.localPosition;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), this._optional.axis, -LTDescr.val);
			Vector3 b = localPosition - rectTransform.localPosition;
			rectTransform.localPosition = localPosition - b;
			rectTransform.rotation = this._optional.origRotation;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), this._optional.axis, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00010EB1 File Offset: 0x0000F0B1
	public LTDescr setCanvasRotateAroundLocal()
	{
		this.type = TweenAction.CANVAS_ROTATEAROUND_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initCanvasRotateAround);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			RectTransform rectTransform = this.rectTransform;
			Vector3 localPosition = rectTransform.localPosition;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), rectTransform.TransformDirection(this._optional.axis), -LTDescr.val);
			Vector3 b = localPosition - rectTransform.localPosition;
			rectTransform.localPosition = localPosition - b;
			rectTransform.rotation = this._optional.origRotation;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), rectTransform.TransformDirection(this._optional.axis), LTDescr.val);
		};
		return this;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00010EE0 File Offset: 0x0000F0E0
	public LTDescr setCanvasPlaySprite()
	{
		this.type = TweenAction.CANVAS_PLAYSPRITE;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			this.fromInternal.x = 0f;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			int num = (int)Mathf.Round(LTDescr.val);
			this.uiImage.sprite = this.sprites[num];
		};
		return this;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00010F0F File Offset: 0x0000F10F
	public LTDescr setCanvasMove()
	{
		this.type = TweenAction.CANVAS_MOVE;
		this.initInternal = delegate()
		{
			this.fromInternal = this.rectTransform.anchoredPosition3D;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.anchoredPosition3D = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00010F3E File Offset: 0x0000F13E
	public LTDescr setCanvasScale()
	{
		this.type = TweenAction.CANVAS_SCALE;
		this.initInternal = delegate()
		{
			this.from = this.rectTransform.localScale;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.localScale = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00010F6D File Offset: 0x0000F16D
	public LTDescr setCanvasSizeDelta()
	{
		this.type = TweenAction.CANVAS_SIZEDELTA;
		this.initInternal = delegate()
		{
			this.from = this.rectTransform.sizeDelta;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.sizeDelta = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x00010F9C File Offset: 0x0000F19C
	private void callback()
	{
		LTDescr.newVect = this.easeMethod();
		LTDescr.val = LTDescr.newVect.x;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00010FC0 File Offset: 0x0000F1C0
	public LTDescr setCallback()
	{
		this.type = TweenAction.CALLBACK;
		this.initInternal = delegate()
		{
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00011010 File Offset: 0x0000F210
	public LTDescr setValue3()
	{
		this.type = TweenAction.VALUE3;
		this.initInternal = delegate()
		{
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0001105D File Offset: 0x0000F25D
	public LTDescr setMove()
	{
		this.type = TweenAction.MOVE;
		this.initInternal = delegate()
		{
			this.from = this.trans.position;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.position = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001108C File Offset: 0x0000F28C
	public LTDescr setMoveLocal()
	{
		this.type = TweenAction.MOVE_LOCAL;
		this.initInternal = delegate()
		{
			this.from = this.trans.localPosition;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localPosition = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000110BB File Offset: 0x0000F2BB
	public LTDescr setMoveToTransform()
	{
		this.type = TweenAction.MOVE_TO_TRANSFORM;
		this.initInternal = delegate()
		{
			this.from = this.trans.position;
		};
		this.easeInternal = delegate()
		{
			this.to = this._optional.toTrans.position;
			this.diff = this.to - this.from;
			this.diffDiv2 = this.diff * 0.5f;
			LTDescr.newVect = this.easeMethod();
			this.trans.position = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x000110EA File Offset: 0x0000F2EA
	public LTDescr setRotate()
	{
		this.type = TweenAction.ROTATE;
		this.initInternal = delegate()
		{
			this.from = this.trans.eulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.fromInternal.x, this.toInternal.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.eulerAngles = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00011119 File Offset: 0x0000F319
	public LTDescr setRotateLocal()
	{
		this.type = TweenAction.ROTATE_LOCAL;
		this.initInternal = delegate()
		{
			this.from = this.trans.localEulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.fromInternal.x, this.toInternal.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localEulerAngles = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00011148 File Offset: 0x0000F348
	public LTDescr setScale()
	{
		this.type = TweenAction.SCALE;
		this.initInternal = delegate()
		{
			this.from = this.trans.localScale;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localScale = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00011177 File Offset: 0x0000F377
	public LTDescr setGUIMove()
	{
		this.type = TweenAction.GUI_MOVE;
		this.initInternal = delegate()
		{
			this.from = new Vector3(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, 0f);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.rect = new Rect(vector.x, vector.y, this._optional.ltRect.rect.width, this._optional.ltRect.rect.height);
		};
		return this;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000111A6 File Offset: 0x0000F3A6
	public LTDescr setGUIMoveMargin()
	{
		this.type = TweenAction.GUI_MOVE_MARGIN;
		this.initInternal = delegate()
		{
			this.from = new Vector2(this._optional.ltRect.margin.x, this._optional.ltRect.margin.y);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.margin = new Vector2(vector.x, vector.y);
		};
		return this;
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x000111D5 File Offset: 0x0000F3D5
	public LTDescr setGUIScale()
	{
		this.type = TweenAction.GUI_SCALE;
		this.initInternal = delegate()
		{
			this.from = new Vector3(this._optional.ltRect.rect.width, this._optional.ltRect.rect.height, 0f);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.rect = new Rect(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, vector.x, vector.y);
		};
		return this;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00011204 File Offset: 0x0000F404
	public LTDescr setGUIAlpha()
	{
		this.type = TweenAction.GUI_ALPHA;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this._optional.ltRect.alpha;
		};
		this.easeInternal = delegate()
		{
			this._optional.ltRect.alpha = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00011233 File Offset: 0x0000F433
	public LTDescr setGUIRotate()
	{
		this.type = TweenAction.GUI_ROTATE;
		this.initInternal = delegate()
		{
			if (!this._optional.ltRect.rotateEnabled)
			{
				this._optional.ltRect.rotateEnabled = true;
				this._optional.ltRect.resetForRotation();
			}
			this.fromInternal.x = this._optional.ltRect.rotation;
		};
		this.easeInternal = delegate()
		{
			this._optional.ltRect.rotation = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00011262 File Offset: 0x0000F462
	public LTDescr setDelayedSound()
	{
		this.type = TweenAction.DELAYED_SOUND;
		this.initInternal = delegate()
		{
			this.hasExtraOnCompletes = true;
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00011291 File Offset: 0x0000F491
	public LTDescr setTarget(Transform trans)
	{
		this.optional.toTrans = trans;
		return this;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x000112A0 File Offset: 0x0000F4A0
	private void init()
	{
		this.hasInitiliazed = true;
		this.usesNormalDt = (!this.useEstimatedTime && !this.useManualTime && !this.useFrames);
		if (this.useFrames)
		{
			this.optional.initFrameCount = Time.frameCount;
		}
		if (this.time <= 0f)
		{
			this.time = Mathf.Epsilon;
		}
		if (this.initInternal != null)
		{
			this.initInternal();
		}
		this.diff = this.to - this.from;
		this.diffDiv2 = this.diff * 0.5f;
		if (this._optional.onStart != null)
		{
			this._optional.onStart();
		}
		if (this.onCompleteOnStart)
		{
			this.callOnCompletes();
		}
		if (this.speed >= 0f)
		{
			this.initSpeed();
		}
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00011384 File Offset: 0x0000F584
	private void initSpeed()
	{
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			this.time = this._optional.path.distance / this.speed;
			return;
		}
		if (this.type == TweenAction.MOVE_SPLINE || this.type == TweenAction.MOVE_SPLINE_LOCAL)
		{
			this.time = this._optional.spline.distance / this.speed;
			return;
		}
		this.time = (this.to - this.from).magnitude / this.speed;
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00011418 File Offset: 0x0000F618
	public LTDescr updateNow()
	{
		this.updateInternal();
		return this;
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00011424 File Offset: 0x0000F624
	public bool updateInternal()
	{
		float num = this.direction;
		if (this.usesNormalDt)
		{
			LTDescr.dt = LeanTween.dtActual;
		}
		else if (this.useEstimatedTime)
		{
			LTDescr.dt = LeanTween.dtEstimated;
		}
		else if (this.useFrames)
		{
			LTDescr.dt = (float)((this.optional.initFrameCount == 0) ? 0 : 1);
			this.optional.initFrameCount = Time.frameCount;
		}
		else if (this.useManualTime)
		{
			LTDescr.dt = LeanTween.dtManual;
		}
		if (this.delay <= 0f && num != 0f)
		{
			if (this.trans == null)
			{
				return true;
			}
			if (!this.hasInitiliazed)
			{
				this.init();
			}
			LTDescr.dt *= num;
			this.passed += LTDescr.dt;
			this.passed = Mathf.Clamp(this.passed, 0f, this.time);
			this.ratioPassed = this.passed / this.time;
			this.easeInternal();
			if (this.hasUpdateCallback)
			{
				this._optional.callOnUpdate(LTDescr.val, this.ratioPassed);
			}
			if ((num > 0f) ? (this.passed >= this.time) : (this.passed <= 0f))
			{
				this.loopCount--;
				if (this.loopType == LeanTweenType.pingPong)
				{
					this.direction = 0f - num;
				}
				else
				{
					this.passed = Mathf.Epsilon;
				}
				bool flag = this.loopCount == 0 || this.loopType == LeanTweenType.once;
				if (!flag && this.onCompleteOnRepeat && this.hasExtraOnCompletes)
				{
					this.callOnCompletes();
				}
				return flag;
			}
		}
		else
		{
			this.delay -= LTDescr.dt;
		}
		return false;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x000115F4 File Offset: 0x0000F7F4
	public void callOnCompletes()
	{
		if (this.type == TweenAction.GUI_ROTATE)
		{
			this._optional.ltRect.rotateFinished = true;
		}
		if (this.type == TweenAction.DELAYED_SOUND)
		{
			AudioSource.PlayClipAtPoint((AudioClip)this._optional.onCompleteParam, this.to, this.from.x);
		}
		if (this._optional.onComplete != null)
		{
			this._optional.onComplete();
			return;
		}
		if (this._optional.onCompleteObject != null)
		{
			this._optional.onCompleteObject(this._optional.onCompleteParam);
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00011694 File Offset: 0x0000F894
	public LTDescr setFromColor(Color col)
	{
		this.from = new Vector3(0f, col.a, 0f);
		this.diff = new Vector3(1f, 0f, 0f);
		this._optional.axis = new Vector3(col.r, col.g, col.b);
		return this;
	}

	// Token: 0x060002FD RID: 765 RVA: 0x000116FC File Offset: 0x0000F8FC
	private static void alphaRecursive(Transform transform, float val, bool useRecursion = true)
	{
		Renderer component = transform.gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			foreach (Material material in component.materials)
			{
				if (material.HasProperty("_Color"))
				{
					material.color = new Color(material.color.r, material.color.g, material.color.b, val);
				}
				else if (material.HasProperty("_TintColor"))
				{
					Color color = material.GetColor("_TintColor");
					material.SetColor("_TintColor", new Color(color.r, color.g, color.b, val));
				}
			}
		}
		if (useRecursion && transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				LTDescr.alphaRecursive((Transform)obj, val, true);
			}
		}
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00011818 File Offset: 0x0000FA18
	private static void colorRecursive(Transform transform, Color toColor, bool useRecursion = true)
	{
		Renderer component = transform.gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			Material[] materials = component.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].color = toColor;
			}
		}
		if (useRecursion && transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				LTDescr.colorRecursive((Transform)obj, toColor, true);
			}
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x000118B0 File Offset: 0x0000FAB0
	private static void alphaRecursive(RectTransform rectTransform, float val, int recursiveLevel = 0)
	{
		if (rectTransform.childCount > 0)
		{
			foreach (object obj in rectTransform)
			{
				RectTransform rectTransform2 = (RectTransform)obj;
				MaskableGraphic component = rectTransform2.GetComponent<Image>();
				if (component != null)
				{
					Color color = component.color;
					color.a = val;
					component.color = color;
				}
				else
				{
					component = rectTransform2.GetComponent<RawImage>();
					if (component != null)
					{
						Color color2 = component.color;
						color2.a = val;
						component.color = color2;
					}
				}
				LTDescr.alphaRecursive(rectTransform2, val, recursiveLevel + 1);
			}
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00011968 File Offset: 0x0000FB68
	private static void alphaRecursiveSprite(Transform transform, float val)
	{
		if (transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				SpriteRenderer component = transform2.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					component.color = new Color(component.color.r, component.color.g, component.color.b, val);
				}
				LTDescr.alphaRecursiveSprite(transform2, val);
			}
		}
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00011A00 File Offset: 0x0000FC00
	private static void colorRecursiveSprite(Transform transform, Color toColor)
	{
		if (transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				SpriteRenderer component = transform.gameObject.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					component.color = toColor;
				}
				LTDescr.colorRecursiveSprite(transform2, toColor);
			}
		}
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00011A78 File Offset: 0x0000FC78
	private static void colorRecursive(RectTransform rectTransform, Color toColor)
	{
		if (rectTransform.childCount > 0)
		{
			foreach (object obj in rectTransform)
			{
				RectTransform rectTransform2 = (RectTransform)obj;
				MaskableGraphic component = rectTransform2.GetComponent<Image>();
				if (component != null)
				{
					component.color = toColor;
				}
				else
				{
					component = rectTransform2.GetComponent<RawImage>();
					if (component != null)
					{
						component.color = toColor;
					}
				}
				LTDescr.colorRecursive(rectTransform2, toColor);
			}
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00011B08 File Offset: 0x0000FD08
	private static void textAlphaChildrenRecursive(Transform trans, float val, bool useRecursion = true)
	{
		if (useRecursion && trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				Transform transform = (Transform)obj;
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					Color color = component.color;
					color.a = val;
					component.color = color;
				}
				LTDescr.textAlphaChildrenRecursive(transform, val, true);
			}
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00011B90 File Offset: 0x0000FD90
	private static void textAlphaRecursive(Transform trans, float val, bool useRecursion = true)
	{
		Text component = trans.GetComponent<Text>();
		if (component != null)
		{
			Color color = component.color;
			color.a = val;
			component.color = color;
		}
		if (useRecursion && trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				LTDescr.textAlphaRecursive((Transform)obj, val, true);
			}
		}
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00011C18 File Offset: 0x0000FE18
	private static void textColorRecursive(Transform trans, Color toColor)
	{
		if (trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				Transform transform = (Transform)obj;
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					component.color = toColor;
				}
				LTDescr.textColorRecursive(transform, toColor);
			}
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private static Color tweenColor(LTDescr tween, float val)
	{
		Vector3 vector = tween._optional.point - tween._optional.axis;
		float num = tween.to.y - tween.from.y;
		return new Color(tween._optional.axis.x + vector.x * val, tween._optional.axis.y + vector.y * val, tween._optional.axis.z + vector.z * val, tween.from.y + num * val);
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00011D2C File Offset: 0x0000FF2C
	public LTDescr pause()
	{
		if (this.direction != 0f)
		{
			this.directionLast = this.direction;
			this.direction = 0f;
		}
		return this;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00011D53 File Offset: 0x0000FF53
	public LTDescr resume()
	{
		this.direction = this.directionLast;
		return this;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00011D62 File Offset: 0x0000FF62
	public LTDescr setAxis(Vector3 axis)
	{
		this._optional.axis = axis;
		return this;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00011D71 File Offset: 0x0000FF71
	public LTDescr setDelay(float delay)
	{
		this.delay = delay;
		return this;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00011D7C File Offset: 0x0000FF7C
	public LTDescr setEase(LeanTweenType easeType)
	{
		switch (easeType)
		{
		case LeanTweenType.linear:
			this.setEaseLinear();
			break;
		case LeanTweenType.easeOutQuad:
			this.setEaseOutQuad();
			break;
		case LeanTweenType.easeInQuad:
			this.setEaseInQuad();
			break;
		case LeanTweenType.easeInOutQuad:
			this.setEaseInOutQuad();
			break;
		case LeanTweenType.easeInCubic:
			this.setEaseInCubic();
			break;
		case LeanTweenType.easeOutCubic:
			this.setEaseOutCubic();
			break;
		case LeanTweenType.easeInOutCubic:
			this.setEaseInOutCubic();
			break;
		case LeanTweenType.easeInQuart:
			this.setEaseInQuart();
			break;
		case LeanTweenType.easeOutQuart:
			this.setEaseOutQuart();
			break;
		case LeanTweenType.easeInOutQuart:
			this.setEaseInOutQuart();
			break;
		case LeanTweenType.easeInQuint:
			this.setEaseInQuint();
			break;
		case LeanTweenType.easeOutQuint:
			this.setEaseOutQuint();
			break;
		case LeanTweenType.easeInOutQuint:
			this.setEaseInOutQuint();
			break;
		case LeanTweenType.easeInSine:
			this.setEaseInSine();
			break;
		case LeanTweenType.easeOutSine:
			this.setEaseOutSine();
			break;
		case LeanTweenType.easeInOutSine:
			this.setEaseInOutSine();
			break;
		case LeanTweenType.easeInExpo:
			this.setEaseInExpo();
			break;
		case LeanTweenType.easeOutExpo:
			this.setEaseOutExpo();
			break;
		case LeanTweenType.easeInOutExpo:
			this.setEaseInOutExpo();
			break;
		case LeanTweenType.easeInCirc:
			this.setEaseInCirc();
			break;
		case LeanTweenType.easeOutCirc:
			this.setEaseOutCirc();
			break;
		case LeanTweenType.easeInOutCirc:
			this.setEaseInOutCirc();
			break;
		case LeanTweenType.easeInBounce:
			this.setEaseInBounce();
			break;
		case LeanTweenType.easeOutBounce:
			this.setEaseOutBounce();
			break;
		case LeanTweenType.easeInOutBounce:
			this.setEaseInOutBounce();
			break;
		case LeanTweenType.easeInBack:
			this.setEaseInBack();
			break;
		case LeanTweenType.easeOutBack:
			this.setEaseOutBack();
			break;
		case LeanTweenType.easeInOutBack:
			this.setEaseInOutBack();
			break;
		case LeanTweenType.easeInElastic:
			this.setEaseInElastic();
			break;
		case LeanTweenType.easeOutElastic:
			this.setEaseOutElastic();
			break;
		case LeanTweenType.easeInOutElastic:
			this.setEaseInOutElastic();
			break;
		case LeanTweenType.easeSpring:
			this.setEaseSpring();
			break;
		case LeanTweenType.easeShake:
			this.setEaseShake();
			break;
		case LeanTweenType.punch:
			this.setEasePunch();
			break;
		default:
			this.setEaseLinear();
			break;
		}
		return this;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00011F94 File Offset: 0x00010194
	public LTDescr setEaseLinear()
	{
		this.easeType = LeanTweenType.linear;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeLinear);
		return this;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00011FB0 File Offset: 0x000101B0
	public LTDescr setEaseSpring()
	{
		this.easeType = LeanTweenType.easeSpring;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeSpring);
		return this;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00011FCD File Offset: 0x000101CD
	public LTDescr setEaseInQuad()
	{
		this.easeType = LeanTweenType.easeInQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuad);
		return this;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00011FE9 File Offset: 0x000101E9
	public LTDescr setEaseOutQuad()
	{
		this.easeType = LeanTweenType.easeOutQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuad);
		return this;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00012005 File Offset: 0x00010205
	public LTDescr setEaseInOutQuad()
	{
		this.easeType = LeanTweenType.easeInOutQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuad);
		return this;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00012021 File Offset: 0x00010221
	public LTDescr setEaseInCubic()
	{
		this.easeType = LeanTweenType.easeInCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInCubic);
		return this;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0001203D File Offset: 0x0001023D
	public LTDescr setEaseOutCubic()
	{
		this.easeType = LeanTweenType.easeOutCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutCubic);
		return this;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00012059 File Offset: 0x00010259
	public LTDescr setEaseInOutCubic()
	{
		this.easeType = LeanTweenType.easeInOutCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutCubic);
		return this;
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00012075 File Offset: 0x00010275
	public LTDescr setEaseInQuart()
	{
		this.easeType = LeanTweenType.easeInQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuart);
		return this;
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00012091 File Offset: 0x00010291
	public LTDescr setEaseOutQuart()
	{
		this.easeType = LeanTweenType.easeOutQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuart);
		return this;
	}

	// Token: 0x06000316 RID: 790 RVA: 0x000120AE File Offset: 0x000102AE
	public LTDescr setEaseInOutQuart()
	{
		this.easeType = LeanTweenType.easeInOutQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuart);
		return this;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x000120CB File Offset: 0x000102CB
	public LTDescr setEaseInQuint()
	{
		this.easeType = LeanTweenType.easeInQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuint);
		return this;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x000120E8 File Offset: 0x000102E8
	public LTDescr setEaseOutQuint()
	{
		this.easeType = LeanTweenType.easeOutQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuint);
		return this;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00012105 File Offset: 0x00010305
	public LTDescr setEaseInOutQuint()
	{
		this.easeType = LeanTweenType.easeInOutQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuint);
		return this;
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00012122 File Offset: 0x00010322
	public LTDescr setEaseInSine()
	{
		this.easeType = LeanTweenType.easeInSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInSine);
		return this;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0001213F File Offset: 0x0001033F
	public LTDescr setEaseOutSine()
	{
		this.easeType = LeanTweenType.easeOutSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutSine);
		return this;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0001215C File Offset: 0x0001035C
	public LTDescr setEaseInOutSine()
	{
		this.easeType = LeanTweenType.easeInOutSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutSine);
		return this;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00012179 File Offset: 0x00010379
	public LTDescr setEaseInExpo()
	{
		this.easeType = LeanTweenType.easeInExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInExpo);
		return this;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00012196 File Offset: 0x00010396
	public LTDescr setEaseOutExpo()
	{
		this.easeType = LeanTweenType.easeOutExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutExpo);
		return this;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x000121B3 File Offset: 0x000103B3
	public LTDescr setEaseInOutExpo()
	{
		this.easeType = LeanTweenType.easeInOutExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutExpo);
		return this;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x000121D0 File Offset: 0x000103D0
	public LTDescr setEaseInCirc()
	{
		this.easeType = LeanTweenType.easeInCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInCirc);
		return this;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x000121ED File Offset: 0x000103ED
	public LTDescr setEaseOutCirc()
	{
		this.easeType = LeanTweenType.easeOutCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutCirc);
		return this;
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0001220A File Offset: 0x0001040A
	public LTDescr setEaseInOutCirc()
	{
		this.easeType = LeanTweenType.easeInOutCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutCirc);
		return this;
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00012227 File Offset: 0x00010427
	public LTDescr setEaseInBounce()
	{
		this.easeType = LeanTweenType.easeInBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInBounce);
		return this;
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00012244 File Offset: 0x00010444
	public LTDescr setEaseOutBounce()
	{
		this.easeType = LeanTweenType.easeOutBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutBounce);
		return this;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00012261 File Offset: 0x00010461
	public LTDescr setEaseInOutBounce()
	{
		this.easeType = LeanTweenType.easeInOutBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutBounce);
		return this;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0001227E File Offset: 0x0001047E
	public LTDescr setEaseInBack()
	{
		this.easeType = LeanTweenType.easeInBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInBack);
		return this;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0001229B File Offset: 0x0001049B
	public LTDescr setEaseOutBack()
	{
		this.easeType = LeanTweenType.easeOutBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutBack);
		return this;
	}

	// Token: 0x06000328 RID: 808 RVA: 0x000122B8 File Offset: 0x000104B8
	public LTDescr setEaseInOutBack()
	{
		this.easeType = LeanTweenType.easeInOutBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutBack);
		return this;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x000122D5 File Offset: 0x000104D5
	public LTDescr setEaseInElastic()
	{
		this.easeType = LeanTweenType.easeInElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInElastic);
		return this;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x000122F2 File Offset: 0x000104F2
	public LTDescr setEaseOutElastic()
	{
		this.easeType = LeanTweenType.easeOutElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutElastic);
		return this;
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0001230F File Offset: 0x0001050F
	public LTDescr setEaseInOutElastic()
	{
		this.easeType = LeanTweenType.easeInOutElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutElastic);
		return this;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001232C File Offset: 0x0001052C
	public LTDescr setEasePunch()
	{
		this._optional.animationCurve = LeanTween.punch;
		this.toInternal.x = this.from.x + this.to.x;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		return this;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00012380 File Offset: 0x00010580
	public LTDescr setEaseShake()
	{
		this._optional.animationCurve = LeanTween.shake;
		this.toInternal.x = this.from.x + this.to.x;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		return this;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000123D4 File Offset: 0x000105D4
	private Vector3 tweenOnCurve()
	{
		return new Vector3(this.from.x + this.diff.x * this._optional.animationCurve.Evaluate(this.ratioPassed), this.from.y + this.diff.y * this._optional.animationCurve.Evaluate(this.ratioPassed), this.from.z + this.diff.z * this._optional.animationCurve.Evaluate(this.ratioPassed));
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00012470 File Offset: 0x00010670
	private Vector3 easeInOutQuad()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val *= LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val = (1f - LTDescr.val) * (LTDescr.val - 3f) + 1f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00012588 File Offset: 0x00010788
	private Vector3 easeInQuad()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00012603 File Offset: 0x00010803
	private Vector3 easeOutQuad()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = -LTDescr.val * (LTDescr.val - 2f);
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00012644 File Offset: 0x00010844
	private Vector3 easeLinear()
	{
		LTDescr.val = this.ratioPassed;
		return new Vector3(this.from.x + this.diff.x * LTDescr.val, this.from.y + this.diff.y * LTDescr.val, this.from.z + this.diff.z * LTDescr.val);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x000126B8 File Offset: 0x000108B8
	private Vector3 easeSpring()
	{
		LTDescr.val = Mathf.Clamp01(this.ratioPassed);
		LTDescr.val = (Mathf.Sin(LTDescr.val * 3.1415927f * (0.2f + 2.5f * LTDescr.val * LTDescr.val * LTDescr.val)) * Mathf.Pow(1f - LTDescr.val, 2.2f) + LTDescr.val) * (1f + 1.2f * (1f - LTDescr.val));
		return this.from + this.diff * LTDescr.val;
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00012758 File Offset: 0x00010958
	private Vector3 easeInCubic()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed * this.ratioPassed;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000335 RID: 821 RVA: 0x000127DC File Offset: 0x000109DC
	private Vector3 easeOutCubic()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00012874 File Offset: 0x00010A74
	private Vector3 easeInOutCubic()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val + 2f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00012999 File Offset: 0x00010B99
	private Vector3 easeInQuart()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed * this.ratioPassed * this.ratioPassed;
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x000129D8 File Offset: 0x00010BD8
	private Vector3 easeOutQuart()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = -(LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val - 1f);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00012A78 File Offset: 0x00010C78
	private Vector3 easeInOutQuart()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		return -this.diffDiv2 * (LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val - 2f) + this.from;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00012B64 File Offset: 0x00010D64
	private Vector3 easeInQuint()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00012BFC File Offset: 0x00010DFC
	private Vector3 easeOutQuint()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00012CA0 File Offset: 0x00010EA0
	private Vector3 easeInOutQuint()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val + 2f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00012DE0 File Offset: 0x00010FE0
	private Vector3 easeInSine()
	{
		LTDescr.val = -Mathf.Cos(this.ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(this.diff.x * LTDescr.val + this.diff.x + this.from.x, this.diff.y * LTDescr.val + this.diff.y + this.from.y, this.diff.z * LTDescr.val + this.diff.z + this.from.z);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00012E84 File Offset: 0x00011084
	private Vector3 easeOutSine()
	{
		LTDescr.val = Mathf.Sin(this.ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00012F04 File Offset: 0x00011104
	private Vector3 easeInOutSine()
	{
		LTDescr.val = -(Mathf.Cos(3.1415927f * this.ratioPassed) - 1f);
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00012F8C File Offset: 0x0001118C
	private Vector3 easeInExpo()
	{
		LTDescr.val = Mathf.Pow(2f, 10f * (this.ratioPassed - 1f));
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00013018 File Offset: 0x00011218
	private Vector3 easeOutExpo()
	{
		LTDescr.val = -Mathf.Pow(2f, -10f * this.ratioPassed) + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000342 RID: 834 RVA: 0x000130A4 File Offset: 0x000112A4
	private Vector3 easeInOutExpo()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			return this.diffDiv2 * Mathf.Pow(2f, 10f * (LTDescr.val - 1f)) + this.from;
		}
		LTDescr.val -= 1f;
		return this.diffDiv2 * (-Mathf.Pow(2f, -10f * LTDescr.val) + 2f) + this.from;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00013144 File Offset: 0x00011344
	private Vector3 easeInCirc()
	{
		LTDescr.val = -(Mathf.Sqrt(1f - this.ratioPassed * this.ratioPassed) - 1f);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000344 RID: 836 RVA: 0x000131D4 File Offset: 0x000113D4
	private Vector3 easeOutCirc()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = Mathf.Sqrt(1f - LTDescr.val * LTDescr.val);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0001326C File Offset: 0x0001146C
	private Vector3 easeInOutCirc()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = -(Mathf.Sqrt(1f - LTDescr.val * LTDescr.val) - 1f);
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = Mathf.Sqrt(1f - LTDescr.val * LTDescr.val) + 1f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x000133A4 File Offset: 0x000115A4
	private Vector3 easeInBounce()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = 1f - LTDescr.val;
		return new Vector3(this.diff.x - LeanTween.easeOutBounce(0f, this.diff.x, LTDescr.val) + this.from.x, this.diff.y - LeanTween.easeOutBounce(0f, this.diff.y, LTDescr.val) + this.from.y, this.diff.z - LeanTween.easeOutBounce(0f, this.diff.z, LTDescr.val) + this.from.z);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00013468 File Offset: 0x00011668
	private Vector3 easeOutBounce()
	{
		LTDescr.val = this.ratioPassed;
		float num;
		float num2;
		if (LTDescr.val < (num = 1f - 1.75f * this.overshoot / 2.75f))
		{
			LTDescr.val = 1f / num / num * LTDescr.val * LTDescr.val;
		}
		else if (LTDescr.val < (num2 = 1f - 0.75f * this.overshoot / 2.75f))
		{
			LTDescr.val -= (num + num2) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.25f * this.overshoot * this.overshoot;
		}
		else if (LTDescr.val < (num = 1f - 0.25f * this.overshoot / 2.75f))
		{
			LTDescr.val -= (num + num2) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.0625f * this.overshoot * this.overshoot;
		}
		else
		{
			LTDescr.val -= (num + 1f) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.015625f * this.overshoot * this.overshoot;
		}
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x06000348 RID: 840 RVA: 0x000135F4 File Offset: 0x000117F4
	private Vector3 easeInOutBounce()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			return new Vector3(LeanTween.easeInBounce(0f, this.diff.x, LTDescr.val) * 0.5f + this.from.x, LeanTween.easeInBounce(0f, this.diff.y, LTDescr.val) * 0.5f + this.from.y, LeanTween.easeInBounce(0f, this.diff.z, LTDescr.val) * 0.5f + this.from.z);
		}
		LTDescr.val -= 1f;
		return new Vector3(LeanTween.easeOutBounce(0f, this.diff.x, LTDescr.val) * 0.5f + this.diffDiv2.x + this.from.x, LeanTween.easeOutBounce(0f, this.diff.y, LTDescr.val) * 0.5f + this.diffDiv2.y + this.from.y, LeanTween.easeOutBounce(0f, this.diff.z, LTDescr.val) * 0.5f + this.diffDiv2.z + this.from.z);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00013768 File Offset: 0x00011968
	private Vector3 easeInBack()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val /= 1f;
		float num = 1.70158f * this.overshoot;
		return this.diff * LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val - num) + this.from;
	}

	// Token: 0x0600034A RID: 842 RVA: 0x000137D8 File Offset: 0x000119D8
	private Vector3 easeOutBack()
	{
		float num = 1.70158f * this.overshoot;
		LTDescr.val = this.ratioPassed / 1f - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val + num) + 1f;
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x0600034B RID: 843 RVA: 0x0001384C File Offset: 0x00011A4C
	private Vector3 easeInOutBack()
	{
		float num = 1.70158f * this.overshoot;
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			num *= 1.525f * this.overshoot;
			return this.diffDiv2 * (LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val - num)) + this.from;
		}
		LTDescr.val -= 2f;
		num *= 1.525f * this.overshoot;
		LTDescr.val = LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val + num) + 2f;
		return this.diffDiv2 * LTDescr.val + this.from;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00013924 File Offset: 0x00011B24
	private Vector3 easeInElastic()
	{
		return new Vector3(LeanTween.easeInElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x0600034D RID: 845 RVA: 0x000139C0 File Offset: 0x00011BC0
	private Vector3 easeOutElastic()
	{
		return new Vector3(LeanTween.easeOutElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeOutElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeOutElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00013A5C File Offset: 0x00011C5C
	private Vector3 easeInOutElastic()
	{
		return new Vector3(LeanTween.easeInOutElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInOutElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInOutElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00013AF5 File Offset: 0x00011CF5
	public LTDescr setOvershoot(float overshoot)
	{
		this.overshoot = overshoot;
		return this;
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00013AFF File Offset: 0x00011CFF
	public LTDescr setPeriod(float period)
	{
		this.period = period;
		return this;
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00013B09 File Offset: 0x00011D09
	public LTDescr setScale(float scale)
	{
		this.scale = scale;
		return this;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00013B13 File Offset: 0x00011D13
	public LTDescr setEase(AnimationCurve easeCurve)
	{
		this._optional.animationCurve = easeCurve;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		this.easeType = LeanTweenType.animationCurve;
		return this;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00013B3C File Offset: 0x00011D3C
	public LTDescr setTo(Vector3 to)
	{
		if (this.hasInitiliazed)
		{
			this.to = to;
			this.diff = to - this.from;
		}
		else
		{
			this.to = to;
		}
		return this;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00013B69 File Offset: 0x00011D69
	public LTDescr setTo(Transform to)
	{
		this._optional.toTrans = to;
		return this;
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00013B78 File Offset: 0x00011D78
	public LTDescr setFrom(Vector3 from)
	{
		if (this.trans)
		{
			this.init();
		}
		this.from = from;
		this.diff = this.to - this.from;
		this.diffDiv2 = this.diff * 0.5f;
		return this;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00013BCD File Offset: 0x00011DCD
	public LTDescr setFrom(float from)
	{
		return this.setFrom(new Vector3(from, 0f, 0f));
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00013BE5 File Offset: 0x00011DE5
	public LTDescr setDiff(Vector3 diff)
	{
		this.diff = diff;
		return this;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00013BEF File Offset: 0x00011DEF
	public LTDescr setHasInitialized(bool has)
	{
		this.hasInitiliazed = has;
		return this;
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00013BF9 File Offset: 0x00011DF9
	public LTDescr setId(uint id, uint global_counter)
	{
		this._id = id;
		this.counter = global_counter;
		return this;
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00013C0A File Offset: 0x00011E0A
	public LTDescr setPassed(float passed)
	{
		this.passed = passed;
		return this;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00013C14 File Offset: 0x00011E14
	public LTDescr setTime(float time)
	{
		float num = this.passed / this.time;
		this.passed = time * num;
		this.time = time;
		return this;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00013C40 File Offset: 0x00011E40
	public LTDescr setSpeed(float speed)
	{
		this.speed = speed;
		if (this.hasInitiliazed)
		{
			this.initSpeed();
		}
		return this;
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00013C58 File Offset: 0x00011E58
	public LTDescr setRepeat(int repeat)
	{
		this.loopCount = repeat;
		if ((repeat > 1 && this.loopType == LeanTweenType.once) || (repeat < 0 && this.loopType == LeanTweenType.once))
		{
			this.loopType = LeanTweenType.clamp;
		}
		if (this.type == TweenAction.CALLBACK || this.type == TweenAction.CALLBACK_COLOR)
		{
			this.setOnCompleteOnRepeat(true);
		}
		return this;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00013CAD File Offset: 0x00011EAD
	public LTDescr setLoopType(LeanTweenType loopType)
	{
		this.loopType = loopType;
		return this;
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00013CB7 File Offset: 0x00011EB7
	public LTDescr setUseEstimatedTime(bool useEstimatedTime)
	{
		this.useEstimatedTime = useEstimatedTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00013CB7 File Offset: 0x00011EB7
	public LTDescr setIgnoreTimeScale(bool useUnScaledTime)
	{
		this.useEstimatedTime = useUnScaledTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00013CC8 File Offset: 0x00011EC8
	public LTDescr setUseFrames(bool useFrames)
	{
		this.useFrames = useFrames;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00013CD9 File Offset: 0x00011ED9
	public LTDescr setUseManualTime(bool useManualTime)
	{
		this.useManualTime = useManualTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00013CEA File Offset: 0x00011EEA
	public LTDescr setLoopCount(int loopCount)
	{
		this.loopType = LeanTweenType.clamp;
		this.loopCount = loopCount;
		return this;
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00013CFC File Offset: 0x00011EFC
	public LTDescr setLoopOnce()
	{
		this.loopType = LeanTweenType.once;
		return this;
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00013D07 File Offset: 0x00011F07
	public LTDescr setLoopClamp()
	{
		this.loopType = LeanTweenType.clamp;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00013D21 File Offset: 0x00011F21
	public LTDescr setLoopClamp(int loops)
	{
		this.loopCount = loops;
		return this;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00013D2B File Offset: 0x00011F2B
	public LTDescr setLoopPingPong()
	{
		this.loopType = LeanTweenType.pingPong;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00013D45 File Offset: 0x00011F45
	public LTDescr setLoopPingPong(int loops)
	{
		this.loopType = LeanTweenType.pingPong;
		this.loopCount = ((loops == -1) ? loops : (loops * 2));
		return this;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00013D60 File Offset: 0x00011F60
	public LTDescr setOnComplete(Action onComplete)
	{
		this._optional.onComplete = onComplete;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00013D76 File Offset: 0x00011F76
	public LTDescr setOnComplete(Action<object> onComplete)
	{
		this._optional.onCompleteObject = onComplete;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00013D8C File Offset: 0x00011F8C
	public LTDescr setOnComplete(Action<object> onComplete, object onCompleteParam)
	{
		this._optional.onCompleteObject = onComplete;
		this.hasExtraOnCompletes = true;
		if (onCompleteParam != null)
		{
			this._optional.onCompleteParam = onCompleteParam;
		}
		return this;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00013DB1 File Offset: 0x00011FB1
	public LTDescr setOnCompleteParam(object onCompleteParam)
	{
		this._optional.onCompleteParam = onCompleteParam;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00013DC7 File Offset: 0x00011FC7
	public LTDescr setOnUpdate(Action<float> onUpdate)
	{
		this._optional.onUpdateFloat = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00013DDD File Offset: 0x00011FDD
	public LTDescr setOnUpdateRatio(Action<float, float> onUpdate)
	{
		this._optional.onUpdateFloatRatio = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00013DF3 File Offset: 0x00011FF3
	public LTDescr setOnUpdateObject(Action<float, object> onUpdate)
	{
		this._optional.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00013E09 File Offset: 0x00012009
	public LTDescr setOnUpdateVector2(Action<Vector2> onUpdate)
	{
		this._optional.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00013E1F File Offset: 0x0001201F
	public LTDescr setOnUpdateVector3(Action<Vector3> onUpdate)
	{
		this._optional.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00013E35 File Offset: 0x00012035
	public LTDescr setOnUpdateColor(Action<Color> onUpdate)
	{
		this._optional.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00013E4B File Offset: 0x0001204B
	public LTDescr setOnUpdateColor(Action<Color, object> onUpdate)
	{
		this._optional.onUpdateColorObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00013E35 File Offset: 0x00012035
	public LTDescr setOnUpdate(Action<Color> onUpdate)
	{
		this._optional.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00013E4B File Offset: 0x0001204B
	public LTDescr setOnUpdate(Action<Color, object> onUpdate)
	{
		this._optional.onUpdateColorObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00013E61 File Offset: 0x00012061
	public LTDescr setOnUpdate(Action<float, object> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00013E86 File Offset: 0x00012086
	public LTDescr setOnUpdate(Action<Vector3, object> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector3Object = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00013EAB File Offset: 0x000120AB
	public LTDescr setOnUpdate(Action<Vector2> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00013ED0 File Offset: 0x000120D0
	public LTDescr setOnUpdate(Action<Vector3> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00013EF5 File Offset: 0x000120F5
	public LTDescr setOnUpdateParam(object onUpdateParam)
	{
		this._optional.onUpdateParam = onUpdateParam;
		return this;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00013F04 File Offset: 0x00012104
	public LTDescr setOrientToPath(bool doesOrient)
	{
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			if (this._optional.path == null)
			{
				this._optional.path = new LTBezierPath();
			}
			this._optional.path.orientToPath = doesOrient;
		}
		else
		{
			this._optional.spline.orientToPath = doesOrient;
		}
		return this;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00013F68 File Offset: 0x00012168
	public LTDescr setOrientToPath2d(bool doesOrient2d)
	{
		this.setOrientToPath(doesOrient2d);
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			this._optional.path.orientToPath2d = doesOrient2d;
		}
		else
		{
			this._optional.spline.orientToPath2d = doesOrient2d;
		}
		return this;
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00013FB4 File Offset: 0x000121B4
	public LTDescr setRect(LTRect rect)
	{
		this._optional.ltRect = rect;
		return this;
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00013FC3 File Offset: 0x000121C3
	public LTDescr setRect(Rect rect)
	{
		this._optional.ltRect = new LTRect(rect);
		return this;
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00013FD7 File Offset: 0x000121D7
	public LTDescr setPath(LTBezierPath path)
	{
		this._optional.path = path;
		return this;
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00013FE6 File Offset: 0x000121E6
	public LTDescr setPoint(Vector3 point)
	{
		this._optional.point = point;
		return this;
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00013FF5 File Offset: 0x000121F5
	public LTDescr setDestroyOnComplete(bool doesDestroy)
	{
		this.destroyOnComplete = doesDestroy;
		return this;
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00013FFF File Offset: 0x000121FF
	public LTDescr setAudio(object audio)
	{
		this._optional.onCompleteParam = audio;
		return this;
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0001400E File Offset: 0x0001220E
	public LTDescr setOnCompleteOnRepeat(bool isOn)
	{
		this.onCompleteOnRepeat = isOn;
		return this;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00014018 File Offset: 0x00012218
	public LTDescr setOnCompleteOnStart(bool isOn)
	{
		this.onCompleteOnStart = isOn;
		return this;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00014022 File Offset: 0x00012222
	public LTDescr setRect(RectTransform rect)
	{
		this.rectTransform = rect;
		return this;
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001402C File Offset: 0x0001222C
	public LTDescr setSprites(Sprite[] sprites)
	{
		this.sprites = sprites;
		return this;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00014036 File Offset: 0x00012236
	public LTDescr setFrameRate(float frameRate)
	{
		this.time = (float)this.sprites.Length / frameRate;
		return this;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001404A File Offset: 0x0001224A
	public LTDescr setOnStart(Action onStart)
	{
		this._optional.onStart = onStart;
		return this;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001405C File Offset: 0x0001225C
	public LTDescr setDirection(float direction)
	{
		if (this.direction != -1f && this.direction != 1f)
		{
			Debug.LogWarning("You have passed an incorrect direction of '" + direction.ToString() + "', direction must be -1f or 1f");
			return this;
		}
		if (this.direction != direction)
		{
			if (this.hasInitiliazed)
			{
				this.direction = direction;
			}
			else if (this._optional.path != null)
			{
				this._optional.path = new LTBezierPath(LTUtility.reverse(this._optional.path.pts));
			}
			else if (this._optional.spline != null)
			{
				this._optional.spline = new LTSpline(LTUtility.reverse(this._optional.spline.pts));
			}
		}
		return this;
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00014121 File Offset: 0x00012321
	public LTDescr setRecursive(bool useRecursion)
	{
		this.useRecursion = useRecursion;
		return this;
	}

	// Token: 0x040001C6 RID: 454
	public bool toggle;

	// Token: 0x040001C7 RID: 455
	public bool useEstimatedTime;

	// Token: 0x040001C8 RID: 456
	public bool useFrames;

	// Token: 0x040001C9 RID: 457
	public bool useManualTime;

	// Token: 0x040001CA RID: 458
	public bool usesNormalDt;

	// Token: 0x040001CB RID: 459
	public bool hasInitiliazed;

	// Token: 0x040001CC RID: 460
	public bool hasExtraOnCompletes;

	// Token: 0x040001CD RID: 461
	public bool hasPhysics;

	// Token: 0x040001CE RID: 462
	public bool onCompleteOnRepeat;

	// Token: 0x040001CF RID: 463
	public bool onCompleteOnStart;

	// Token: 0x040001D0 RID: 464
	public bool useRecursion;

	// Token: 0x040001D1 RID: 465
	public float ratioPassed;

	// Token: 0x040001D2 RID: 466
	public float passed;

	// Token: 0x040001D3 RID: 467
	public float delay;

	// Token: 0x040001D4 RID: 468
	public float time;

	// Token: 0x040001D5 RID: 469
	public float speed;

	// Token: 0x040001D6 RID: 470
	public float lastVal;

	// Token: 0x040001D7 RID: 471
	private uint _id;

	// Token: 0x040001D8 RID: 472
	public int loopCount;

	// Token: 0x040001D9 RID: 473
	public uint counter = uint.MaxValue;

	// Token: 0x040001DA RID: 474
	public float direction;

	// Token: 0x040001DB RID: 475
	public float directionLast;

	// Token: 0x040001DC RID: 476
	public float overshoot;

	// Token: 0x040001DD RID: 477
	public float period;

	// Token: 0x040001DE RID: 478
	public float scale;

	// Token: 0x040001DF RID: 479
	public bool destroyOnComplete;

	// Token: 0x040001E0 RID: 480
	public Transform trans;

	// Token: 0x040001E1 RID: 481
	internal Vector3 fromInternal;

	// Token: 0x040001E2 RID: 482
	internal Vector3 toInternal;

	// Token: 0x040001E3 RID: 483
	internal Vector3 diff;

	// Token: 0x040001E4 RID: 484
	internal Vector3 diffDiv2;

	// Token: 0x040001E5 RID: 485
	public TweenAction type;

	// Token: 0x040001E6 RID: 486
	private LeanTweenType easeType;

	// Token: 0x040001E7 RID: 487
	public LeanTweenType loopType;

	// Token: 0x040001E8 RID: 488
	public bool hasUpdateCallback;

	// Token: 0x040001E9 RID: 489
	public LTDescr.EaseTypeDelegate easeMethod;

	// Token: 0x040001EC RID: 492
	public SpriteRenderer spriteRen;

	// Token: 0x040001ED RID: 493
	public RectTransform rectTransform;

	// Token: 0x040001EE RID: 494
	public Text uiText;

	// Token: 0x040001EF RID: 495
	public Image uiImage;

	// Token: 0x040001F0 RID: 496
	public RawImage rawImage;

	// Token: 0x040001F1 RID: 497
	public Sprite[] sprites;

	// Token: 0x040001F2 RID: 498
	public LTDescrOptional _optional = new LTDescrOptional();

	// Token: 0x040001F3 RID: 499
	public static float val;

	// Token: 0x040001F4 RID: 500
	public static float dt;

	// Token: 0x040001F5 RID: 501
	public static Vector3 newVect;

	// Token: 0x02000047 RID: 71
	// (Invoke) Token: 0x060003E6 RID: 998
	public delegate Vector3 EaseTypeDelegate();

	// Token: 0x02000048 RID: 72
	// (Invoke) Token: 0x060003EA RID: 1002
	public delegate void ActionMethodDelegate();
}
