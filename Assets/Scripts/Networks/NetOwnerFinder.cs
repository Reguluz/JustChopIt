using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class NetOwnerFinder : MonoBehaviour,IPunInstantiateMagicCallback
{
    public List<PhotonView> _playerCharacterlist = new List<PhotonView>();

    public ParticleSystem DestoryParticle;
    public GameObject[] BanedParticle;
    public float DestoryDelay;

    private PhotonView _photonView;
    // Start is called before the first frame update
    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        if (_playerCharacterlist.Count<1)
        {
            Init();
        }
        else
        {
            foreach (GameObject system in BanedParticle)
            {
                system.SetActive(true);
            }
        }
    }

    public void Init()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("玩家数量"+players.Length);
        foreach (GameObject player in players)
        {
            _playerCharacterlist.Add(player.GetPhotonView());
        }

        

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        foreach (PhotonView character in _playerCharacterlist)
        {
            Debug.Log("道具所有者"+_photonView.Owner+"   检测到所有者"+character.Owner);
            if (character.Owner.Equals(_photonView.Owner))
            {
                Debug.Log("Find Owner");
                transform.parent = character.transform;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                break;
            }
        }
    }

    [PunRPC]
    public void DestoryEffect()
    {
        if (DestoryParticle != null)
        {
            foreach (GameObject system in BanedParticle)
            {
                system.SetActive(false);
            } 
            DestoryParticle.Play();
            DestoryParticle.gameObject.GetComponent<AudioSource>().Play();
        }
        Invoke(nameof(Destoryself),DestoryDelay);
        
    }

    private void Destoryself()
    {
        PhotonNetwork.Destroy(gameObject);
    }

}
