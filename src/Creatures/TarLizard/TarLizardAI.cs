using UnityEngine;

namespace Creatures.TarLizard;

public class TarLizardAI : LizardAI
{
    public TarLizardAI(AbstractCreature abstractCreature, World world) : base(abstractCreature, world)
    {

        base.pathFinder.stepsPerFrame = 30;
        this.redSpitAI = new LizardAI.LizardSpitTracker(this);
        base.AddModule(this.redSpitAI);
        /*
        AddModule(lurkTracker = new(this, lizard));
        utilityComparer.AddComparedModule(lurkTracker, null, Mathf.Lerp(.4f, .3f, creature.personality.energy), 1f);
        */
    }

    public override void Update()
    {
        base.Update();
        UnityEngine.Debug.Log($"TarLizardAI Update - Current behavior: {this.behavior} | Spitting: {this.redSpitAI.spitting}");
        if (this.redSpitAI.spitting)
        {
            this.lizard.EnterAnimation(Lizard.Animation.Spit, true);
        }
    }
}