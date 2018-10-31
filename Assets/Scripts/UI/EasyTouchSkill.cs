using GamePlayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class EasyTouchSkill : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public UnityEvent Hold;
        public UnityEvent Release;
        public GamePlayerController Owner;
        public GameObject GuideUI;

        public bool isLine;
        //图标移动最大半径
        public float maxRadius;
        //初始化背景图标位置
        private Vector2 moveBackPos;
    
        //hor,ver的属性访问器
        private float horizontal=0;
        private float vertical=0;
        private static float ratio = 0.1f;
        public Vector3 EndVector;

        private void Start()
        {
            
        }

        public void Init(GamePlayerController owner,bool isLine)
        {
            Owner = owner;
            GuideUI = Owner.transform.Find("GuideUI").gameObject;
            this.isLine = isLine;
            Owner.gameObject.GetComponentInChildren<LineRenderer>().enabled = false;
            Owner.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        
        private void Update () {
            horizontal = transform.localPosition.x*ratio;
            vertical = transform.localPosition.y*ratio;
            moveBackPos = transform.parent.transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            EndVector = Vector3.zero;
            Vector2 oppsitionVec = eventData.position - moveBackPos;
            Debug.DrawLine(Vector3.zero, eventData.position,Color.green);
            Debug.DrawLine(Owner.transform.position, Owner.transform.position + new Vector3(horizontal,0.25f,vertical),Color.white);
            //获取向量的长度    
            float distance = Vector3.Magnitude(oppsitionVec);
            //最小值与最大值之间取半径
            float radius = Mathf.Clamp(distance, 0, maxRadius);
            //限制半径长度
            transform.position = moveBackPos + oppsitionVec.normalized * radius;

            if (isLine)
            {
                GuideUI.GetComponentInChildren<LineRenderer>().enabled = true;
                GuideUI.GetComponentInChildren<LineRenderer>().SetPosition(0,Owner.transform.position+new Vector3(0,-0.75f,0));
                GuideUI.GetComponentInChildren<LineRenderer>().SetPosition(1,Owner.transform.position+new Vector3(horizontal,0.25f,vertical));
            }
            else
            {
                GuideUI.GetComponentInChildren<SpriteRenderer>().enabled = true;
                GuideUI.transform.position = Owner.transform.position + new Vector3(horizontal, 0.25f-1, vertical);
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
            EndVector = new Vector3(horizontal,0,vertical);
            Release.Invoke();
            transform.position = moveBackPos;
            transform.localPosition = Vector3.zero;
            Owner.gameObject.GetComponentInChildren<LineRenderer>().enabled = false;
            Owner.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            
        }

        private void ButtonHold()
        {
            
        }

        private void ButtonRelease()
        {
            
        }
        



        

        
    }
}