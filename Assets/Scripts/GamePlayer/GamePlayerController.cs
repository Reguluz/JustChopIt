﻿using GamePlayer.Characters;
using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PlayerProperties))]
    [RequireComponent(typeof(MoveController))]
    public  class GamePlayerController:MonoBehaviour,IGamePlayerControl
    {
        public  UIController              UiController;
        protected CoolDownImageController[] Cooldown;
        public  float                       SpeedLevel;
        public  float                       RotateLevel;
        
        public SkillBaseInfo[] ActiveSkillInfo;    //技能信息
        
        protected PlayerProperties Properties;
        protected PhotonView PhotonView;
        protected MoveController MoveController;

        public void Start()
        {
            //获取自身组件
            PhotonView = GetComponent<PhotonView>();
            Properties     =gameObject.GetComponent<PlayerProperties>();
            
            
          
			
            //控制器绑定
            if (PhotonView.IsMine)
            {
                MoveController = gameObject.GetComponent<MoveController>();
                UiController = GameObject.Find("GameCanvas").GetComponent<UIController>();
                //UI绑定数据
                UiController.PlayerProperties = Properties;
                //技能设置
                SetSkillButton();
                //移动控制设置
                MoveController.RotateLevel = RotateLevel;
                MoveController.SpeedLevel = SpeedLevel;
                //向属性控制注册
                PropControllerRegister();
            }
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