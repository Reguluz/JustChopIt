using Photon.Pun;
using UnityEngine;

namespace GamePlayer
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PlayerProperties))]
    [RequireComponent(typeof(MoveController))]
    public abstract class GamePlayerController:MonoBehaviour,IGamePlayerControl
    {
        public  UIController              UiController;
        protected CoolDownImageController[] Cooldown;
        public  int                       SpeedLevel;
        public  int                       RotateLevel;
        
        public SkillBaseInfo[] ActiveSkillInfo;    //技能信息
        
        protected PlayerProperties Properties;
        protected PhotonView PhotonView;
        protected MoveController MoveController;
        
        public abstract void PropControllerRegister();    //属性控制器注册

        public abstract void SkillRelease(int skillnum,Vector3 direction);    //技能释放控制

        public abstract void SetSkillButton();    //技能UI设置
    
        public abstract bool DamageFilter();    //被动伤害过滤

        public abstract void RefreshShow();    //刷新
    }
}