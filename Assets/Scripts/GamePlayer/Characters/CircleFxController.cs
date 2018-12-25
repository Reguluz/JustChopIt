using Photon.Pun;
using UnityEngine;

namespace GamePlayer.Characters
{
	public class CircleFxController : CharacterFxController
	{
		public override void PlayFx(string fxname)
		{
			switch (fxname)
			{
				case "Dead": DeadFx();
					break;
				case "Relieve":RelieveFx();
					break;
				case "Dodge": DodgeFx();
					break;
				case "Rebuild":RebuildFx();
					break;
				case "SkillRelease":SkillRelease();
					break;
			}
		}
		
		
	}
}
