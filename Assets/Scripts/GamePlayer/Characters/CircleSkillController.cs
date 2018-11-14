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

		[PunRPC]
		public override void Rebuild()
		{
			RotateLevel = 1;
			SpeedLevel = 1;	
		}

		private void Update()
		{
#if UNITY_STANDALONE_WIN
			if (Input.GetKey(KeyCode.J))
			{
				Shoot(Vector3.zero);
			}else if (Input.GetKey(KeyCode.K)){
				Dodge();
			}
#endif
		}

		
		//技能释放选择（操作来源于UI）
		public override void SkillRelease(int skillnum,Vector3 direction)
		{
			switch (skillnum)
			{
				case 0:Dodge();
					break;
				case 1:Shoot(direction);
					break;
				default:break;
			}
		}

		
		
		//被动伤害判定过滤
		[PunRPC]
		public override bool DamageFilter()	
		{
			return true;
		}

		public override void SetCharacterType()
		{
			Properties.CharacterType = CharacterType.Circle;
		}

		private void Shoot(Vector3 direction)	
		{
			Debug.Log("Shoot direction"+direction);
			PhotonView.RPC("ShootFx",RpcTarget.All);
			GameObject shriken = PhotonNetwork.Instantiate(ShrikenPrefab.name, transform.position, transform.rotation, 0);		
			PhotonView pv = shriken.GetComponent<PhotonView>();
			pv.RPC("SetOwner", RpcTarget.All,gameObject.GetComponent<PhotonView>().ViewID);		
			pv.RPC("SetDirection",RpcTarget.All,direction);
		}

		
	}


}
