using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Derivative
{
    public class Trap:Derivative
    {
        public ParticleSystem TrapParticle;
        private readonly float _lifeTime = 2;
        private float _effectRadius = 10;
	    private MeshRenderer _mesh;

	    private void Awake()
	    {
		    _mesh = GetComponent<MeshRenderer>();
	    }

	    private void OnEnable()
        {
            //延迟销毁
	        TrapParticle.Stop();
	        _mesh.enabled = true;
            Invoke(nameof(Boom),_lifeTime);
        }

	    public void Boom()
	    {
		    _mesh.enabled = false;
		    StartCoroutine(BoomEffect());
	    }
	    
        IEnumerator  BoomEffect()
        {
	        //播放动画
	        TrapParticle.Play();
	        //等待出发时间（动画放0.2s）
	        yield return new WaitForSeconds(0.2f);
	        
            if (photonView.IsMine)
			{
				Debug.Log("陷阱生效");
				//命中检测
				Collider[] players =
					Physics.OverlapSphere(transform.position, _effectRadius, 1 << LayerMask.NameToLayer("Player"));

				foreach (Collider collider in players)
				{
					GameObject enemy = collider.transform.parent.gameObject;
					//排除自伤
					if (collider.transform.parent.GetComponent<PhotonView>().ViewID != CreatorId)
					{
						//检测状态（非死亡状态）
						if (enemy.GetComponent<PlayerProperties>().StateType != PlayerStateType.Dead &&
						    enemy.GetComponent<PlayerProperties>().StateType != PlayerStateType.Relieve)
						{
							Debug.Log(gameObject.GetComponent<PhotonView>().ViewID+"的陷阱命中"+enemy.GetComponent<PhotonView>().ViewID);
							PhotonView pv = enemy.GetComponent<PhotonView>();
							//给予伤害
							pv.RPC("Hurt", RpcTarget.All, DamageType.Normal,CreatorId);
						}
					}
				}
			}
	        yield return new WaitForSeconds(0.6f);
	        PhotonNetwork.Destroy(this.gameObject);
            
        }

	    private void OnDisable()
	    {
		    TrapParticle.Stop();
		    StopCoroutine(BoomEffect());
	    }

	    public override void DestroySelf()
	    {
		    PhotonNetwork.Destroy(this.gameObject);
	    }
        
    }
}