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
		
	
		// Update is called once per frame
		void Update () {
#if UNITY_STANDALONE_WIN
			if (Input.GetKey(KeyCode.J))
			{
				Rush();
			}
#endif
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
			if (_isRush && other.CompareTag("PlayerModel"))
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
