using DevInterface;
using Fisobs.Core;
using Fisobs.Creatures;
using System.Collections.Generic;
using UnityEngine;
using Fisobs.Sandbox;

namespace Creatures.LunarLizard;

sealed class LunarLizardCritob : Critob
{
    internal LunarLizardCritob() : base(Enums.CreatureTemplateType.LunarLizard)
    {
        Icon = new SimpleIcon("Kill_Standard_Lizard", Color.yellow);
        LoadedPerformanceCost = 50f;
        SandboxPerformanceCost = new(.5f, .5f);
        RegisterUnlock(KillScore.Configurable(9), Enums.SandboxUnlockID.LunarLizard);
    }

    public override int ExpeditionScore() => 9;

    public override CreatureTemplate.Type? ArenaFallback() => CreatureTemplate.Type.WhiteLizard;

    public override Color DevtoolsMapColor(AbstractCreature acrit) => Color.white;

    public override string DevtoolsMapName(AbstractCreature acrit) => "LunL";

    
    public override IEnumerable<RoomAttractivenessPanel.Category> DevtoolsRoomAttraction() =>
    [
        RoomAttractivenessPanel.Category.Lizards,
        RoomAttractivenessPanel.Category.LikesOutside
    ];
    

    public override CreatureTemplate CreateTemplate() => LizardBreeds.BreedTemplate(Type, StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LizardTemplate), StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.PinkLizard), StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.BlueLizard), StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.GreenLizard));

    public override void EstablishRelationships()
    {
        var p = new Relationships(Type);
        p.Rivals(Type, .1f);
        p.Rivals(CreatureTemplate.Type.LizardTemplate, .2f);
        p.Rivals(CreatureTemplate.Type.WhiteLizard, .5f);
        if (ModManager.DLCShared)
        {
            p.IgnoredBy(DLCSharedEnums.CreatureTemplateType.ZoopLizard);
            p.Ignores(DLCSharedEnums.CreatureTemplateType.ZoopLizard);
        }
    }

    public override IEnumerable<string> WorldFileAliases() => ["lunar lizard", "lunarlizard", "lunar"];

    //public override ArtificialIntelligence? CreateRealizedAI(AbstractCreature acrit) => new LunarLizardAI(acrit, acrit.world);
    public override ArtificialIntelligence? CreateRealizedAI(AbstractCreature acrit) => new LizardAI(acrit, acrit.world);

    public override Creature CreateRealizedCreature(AbstractCreature acrit) => new LunarLizard(acrit, acrit.world);

    public override CreatureState CreateState(AbstractCreature acrit) => new LizardState(acrit);

    public override void LoadResources(RainWorld rainWorld) { }
}