using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
    public class TrapperSkillController : GamePlayerController
    {
        //技能所需道具
		//private TrapperFxController _fxController;
		public GameObject TrapPrefab;
		// Use this for initialization
		
		
		private void OnEnable()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			StaticData.RotateSpeed = 1;
			StaticData.MoveSpeed = 0.8f;
			Cooldown = new CoolDownImageController[2];
			FxController = gameObject.GetComponent<TrapperFxController>();
		}

		[PunRPC]
		public override void Rebuild()
		{
			StaticData.RotateSpeed  = 1;
			StaticData.MoveSpeed = 0.8f;	
		}

		private void Update()
		{

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
			Properties.CharacterType = CharacterType.Trapper;
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
			FxController.PlayFx("SkillRelease");
		}
    }
}