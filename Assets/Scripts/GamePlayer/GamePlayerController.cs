using System.Collections.Generic;
using GamePlayer.Characters;
using Items.Buff;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
    [RequireComponent(typeof(PhotonView))]
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
        
        protected PlayerProperties Properties;
        protected PhotonView PhotonView;
        private MoveController _moveController;
        protected List<ParticleSystem> BuffParticles;

        
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
                _moveController = gameObject.GetComponent<MoveController>();
                UiController = GameObject.Find("GameCanvas").GetComponent<UIController>();
                //UI绑定数据
                UiController.PlayerProperties = Properties;
                //技能设置
                SetSkillButton();
                //移动控制设置
                _moveController.RotateLevel = StaticData.RotateSpeed * SkillCo.RotateSpeed * BuffCo.RotateSpeed;
                _moveController.SpeedLevel = StaticData.MoveSpeed * SkillCo.MoveSpeed * BuffCo.MoveSpeed;
                //向属性控制注册
                PropControllerRegister();
            }
        }
        
        void FixedUpdate()
        {
            if (PhotonView.IsMine)
            {
                _moveController.SpeedLevel = StaticData.MoveSpeed + SkillCo.MoveSpeed + BuffCo.MoveSpeed;
                _moveController.RotateLevel = StaticData.RotateSpeed + SkillCo.RotateSpeed + BuffCo.MoveSpeed;
            }
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
            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Bufftype.Equals(Bufftype.Shield))
                {
                    Buffs[i].RemoveBuff(this);
                    Buffs.Remove(Buffs[i]);
                    return false;
                }
            }
            return true;
        }    

        public void RefreshShow()
        {
            UiController.Refresh();
        }    //刷新

        public virtual void SetCharacterType()
        {
            
        }
        
        protected virtual void Dodge()
        {
            
        }

        protected virtual void EndDodge()
        {
           
        }

        public virtual void Rebuild()    //状态重置
        {
            
        }

       
    }
}