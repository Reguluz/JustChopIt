using GamePlayer;
using GamePlayer.Characters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class EasyTouchSkill : MonoBehaviour, IDragHandler, IEndDragHandler,IPointerClickHandler
    {
        public float DistanceRatio=2;
        
        //长按事件
        public UnityEvent Hold;
        //释放事件
        public UnityEvent Release;
        //单击事件
        public UnityEvent Click;
        
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
            Hold.AddListener(new UnityAction(ButtonHold));
            Release.AddListener(new UnityAction(ButtonRelease));
            Click.AddListener(new UnityAction(ButtonClick));
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
                    GuideUI.GetComponentInChildren<LineRenderer>()?.SetPosition(0,Owner.transform.position+new Vector3(0,0.5f,0));
                    GuideUI.GetComponentInChildren<LineRenderer>()?.SetPosition(1,Owner.transform.position+new Vector3(horizontal,0.5f,vertical));
                }
                else
                {
                    GuideUI.transform.position = Owner.transform.position + new Vector3(horizontal, 0.5f, vertical);
                }
            }
            moveBackPos = transform.parent.transform.position;
            
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            //执行单击时运行的函数
            if (!_ondrag)
            {
                Click.Invoke();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            //设置状态
            _ondrag = true;
            EndVector = Vector3.zero;
            
            //执行开始拖拽时运行的函数
            Hold.Invoke();
            //获取向量的长度    
            Vector2 oppsitionVec = eventData.position - moveBackPos;
            Debug.DrawLine(Vector3.zero, eventData.position,Color.green);
            Debug.DrawLine(Owner.transform.position, Owner.transform.position + new Vector3(horizontal,0.5f,vertical),Color.white);
           
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
            
            //获取位置
            EndVector = new Vector3(horizontal,0,vertical);
            
            //执行停止拖拽时运行的函数
            Release.Invoke();
            
            //设置状态
            _ondrag = false;
            
            //复原
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

        private void ButtonClick()
        {
            
        }


       
    }
}