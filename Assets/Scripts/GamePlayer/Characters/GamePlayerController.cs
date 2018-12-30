using System.Collections.Generic;
using Items.Buff;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    [RequireComponent(typeof(PhotonRigidbodyView))]
    [RequireComponent(typeof(PlayerSetup))]
    [RequireComponent(typeof(PlayerProperties))]
    [RequireComponent(typeof(MoveController))]
    public  class GamePlayerController:MonoBehaviour,IGamePlayerControl,IPunObservable
    {
        public  UIController              UiController;
        protected CoolDownImageController[] Cooldown;
        public CharacterData StaticData = new CharacterData(1,1,1) ;
        public CharacterData SkillCo = new CharacterData(0,0,0);
        public CharacterData BuffCo = new CharacterData(0,0,0);
        public List<PlayerBuff> Buffs = new List<PlayerBuff>();
        
        public SkillBaseInfo[] ActiveSkillInfo;    //技能信息

        protected CharacterFxController _fxController;
        protected PlayerProperties Properties;
        protected PhotonView PhotonView;
        private MoveController _moveController;
        protected List<ParticleSystem> BuffParticles;

        public delegate bool DamageCalculate(GamePlayerController controller,bool isHurt);
        public DamageCalculate DamageCalculator;
        public CharacterType CharacterType;

        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
        
        
        public void Start()
        {
            //获取自身组件
            PhotonView = GetComponent<PhotonView>();
            Properties     =gameObject.GetComponent<PlayerProperties>();			
            //控制器绑定
            if (PhotonView.IsMine)
            {
                ControllerRegister();
            }
        }

        private void ControllerRegister()
        {
            _moveController = gameObject.GetComponent<MoveController>();
            UiController = GameObject.Find("GameCanvas").GetComponent<UIController>();
            //UI绑定数据
            UiDataLink();
            //技能设置
            SetSkillButton();
            //移动控制设置
            SetMoveData();
            //向属性控制注册
            PropControllerRegister();
            //注册角色种类
            SetCharacterType();
        }

        private void SetMoveData()
        {
            _moveController.RotateLevel = StaticData.RotateSpeed + SkillCo.RotateSpeed + BuffCo.RotateSpeed;
            _moveController.SpeedLevel = StaticData.MoveSpeed + SkillCo.MoveSpeed + BuffCo.MoveSpeed;
        }

        private void UiDataLink()
        {
            UiController.PlayerProperties = Properties;
        }

        void FixedUpdate()
        {
            if (PhotonView.IsMine)
            {
                SetMoveData();
            }
            RuntimeBuff();
        }

        private void RuntimeBuff()
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buffs[i].Interval += Time.deltaTime;
                if (Buffs[i].CheckOvertime())
                {
                    Buffs[i].RemoveBuff(this);
                    Buffs.Remove(Buffs[i]);
                }
            }
        }

        [PunRPC]
        public void AddBuff(Bufftype buff)
        {
            int count = 0;
            int firstserial=0;
            var temp = BuffChecker.Check(buff);
            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Bufftype.Equals(buff))
                {
                    count++;
                    if (count == 1)
                    {
                        firstserial = i;
                    }
                    if (count == 2)
                    {
                        Buffs[firstserial].RemoveBuff(this);
                        Buffs.RemoveAt(firstserial);
                        firstserial = 0;
                        count = 0;
                    }
                }
            }
            Buffs.Add(temp);
            temp.GetBuff(this);
            Debug.Log("Buffco"+BuffCo.MoveSpeed);
        }

        [PunRPC]
        public void RemoveAllBuff()
        {
            foreach (PlayerBuff playerBuff in Buffs)
            {
                playerBuff.RemoveBuff(this);
            }
            Buffs.Clear();
        }
        //属性控制器注册
        public void PropControllerRegister()
        {
            Properties.Controller = this;
        }    

        //技能释放控制(在子类设置）
        [PunRPC]
        public virtual void SkillRelease(int skillnum,Vector3 direction)
        {
            
        }   

        //技能UI设置
        public void SetSkillButton()
        {
            for (int i = 0; i < ActiveSkillInfo.Length; i++)
            {
                //设置技能序号
                int serial = ActiveSkillInfo[i].Pos;
                //获取技能按钮
                Debug.Log(serial+"  "+UiController.Skill.Length);
                Cooldown[serial] = UiController.Skill[serial].GetComponent<CoolDownImageController>();
                //控制器注册到技能按钮
                Cooldown[serial].RegisterOwner(this);
                //设置技能参数
                Cooldown[serial].SetSkill(ActiveSkillInfo[i]);
                //设置技能可用
                Cooldown[serial].SkillActived = true;
				
            }
            //隐藏不使用的按钮
            UiController.RejectorBlank();	
        }    

        //被动伤害过滤

        
        public virtual bool DamageFilter()
        {

            return DamageCalculator(this, false);
            /*for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Bufftype.Equals(Bufftype.Shield))
                {
                    Buffs[i].RemoveBuff(this);
                    Buffs.Remove(Buffs[i]);
                    return false;
                }
            }
            return true;*/
        }    

        public void RefreshShow()
        {
            UiController.Refresh();
        }    //刷新

        public virtual void SetCharacterType()
        {
            
        }
        
        

        public void BulletShoot()
        {
            //PhotonView.RPC("ShootFx",RpcTarget.All);
            if (PhotonView.IsMine)
            {
                Debug.Log("Shoot");
                //GameObject shriken = PhotonNetwork.PrefabPool.Instantiate("Player/Derivative/" + ShrikenPrefab.name, transform.position,transform.rotation);
                GameObject bullet = PhotonNetwork.Instantiate("Bullet", transform.position, transform.rotation, 0);		
                PhotonView pv = bullet.GetComponent<PhotonView>();
                pv.RPC("SetMesh",RpcTarget.All,Properties.CharacterType);
                pv.RPC("SetOwner", RpcTarget.All,gameObject.GetComponent<PhotonView>().ViewID);		
                pv.RPC("SetDirection",RpcTarget.All,transform.forward);
            }
            _fxController.PlayFx("SkillRelease");
        }

        public virtual void Rebuild()    //状态重置
        {
            
        }

       
    }
}