using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

namespace GamePlayer
{
	public class DiamondSkillController : GamePlayerController
	{
		//私有技能参数
		private bool _isRush = false;
		
		
		
		private void Awake()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			RotateLevel = 2;
			SpeedLevel = 1;
			Cooldown = new CoolDownImageController[2];
		}

		// Use this for initialization
		void Start () {
		
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
	
		// Update is called once per frame
		void Update () {
#if UNITY_STANDALONE_WIN
			if (Input.GetKey(KeyCode.J))
			{
				Rush();
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
				case 0:Rush();
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
			if (_isRush)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public override void RefreshShow()
		{
			UiController.Refresh();
		}
		
		
		
		
		

		private void Rush()
		{
			MoveController.SpeedLevel = 2;
			_isRush = true;
			//PhotonView.RPC("RushFx",RpcTarget.All,true);
			Invoke("EndRush",1f);
		}

		private void EndRush()
		{
			MoveController.SpeedLevel = 1;
			//PhotonView.RPC("RushFx",RpcTarget.All,false);
			_isRush = false;
		}
		
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log("是否在冲刺:" + _isRush);
			if (_isRush && other.CompareTag("Player"))
			{
				GameObject enemyscript = other.transform.parent.gameObject;
				Debug.Log(GetComponent<PhotonView>().ViewID + "冲刺碰到了" + enemyscript.GetComponent<PhotonView>().ViewID);
				if (enemyscript.GetComponent<PhotonView>().ViewID != GetComponent<PhotonView>().ViewID)
				{
					if (enemyscript.GetComponent<PlayerProperties>().StateType != PlayerStateType.Dead &&
					    enemyscript.GetComponent<PlayerProperties>().StateType != PlayerStateType.Relieve)
					{
						Debug.Log(gameObject.GetComponent<PhotonView>().ViewID+"冲刺攻击到"+enemyscript.GetComponent<PhotonView>().ViewID);
						PhotonView pv = enemyscript.GetComponent<PhotonView>();
						pv.RPC("Hurt", RpcTarget.All, DamageType.Normal,PhotonView.ViewID);
					}
			
				}
			}
		}
		
		
	}
}
