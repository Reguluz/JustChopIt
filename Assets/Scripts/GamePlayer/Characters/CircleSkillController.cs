using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	
	public class CircleSkillController : GamePlayerController
	{
		//技能所需道具
		//private CircleFxController _fxController;
		public GameObject ShrikenPrefab;
		// Use this for initialization
		
		
		private void OnEnable()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			StaticData.RotateSpeed = 1;
			StaticData.MoveSpeed = 1;
			Cooldown = new CoolDownImageController[ActiveSkillInfo.Length];
			FxController = gameObject.GetComponent<CircleFxController>();
		}

		[PunRPC]
		public override void Rebuild()
		{
			StaticData.RotateSpeed  = 1;
			StaticData.MoveSpeed = 1;	
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
		[PunRPC]
		public override void SkillRelease(int skillnum,Vector3 direction)
		{
			switch (skillnum)
			{
				case 0:
					BulletShoot();
					break;
				case 1:
					Shoot(direction);
					break;
				case 2:
					Dodge();
					break;
				default:break;
			}
		}

		
		
		//被动伤害判定过滤
		[PunRPC]
		public override bool DamageFilter()	
		{
			for (int i = 0; i < Buffs.Count; i++)
			{
				if (Buffs[i].Bufftype.Equals(Bufftype.Shield))
				{
					Debug.Log("FindShieldBuff");
					Buffs[i].RemoveBuff(this);
					Buffs.Remove(Buffs[i]);
					return false;
				}
			}
			return true;
		}

		public override void SetCharacterType()
		{
			Properties.CharacterType = CharacterType.Shooter;
		}

		private  void Dodge()
		{
			Debug.Log("Dodge");
			Properties.StateType = PlayerStateType.Vanity;
			FxController.PlayFx("Dodge");
			//PhotonView.RPC("DodgeFx",RpcTarget.All);
			Invoke(nameof(EndDodge),1f);
		}

		private  void EndDodge()
		{
			Properties.StateType = PlayerStateType.Alive;
			FxController.PlayFx("Rebuild");
			//PhotonView.RPC("FxRebuild",RpcTarget.All);
		}

		private void Shoot(Vector3 direction)	
		{
			Debug.Log("Shoot direction"+direction);
			//PhotonView.RPC("ShootFx",RpcTarget.All);
			if (PhotonView.IsMine)
			{
				//GameObject shriken = PhotonNetwork.PrefabPool.Instantiate("Player/Derivative/" + ShrikenPrefab.name, transform.position,transform.rotation);
				GameObject shriken = PhotonNetwork.Instantiate(ShrikenPrefab.name, transform.position, transform.rotation, 0);		
				PhotonView pv = shriken.GetComponent<PhotonView>();
				pv.RPC("SetOwner", RpcTarget.All,gameObject.GetComponent<PhotonView>().ViewID);
				if (direction == Vector3.zero)
				{
					pv.RPC("SetDirection",RpcTarget.All,transform.forward);
				}
				else
				{
					pv.RPC("SetDirection",RpcTarget.All,direction);
				}
			}
			FxController.PlayFx("SkillRelease");
		}
		
		

		
	}


}
