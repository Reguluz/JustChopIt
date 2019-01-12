using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
    public class SorcererSkillController:GamePlayerController
    {
        private void OnEnable()
		{
			//初始化基础参数（转向速度等级、移动速度等级、按技能数量新建控制器
			StaticData.RotateSpeed = 1;
			StaticData.MoveSpeed = 1;
			Cooldown = new CoolDownImageController[ActiveSkillInfo.Length];
			FxController = gameObject.GetComponent<SorcererFxController>();
		}

		[PunRPC]
		public override void Rebuild()
		{
			StaticData.RotateSpeed  = 1;
			StaticData.MoveSpeed = 1;	
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
					Chant();
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

	    private void Chant()
	    {
		    Debug.Log("Sorcerer is Chanting");
		    //PhotonView.RPC("ShootFx",RpcTarget.All);
		    if (PhotonView.IsMine)
		    {
			    //GameObject shriken = PhotonNetwork.PrefabPool.Instantiate("Player/Derivative/" + ShrikenPrefab.name, transform.position,transform.rotation);
			    Invoke(nameof(MatrixRelease),2f);
		    }
		    FxController.PlayFx("Chant");
	    }

	    private void MatrixRelease()
	    {
		    FxController.PlayFx("MatrixRelease");
		    Collider[] target = Physics.OverlapSphere(transform.position, 5, 1 << LayerMask.NameToLayer("Player"));

		    foreach (Collider player in target)
		    {
			    if (player.GetComponent<PhotonView>().ViewID != GetComponent<PhotonView>().ViewID)
			    {
				    if (player.GetComponent<PlayerProperties>().StateType != PlayerStateType.Dead &&
				        player.GetComponent<PlayerProperties>().StateType != PlayerStateType.Relieve)
				    {
					    Debug.Log(gameObject.GetComponent<PhotonView>().ViewID+"法阵攻击到"+player.GetComponent<PhotonView>().ViewID);
					    PhotonView pv = player.gameObject.GetPhotonView();
					    pv.RPC("Hurt", RpcTarget.All, DamageType.Normal,PhotonView.ViewID);
				    }
			
			    }
		    }
	    }
		
    }
}