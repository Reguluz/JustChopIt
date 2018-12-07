using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
    public class TrapperSkillController : GamePlayerController
    {
        //技能所需道具
		private TrapperFxController _fxController;
		public GameObject TrapPrefab;
		// Use this for initialization
		
		
		private void OnEnable()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			StaticData.RotateSpeed = 1;
			StaticData.MoveSpeed = 1;
			Cooldown = new CoolDownImageController[2];
			_fxController = gameObject.GetComponent<TrapperFxController>();
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
					Dodge();
					break;
				case 1:
					Trap();
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

		private void Trap()	
		{
			if (PhotonView.IsMine)
			{
				//GameObject shriken = PhotonNetwork.PrefabPool.Instantiate("Player/Derivative/" + ShrikenPrefab.name, transform.position,transform.rotation);
				GameObject trap = PhotonNetwork.Instantiate(TrapPrefab.name, transform.position, transform.rotation, 0);		
				PhotonView pv = trap.GetComponent<PhotonView>();
				pv.RPC("SetOwner", RpcTarget.All,gameObject.GetComponent<PhotonView>().ViewID);
			}
			_fxController.TrapFx();
		}
    }
}