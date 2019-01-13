using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Derivative
{
    public class Trap:Derivative
    {
        public ParticleSystem TrapParticle;
        public  float Delaytime = 1.5f;
	    [Tooltip("半径和粒子StartSize的比例为2.5:1")]
        public float EffectRadius = 10;
	    private MeshRenderer _mesh;
		private AudioSource _audioSource;
			
	    private void Awake()
	    {
		    _mesh = GetComponent<MeshRenderer>();
		    var trapParticleMain = TrapParticle.main;
		    trapParticleMain.startSize = EffectRadius / 2.5f;
		    _audioSource = GetComponent<AudioSource>();
	    }

	    private void OnEnable()
        {
            //延迟销毁
	        TrapParticle.Stop();
	        _mesh.enabled = true;
            Invoke(nameof(Boom),Delaytime);
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
				_audioSource.Play();
				//命中检测
				Collider[] players =
					Physics.OverlapSphere(transform.position, EffectRadius, 1 << LayerMask.NameToLayer("Player"));

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
	        if (photonView.IsMine)
	        {
		        PhotonNetwork.Destroy(this.gameObject);
	        }
            
        }

	    private void OnDisable()
	    {
		    TrapParticle.Stop();
		    CancelInvoke(nameof(Boom));
		    StopCoroutine(BoomEffect());
	    }

	   
        
    }
}