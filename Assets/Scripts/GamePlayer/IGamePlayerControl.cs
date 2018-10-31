using UnityEngine;

namespace GamePlayer
{
    public interface IGamePlayerControl
    {
        void PropControllerRegister();
        void SkillRelease(int skillnum,Vector3 direction);
        void SetSkillButton();
        bool DamageFilter();
        void RefreshShow();
    }
}