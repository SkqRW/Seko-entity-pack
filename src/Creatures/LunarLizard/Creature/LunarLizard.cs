using UnityEngine;
using Watcher;
using RWCustom;

namespace Creatures.LunarLizard;

public class LunarLizard : Lizard
{
    public LunarLizard(AbstractCreature abstractCreature, World world) : base(abstractCreature, world)
    {
        effectColor = lizardParams.standardColor;
        if (rotModule is LizardRotModule mod && LizardState.rotType != LizardState.RotType.Slight)
            effectColor = Color.Lerp(effectColor, mod.RotEyeColor, LizardState.rotType == LizardState.RotType.Opossum ? .2f : .8f);
    }

    public override void InitiateGraphicsModule() => graphicsModule ??= new LunarLizardGraphics(this);

    public override void Update(bool eu)
    {
        for (int n = 0; n < this.room.world.game.Players.Count; n++)
        {
            if (this.room.world.game.Players[n] != null && !this.room.world.game.Players[n].slatedForDeletion
                //&& (base.abstractCreature as AbstractPhysicalObject).IsSameRippleLayer((this.room.world.game.Players[n] as AbstractPhysicalObject).rippleLayer;) 
                && this.room.world.game.Players[n].realizedCreature != null)
            {
                Player player = this.room.world.game.Players[n].realizedCreature as Player;
                if (player != null && Custom.DistLess(player.firstChunk.pos, base.firstChunk.pos, 150f) && this.room == player.room)
                {
                    int mushroomCounter = 10;
                    (this.room.world.game.Players[n].realizedCreature as Player).mushroomCounter = mushroomCounter;
                }
            }
        }
        base.Update(eu);
    }

    public override void LoseAllGrasps() => ReleaseGrasp(0);
}