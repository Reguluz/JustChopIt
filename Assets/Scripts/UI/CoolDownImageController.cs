﻿using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using GamePlayer.Characters;
using Photon.Pun;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownImageController : MonoBehaviour,SkillButton
{
	public GamePlayerController Owner;
	public PhotonView OwnerPhotonView;
	public Image DefaultImage;
	public Image EffectImage;

	public int Serial;
	public float MaxCoolDown;
	public float IntervalTime;

	private SkillType _type;

	public bool SkillActived = false;	//技能是否可释放

	public bool PlayerActived = true;	//是否可控

	public Button Click;
	public EasyTouchSkill Drag;
	
	// Use this for initialization
	private void Awake()
	{
		DefaultImage.color = new Color(0.75f,0.75f,0.75f);
	}

	private void Start()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		EffectImage.fillAmount = (MaxCoolDown-IntervalTime) / MaxCoolDown;
	}

	private void FixedUpdate()
	{
		if (IntervalTime > 0)
		{
			IntervalTime -= 0.02f;
		}
		else if(IntervalTime<0)
		{
			IntervalTime = 0;
			SkillActived = true;
			//Click.enabled = true;
			if (!_type.Equals(SkillType.AutoTarget))
			{
				/*Click.enabled = true;
			}
			else
			{*/
				Drag.enabled = true;
			}
		}
	}

	public void SetSkill(SkillBaseInfo baseInfo)
	{
		Serial = baseInfo.Pos;
		MaxCoolDown = baseInfo.CoolDown;
		DefaultImage.sprite = baseInfo.SkillImage;
		EffectImage.sprite = baseInfo.SkillImage;
		_type = baseInfo.SkillType;
		Click.enabled = false;
		if (_type.Equals(SkillType.AutoTarget))
		{
			//Drag.enabled = false;
			Owner.gameObject.GetComponentInChildren<LineRenderer>().enabled = false;
			Owner.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
		else
		{
			Debug.Log("是指向性技能");
			//Click.enabled = false;
			Drag.Init(Owner,baseInfo.SkillType.Equals(SkillType.LineTarget));
			Owner.gameObject.GetComponentInChildren<LineRenderer>().enabled = false;
			Owner.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
	}

	public void RegisterOwner(GamePlayerController controller)
	{
		Owner = controller;
		OwnerPhotonView = Owner.gameObject.GetComponent<PhotonView>();
	}

	public void SkillButtonPressed()
	{
		Debug.Log("技能"+Serial+"点击了");
		if (SkillActived && PlayerActived)
		{
			Debug.Log("技能释放了");
			OwnerPhotonView.RPC("SkillRelease",RpcTarget.All,Serial,Vector3.zero);
			//Owner.SkillRelease(Serial,Vector3.zero);
			IntervalTime = MaxCoolDown;
			SkillActived = false;
			//Click.enabled = false;
		}
	}

	public void SkillButtonReleased()
	{
		Debug.Log("技能"+Serial+"点击了");
		if (SkillActived && PlayerActived)
		{
			Debug.Log("技能释放了"+Drag.EndVector);
			OwnerPhotonView.RPC("SkillRelease",RpcTarget.All,Serial,Drag.EndVector);
			//Owner.SkillRelease(Serial,Drag.EndVector);
			IntervalTime = MaxCoolDown;
			SkillActived = false;
			Drag.enabled = false;
		}
	}

	public void BanClick()
	{
		Click.enabled = false;
	}

}
