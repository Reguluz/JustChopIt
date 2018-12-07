using GamePlayer;
using GamePlayer.Characters;
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