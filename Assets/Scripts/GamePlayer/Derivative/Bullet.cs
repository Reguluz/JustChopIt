using System;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Derivative
{
	public class Bullet : Derivative
	{
		//设置移动速度和生命时间
		private readonly float _moveSpeed = 100;
		private readonly float _lifeTime = 0.1f;

		private MeshFilter _meshFilter;
		private MeshCollider _collider;
		public Mesh[] Shadowmesh;

		private void Awake()
		{
			_meshFilter = GetComponent<MeshFilter>();
			_collider = GetComponent<MeshCollider>();
		}

		private void OnEnable()
		{
			//延迟销毁
			Invoke(nameof(DestroySelf),_lifeTime);
		}

		// Update is called once per frame
		void FixedUpdate () {
			//向前移动
			transform.Translate(transform.forward * Time.deltaTime*_moveSpeed,Space.World);
		}

		
		/***注意****/
		/*本技能由于具有穿透作用，所以使用Trigger*/
		private void OnTriggerEnter(Collider other)	
		{
			if (photonView.IsMine)
			{
				Debug.Log("命中");
				//命中类型检测
				if (other.CompareTag("PlayerModel"))
				{
					
					GameObject enemy = other.transform.parent.gameObject;
				
					//排除自伤
					if (enemy.GetComponent<PhotonView>().ViewID != CreatorId)
					{
						Debug.Log("命中其他玩家");
						//检测状态（非死亡状态）
						if (enemy.GetComponent<PlayerProperties>().StateType != PlayerStateType.Dead &&
						    enemy.GetComponent<PlayerProperties>().StateType != PlayerStateType.Relieve)
						{
							Debug.Log(gameObject.GetComponent<PhotonView>().ViewID+"的子弹命中"+enemy.GetComponent<PhotonView>().ViewID);
							PhotonView pv = enemy.GetComponent<PhotonView>();
							//给予伤害
							pv.RPC("Hurt", RpcTarget.All, DamageType.Normal,CreatorId);
						}
						DestroySelf();
					}
				}else if(other.CompareTag("Terrain")){
					Debug.Log("命中地形");
					CancelInvoke(nameof(DestroySelf));
					DestroySelf();
				}
				
			}
		}

		[PunRPC]
		public void SetMesh(CharacterType characterType)
		{
			_meshFilter.mesh = Shadowmesh[(int)characterType];
			_collider.sharedMesh = _meshFilter.mesh;
		}
		


	}
}
