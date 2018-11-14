using System;
using System.Collections;
using System.Collections.Generic;
using GamePlayer;
using Photon.Pun;
using UnityEngine;

public class Shriken : MonoBehaviour
{

	//public GameObject Owner;

	public int CreatorId;
	//private CircleSkillController _owner;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(transform.forward * Time.deltaTime*10,Space.World);
		Invoke("DestroySelf",2f);
	}

	private void OnTriggerEnter(Collider other)
	{	
		Debug.Log("命中");
		if (other.CompareTag("PlayerModel"))
		{
			Debug.Log("命中玩家");
			GameObject enemyscript = other.transform.parent.gameObject;
			if (enemyscript.GetComponent<PhotonView>().ViewID != CreatorId)
			{
				if (enemyscript.GetComponent<PlayerProperties>().StateType != PlayerStateType.Dead &&
				    enemyscript.GetComponent<PlayerProperties>().StateType != PlayerStateType.Relieve)
				{
					Debug.Log(gameObject.GetComponent<PhotonView>().ViewID+"的飞镖命中"+enemyscript.GetComponent<PhotonView>().ViewID);
					PhotonView pv = enemyscript.GetComponent<PhotonView>();
					pv.RPC("Hurt", RpcTarget.All, DamageType.Normal,CreatorId);
				}
			}
		}if(other.CompareTag("Terrain")){
			Debug.Log("命中地形");
			DestroySelf();
		}
	}

	void DestroySelf()
	{
		//PhotonNetwork.Destroy(GetComponent<PhotonView>());
		Destroy(this.gameObject);
	}


	[PunRPC]
	public void SetOwner(int serial)
	{
		Debug.Log("设置来源为"+serial);
		CreatorId = serial;
	}

	[PunRPC]
	public void SetDirection(Vector3 direction)
	{
		transform.forward = direction;
	}
}
