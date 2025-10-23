using UnityEngine;
using Watcher;
using RWCustom;

namespace Creatures.TarLizard;

public class TarLizard : Lizard
{
    public TarLizard(AbstractCreature abstractCreature, World world) : base(abstractCreature, world)
    {
        effectColor = lizardParams.standardColor;
        if (rotModule is LizardRotModule mod && LizardState.rotType != LizardState.RotType.Slight)
            effectColor = Color.Lerp(effectColor, mod.RotEyeColor, LizardState.rotType == LizardState.RotType.Opossum ? .2f : .8f);
    }

    public override void InitiateGraphicsModule() => graphicsModule ??= new TarLizardGraphics(this);
    public override void LoseAllGrasps() => ReleaseGrasp(0);


}