using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	
	public class CircleSkillController : GamePlayerController
	{
		//技能所需道具
		private CircleFxController _fxController;
		public GameObject ShrikenPrefab;
		// Use this for initialization
		
		
		private void Awake()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			RotateLevel = 1;
			SpeedLevel = 1;
			Cooldown = new CoolDownImageController[2];
			_fxController = gameObject.GetComponent<CircleFxController>();
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
		[PunRPC]
		public override void SkillRelease(int skillnum,Vector3 direction)
		{
			switch (skillnum)
			{
				case 0:
					Dodge();
					break;
				case 1:
					Shoot(direction);
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

		protected override void Dodge()
		{
			Debug.Log("Dodge");
			Properties.StateType = PlayerStateType.Vanity;
			_fxController.DodgeFx();
			//PhotonView.RPC("DodgeFx",RpcTarget.All);
			Invoke(nameof(EndDodge),1f);
		}

		protected override void EndDodge()
		{
			Properties.StateType = PlayerStateType.Alive;
			_fxController.FxRebuild();
			//PhotonView.RPC("FxRebuild",RpcTarget.All);
		}

		private void Shoot(Vector3 direction)	
		{
			Debug.Log("Shoot direction"+direction);
			_fxController.ShootFx();
			//PhotonView.RPC("ShootFx",RpcTarget.All);
			if (PhotonView.IsMine)
			{
				GameObject shriken = PhotonNetwork.Instantiate(ShrikenPrefab.name, transform.position, transform.rotation, 0);		
				PhotonView pv = shriken.GetComponent<PhotonView>();
				pv.RPC("SetOwner", RpcTarget.All,gameObject.GetComponent<PhotonView>().ViewID);		
				pv.RPC("SetDirection",RpcTarget.All,direction);
			}
		}
		
		

		
	}


}
