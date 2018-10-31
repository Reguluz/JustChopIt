using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
	
	public class CircleSkillController : GamePlayerController
	{
		//技能所需道具
		public GameObject ShrikenPrefab;
		// Use this for initialization
		
		
		private void Awake()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			RotateLevel = 1;
			SpeedLevel = 1;
			Cooldown = new CoolDownImageController[2];
		}

		void Start ()
		{
			//获取自身组件
			PhotonView = GetComponent<PhotonView>();
			Properties     =gameObject.GetComponent<PlayerProperties>();
			MoveController = gameObject.GetComponent<MoveController>();
			
			//获取UI总控组件
			UiController = GameObject.Find("GameCanvas").GetComponent<UIController>();
			
			//控制器绑定
			if (PhotonView.IsMine)
			{
				//UI绑定数据
				UiController.PlayerProperties = Properties;
				//技能设置
				SetSkillButton();
				//移动控制设置
				MoveController.RotateLevel = RotateLevel;
				MoveController.SpeedLevel = SpeedLevel;
				//向属性控制注册
				PropControllerRegister();
			}
			
			
		}

		private void Update()
		{
#if UNITY_STANDALONE_WIN
			if (Input.GetKey(KeyCode.J))
			{
				Shoot();
			}else if (Input.GetKey(KeyCode.K)){
				Dodge();
			}
#endif
		}

		public override void PropControllerRegister()
		{
			Properties.Controller = this;
		}
		
		//技能释放选择（操作来源于UI）
		public override void SkillRelease(int skillnum,Vector3 direction)
		{
			switch (skillnum)
			{
				case 0:Shoot(direction);
					break;
				case 1:Dodge();
					break;
				default:break;
			}
		}

		//技能初始化
		public override void SetSkillButton()	
		{
			for (int i = 0; i < ActiveSkillInfo.Length; i++)
			{
				//设置技能序号
				int serial = ActiveSkillInfo[i].Pos;
				//获取技能按钮
				Cooldown[serial] = UiController.Skill[serial].GetComponent<CoolDownImageController>();
				//控制器注册到技能按钮
				Cooldown[serial].RegisterOwner(this);
				//设置技能参数
				Cooldown[serial].SetSkill(ActiveSkillInfo[i]);
				//设置技能可用
				Cooldown[serial].SkillActived = true;
			
			}
			//隐藏不使用的按钮
			UiController.RejectorBlank();		
		}
		
		//被动伤害判定过滤
		[PunRPC]
		public override bool DamageFilter()	
		{
			return true;
		}
		public override void RefreshShow()	
		{
			UiController.Refresh();
		}
		
		
		


		private void Shoot(Vector3 direction)	
		{
			Debug.Log("Shoot direction"+direction);
			GameObject shriken = PhotonNetwork.Instantiate(ShrikenPrefab.name, transform.position, transform.rotation, 0);		
			PhotonView pv = shriken.GetComponent<PhotonView>();
			pv.RPC("SetOwner", RpcTarget.All,gameObject.GetComponent<PhotonView>().ViewID);		
			pv.RPC("SetDirection",RpcTarget.All,direction);
		}

		private void Dodge()
		{
			Debug.Log("Dodge");
			Properties.StateType = PlayerStateType.Vanity;
			//PhotonView.RPC("DodgeFx",RpcTarget.All,true);
			Invoke(nameof(EndDodge),1f);
		}

		private void EndDodge()
		{
			Properties.StateType = PlayerStateType.Alive;
			//PhotonView.RPC("DodgeFx",RpcTarget.All,false);
		}

	}


}
