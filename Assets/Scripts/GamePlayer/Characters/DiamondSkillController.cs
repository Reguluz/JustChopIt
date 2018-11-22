using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	public class DiamondSkillController : GamePlayerController
	{
		//私有技能参数
		private bool _isRush = false;
		private DiamondFxController _fxController;
		
		
		private void Awake()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			StaticData.RotateSpeed = 2;
			StaticData.MoveSpeed = 1.25f;
			Cooldown = new CoolDownImageController[2];
			_fxController = gameObject.GetComponent<DiamondFxController>();
		}

		[PunRPC]
		public override void Rebuild()
		{
			StaticData.RotateSpeed = 2;
			StaticData.MoveSpeed = 1.25f;
			StaticData.Scale = 1;
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
		[PunRPC]
		public override void SkillRelease(int skillnum,Vector3 direction)
		{
			switch (skillnum)
			{
				case 0:Dodge();
					break;
				case 1:Rush();
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
		private void Rush()
		{
			if (PhotonView.IsMine)
			{
				SkillCo.MoveSpeed = 0.75f;
			}
			_isRush = true;
			_fxController.RushFx(true);
			//PhotonView.RPC("RushFx",RpcTarget.All,true);
			Invoke(nameof(EndRush),1f);
		}

		private void EndRush()
		{
			if (PhotonView.IsMine)
			{
				SkillCo.MoveSpeed = 0;
			}
			_fxController.RushFx(false);
			//PhotonView.RPC("RushFx",RpcTarget.All,false);
			_isRush = false;
		}
		
		private void OnCollisionEnter(Collision other)
		{
			Debug.Log("是否在冲刺:" + _isRush + "对方类型" + other.gameObject.tag);
			if (_isRush && other.gameObject.CompareTag("Player"))
			{
				GameObject enemyscript = other.gameObject;
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
