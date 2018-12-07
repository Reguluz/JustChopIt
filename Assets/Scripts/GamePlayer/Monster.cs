using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
    public class Monster:MonoBehaviour,IPunInstantiateMagicCallback
    {
        public int Level;
        public int EffectRadius;

        private PhotonView _photonView;
        private MapController _mapController;

        private Rigidbody _rigidbody;
        public GameObject _target;
        public Vector3 _targetPos;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            
        }

        private void FixedUpdate()
        {
            Collider[] onEffect=Physics.OverlapSphere(transform.position, EffectRadius,1<<LayerMask.NameToLayer("Player"));
            foreach (Collider obj in onEffect)
            {
                
                    Vector3 deltaPos = transform.position - obj.gameObject.transform.position;
                    Vector3 force = deltaPos.normalized * 1 * 10;
                    obj.transform.parent.GetComponent<Rigidbody>().AddForce(force*5);
                
            }
        }

        private void Update()
        {
            if (_target != null)
            {
                if (_target.GetComponent<PlayerProperties>()?.StateType != PlayerStateType.Alive)
                {
                    _target = null;
                    System.Random random = new System.Random();
                    _targetPos = new Vector3(transform.position.x+random.Next(-10,10),
                        transform.position.y,
                        transform.position.z+random.Next(-10,10));
                }
            }
            
            _rigidbody.MovePosition(Vector3.Lerp(transform.position,_targetPos,Time.fixedDeltaTime));
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PhotonView>().RPC("Hurt",RpcTarget.All,DamageType.Hard,_photonView.ViewID);
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            _mapController = GameObject.Find("Map").GetComponent<MapController>();
            _mapController.SetEffectCenter(this.gameObject);
            _mapController.EffectSwitch(true);
            StartCoroutine(Move());
            Invoke(nameof(DestoryMonster),30f);
        }
        
        private void DestoryMonster()
        {
            _mapController.EffectSwitch(false);
            StopCoroutine(Move());
            if (PhotonNetwork.IsMasterClient)
            {
                
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

        IEnumerator Move()
        {
            while (gameObject.activeSelf)
            {
                Collider[] target = Physics.OverlapSphere(transform.position,EffectRadius*1.5f,1<<LayerMask.NameToLayer("Player"));
                System.Random random = new System.Random();
                if (target.Length > 0)
                {
                    Debug.Log("范围内有玩家");
                    int t = Random.Range(0, target.Length);
                    _target = target[t].transform.parent.gameObject;
                    _targetPos = _target.transform.position;
                }
                else
                {
                    _targetPos = new Vector3(transform.position.x+random.Next(-10,10),
                        transform.position.y,
                        transform.position.z+random.Next(-10,10));
                }
                yield return new WaitForSeconds(2f);
            }
        }
    }
}