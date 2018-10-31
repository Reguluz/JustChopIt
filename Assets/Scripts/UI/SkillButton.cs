using GamePlayer;
using UnityEngine;

namespace UI
{
    public interface SkillButton
    {
        void SetSkill(SkillBaseInfo baseInfo);
        void RegisterOwner(GamePlayerController controller);
        void SkillButtonPressed();
        void SkillButtonReleased();
    }
}