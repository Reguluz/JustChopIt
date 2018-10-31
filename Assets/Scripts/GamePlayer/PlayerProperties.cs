using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
	public class PlayerProperties : MonoBehaviour,IPunInstantiateMagicCallback
	{

		public PlayerStateType StateType;

		[HideInInspector]
		public GamePlayerController Controller;

		
		public int KillTime;
		public int DeathTime;
		
		//private MeshRenderer _meshRenderer;
		public Sprite Map;
		private Vector3 _minPosition;
		private Vector3 _maxPosition;
		private readonly Object _locker = new Object();
		private UIController _uiController;
		private PhotonView _photonView;

		//fx
		public ParticleSystem Deadparticles;

		private GameObject _meshModel;
		//public _2dxFX_GoldFX AvatarFx;
		//public SpriteRenderer FXrenderer;
		
		
		// Use this for initialization
		private void Awake()
		{
			ComponentInit();
		}

		void Start ()
		{
			FunctionInit();
			FxInit();
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		private void ComponentInit()
		{
			//_meshRenderer = GetComponent<MeshRenderer>();
			_meshModel = transform.GetChild(0).gameObject;
			_uiController = GameObject.Find("GameCanvas").GetComponent<UIController>();
			_photonView = GetComponent<PhotonView>();
			Controller = GetComponent<GamePlayerController>();
		}

		private void FunctionInit()
		{
			Map = GameObject.Find("Map").GetComponent<SpriteRenderer>().sprite;
			if (Map.texture != null)                                            //当区域贴图不为空时获得贴图的最小坐标（x最小与y最小）和最大坐标（x最大与y最大）来获取生成区域
			{
				_minPosition = new Vector3((transform.position.x - (Map.texture.width >> 7) * gameObject.transform.localScale.x),
					(transform.position.y - (Map.texture.height >> 7) * gameObject.transform.localScale.y),
					SystemOption.PlayerZPosition);
				_maxPosition = new Vector3((transform.position.x + (Map.texture.width >> 7) * gameObject.transform.localScale.x),
					(transform.position.y + (Map.texture.height >> 7) * gameObject.transform.localScale.y),
					SystemOption.PlayerZPosition);
			}
		}

		private void FxInit()
		{
			//AvatarFx.enabled = false;
		}
		
		
		[PunRPC]
		public void Hurt(DamageType damageType,int killerViewID)
		{
			lock (_locker)
			{
				Debug.Log("受伤");
				
				if (StateType == PlayerStateType.Alive)
				{
					Debug.Log("普通击杀");
					PhotonView.Find(killerViewID)?.RPC("GetNewScore", RpcTarget.Others);
					if (Controller.DamageFilter())
					{
						Dead();
					};
				}else if (StateType == PlayerStateType.Strong)
				{
					if (damageType == DamageType.Hard)
					{
						Debug.Log("穿刺击杀");
						PhotonView.Find(killerViewID)?.RPC("GetNewScore", RpcTarget.Others);
						if (Controller.DamageFilter())
						{
							Dead();
						};
					}
					//闪避
				}
				else
				{
					//闪避
				}
			}
		}

		[PunRPC]
		public void Dead()
		{
			StateType = PlayerStateType.Dead;
			_meshModel.SetActive(true);
			//FXrenderer.enabled = false;
			Deadparticles.Play();
			_meshModel.SetActive(false);
			//屏幕特效
			Invoke("Relieve",1f);
			if (_photonView.IsMine)
			{
				_uiController.DisableSkill();
			}
		}

		private void Relieve()
		{
			StateType = PlayerStateType.Relieve;
			if (_photonView.IsMine)
			{
				transform.position = new Vector3(Random.Range(_minPosition.x,_maxPosition.x),1,Random.Range(_minPosition.y,_maxPosition.y));	
			}
			
			/*if (gameObject.GetComponent<PhotonView>().IsMine)
			{
				
				transform.position = new Vector3(Random.Range(_minPosition.x,_maxPosition.x),Random.Range(_minPosition.y,_maxPosition.y));
			}*/
			//屏幕特效
			
			Invoke("Avatar",1f);
		}
		
		private void Avatar()
		{
			_meshModel.SetActive(true);
			//FXrenderer.enabled = true;
			//AvatarFx.enabled = true;
			Invoke("NewLife",1f);
		}
		
		private void NewLife()
		{
			if (_photonView.IsMine)
			{
				_uiController.AbleSkill();
			}
			//AvatarFx.enabled = false;
			StateType = PlayerStateType.Alive;
		}

		[PunRPC]
		public void GetNewScore()
		{
			KillTime++;
			if (gameObject.GetComponent<PhotonView>().IsMine)
			{
				Controller.RefreshShow();
			}
		}

		

		


		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			Debug.Log("创建者passId");
			info.photonView.ViewID = gameObject.GetComponent<PhotonView>().ViewID;
		}
	}


}
