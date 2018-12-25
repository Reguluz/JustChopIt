using System;
using System.Collections;
using System.Collections.Generic;
using GamePlayer.Characters;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GamePlayer
{
	public class PlayerProperties : MonoBehaviour,IPunObservable,IComparable<PlayerProperties>
	{

		public PlayerStateType StateType;
		[HideInInspector]
		public GamePlayerController Controller;
		public Player User;
		private GameBoard _board;
		
		
		[HideInInspector]
		public CharacterType CharacterType;
		[Header("Data")]
		public int GroupSerial;
		public int Score;
		public int Deathtime;
		
		//private MeshRenderer _meshRenderer;
		public MapController Map;

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
		}

		void Start ()
		{
			FxInit();
		}
	
		// Update is called once per frame
		void Update () {
		
		}
		[PunRPC]
		public void ComponentInit()
		{
			//_meshRenderer = GetComponent<MeshRenderer>();
			_meshModel = transform.GetChild(0).gameObject;
			_uiController = GameObject.Find("GameCanvas").GetComponent<UIController>();
			_photonView = GetComponent<PhotonView>();
			Controller = GetComponent<GamePlayerController>();
			User = _photonView.Owner;
			_board = GameObject.Find("ScoreBoard").GetComponent<GameBoard>();
			_board.AddEntry(this.gameObject);
			Dead();
		}

		

		private void FxInit()
		{
			//AvatarFx.enabled = false;
		}
		
		
		[PunRPC]
		public void Hurt(DamageType damageType,int killerViewId)
		{
			lock (_locker)
			{
				Debug.Log("受伤");
				
				if (StateType == PlayerStateType.Alive)
				{
					Debug.Log(killerViewId+"普通击杀");
					if (Controller.DamageFilter())
					{
						//_photonView.RPC("Dead",RpcTarget.All);
						Deathtime++;
						Dead();
						PhotonView pv = PhotonView.Find(killerViewId);
						if (!pv.IsSceneView)
						{
							pv.RPC("GetNewScore", RpcTarget.Others,1);
						}
					};
				}else if (StateType == PlayerStateType.Strong)
				{
					if (damageType == DamageType.Hard)
					{
						Debug.Log("穿刺击杀");
						if (Controller.DamageFilter())
						{
							Deathtime++;
							//_photonView.RPC("Dead",RpcTarget.All);
							Dead();
							PhotonView pv = PhotonView.Find(killerViewId);
							if (!pv.IsSceneView)
							{
								pv.RPC("GetNewScore", RpcTarget.Others,1);
							}
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

		//[PunRPC]
		public void Dead()
		{
			Debug.Log("玩家"+_photonView.Owner+"已经死亡");
			StartCoroutine(RelievePass());
		}

		IEnumerator RelievePass()
		{
			
			StateType = PlayerStateType.Dead;
			
			_meshModel.SetActive(true);
			//FXrenderer.enabled = false;
			Deadparticles.Play();
			_meshModel.SetActive(false);
			_photonView.RPC("RemoveAllBuff",RpcTarget.All);
			
			//屏幕特效
			if (_photonView.IsMine)
			{
				_uiController.DisableSkill();
			}
			_board.DataRefresh(this);
			yield return new WaitForSeconds(1f);
			
			

			StateType = PlayerStateType.Relieve;
			if (_photonView.IsMine)
			{
				transform.position = Map.GetRelievePoint();
			}
			/*if (gameObject.GetComponent<PhotonView>().IsMine)
			{
				
				transform.position = new Vector3(Random.Range(_minPosition.x,_maxPosition.x),Random.Range(_minPosition.y,_maxPosition.y));
			}*/
			//屏幕特效
			_board.DataRefresh(this);
			yield return new WaitForSeconds(1f);
			
			
			
			
			_meshModel.SetActive(true);
			
			//FXrenderer.enabled = true;
			//AvatarFx.enabled = true;
			_board.DataRefresh(this);
			yield return new WaitForSeconds(1f);
			
			
			
			
			if (_photonView.IsMine)
			{
				_uiController.AbleSkill();
			}
			//AvatarFx.enabled = false;
			Controller.Rebuild();
			//_photonView.RPC("Rebuild",RpcTarget.All);
			StateType = PlayerStateType.Alive;
			_board.DataRefresh(this);
			
		}
	
		private void Relieve()
		{
			
		}
		
		private void Avatar()
		{
			
		}
		
		private void NewLife()
		{
			
		}

		[PunRPC]
		public void GetNewScore(int t)
		{
			Score+=t;
			if (gameObject.GetComponent<PhotonView>().IsMine)
			{
				Controller.RefreshShow();
			}
			_board.DataRefresh(this);
		}

		//忘了干啥的
		public void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			Debug.Log("创建者passId");
			info.photonView.ViewID = gameObject.GetComponent<PhotonView>().ViewID;
		}

		//同步积分/死亡数量/死亡状态
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(this.Score);
				stream.SendNext(this.Deathtime);
				stream.SendNext(this.StateType);
			}else if(stream.IsReading){
				this.Score = (int) stream.ReceiveNext();
				this.Deathtime = (int) stream.ReceiveNext();
				this.StateType = (PlayerStateType) stream.ReceiveNext();
			}
		}

		//用于记分板排名
		public int CompareTo(PlayerProperties other)
		{
			if (ReferenceEquals(this, other)) return 1;
			if (ReferenceEquals(null, other)) return 0;
			return other.Score.CompareTo(Score);
		}
	}


}
