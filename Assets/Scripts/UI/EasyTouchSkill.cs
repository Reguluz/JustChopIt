using GamePlayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class EasyTouchSkill : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public float DistanceRatio=1;
        
        //长按事件
        public UnityEvent Hold;
        //释放事件
        public UnityEvent Release;
        //使用对象
        public GamePlayerController Owner;
        //适用对象的辅助UI
        public GameObject GuideUI;

        //是否为线性引导
        public bool isLine;
        //图标移动最大半径
        public float maxTouchRadius;
        //初始化背景图标位置
        private Vector2 moveBackPos;
    
        //hor,ver的属性访问器
        private float horizontal=0;
        private float vertical=0;
        private static float ratio = 0.1f;
        private bool _ondrag = false;
        //松手时的向量
        public Vector3 EndVector;

        private void Start()
        {
            
        }

        public void Init(GamePlayerController owner,bool isline)
        {
            //初始化设置
            Owner = owner;
            GuideUI = Owner.transform.Find("GuideUI").gameObject;
            this.isLine = isline;
        }
        
        private void Update () {
            horizontal = transform.localPosition.x*ratio*DistanceRatio;
            vertical = transform.localPosition.y*ratio*DistanceRatio;
            
            //按住状态持续修改引导UI的状态
            if (_ondrag)
            {
                if (isLine)
                {
                    GuideUI.GetComponentInChildren<LineRenderer>()?.SetPosition(0,Owner.transform.position+new Vector3(0,-0.75f,0));
                    GuideUI.GetComponentInChildren<LineRenderer>()?.SetPosition(1,Owner.transform.position+new Vector3(horizontal,-0.75f,vertical));
                }
                else
                {
                    GuideUI.transform.position = Owner.transform.position + new Vector3(horizontal, 0.25f-1, vertical);
                }
            }
            moveBackPos = transform.parent.transform.position;
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            _ondrag = true;
            EndVector = Vector3.zero;
            //获取向量的长度    
            Vector2 oppsitionVec = eventData.position - moveBackPos;
            Debug.DrawLine(Vector3.zero, eventData.position,Color.green);
            Debug.DrawLine(Owner.transform.position, Owner.transform.position + new Vector3(horizontal,0.25f,vertical),Color.white);
           
            //最小值与最大值之间取半径
            float distance = Vector3.Magnitude(oppsitionVec);
            //限制半径长度
            float radius = Mathf.Clamp(distance, 0, maxTouchRadius);
            transform.position = moveBackPos + oppsitionVec.normalized * radius;
            if (isLine)
            {
                GuideUI.GetComponentInChildren<LineRenderer>().enabled = true;
            }
            else
            {
                GuideUI.GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
            

        }

        public void OnEndDrag(PointerEventData eventData)
        {

            _ondrag = false;
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