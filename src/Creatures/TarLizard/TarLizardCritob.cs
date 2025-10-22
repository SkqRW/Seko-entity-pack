using DevInterface;
using Fisobs.Core;
using Fisobs.Creatures;
using System.Collections.Generic;
using UnityEngine;
using Fisobs.Sandbox;

namespace Creatures.TarLizard;

sealed class TarLizardCritob : Critob
{
    internal TarLizardCritob() : base(Enums.CreatureTemplateType.TarLizard)
    {
        Icon = new SimpleIcon("Kill_Standard_Lizard", Color.gray);
        LoadedPerformanceCost = 50f;
        SandboxPerformanceCost = new(.5f, .5f);
        RegisterUnlock(KillScore.Configurable(9), Enums.SandboxUnlockID.TarLizard, MultiplayerUnlocks.SandboxUnlockID.Slugcat);
    }

    public override int ExpeditionScore() => 9;

    public override CreatureTemplate.Type? ArenaFallback() => CreatureTemplate.Type.GreenLizard;

    public override Color DevtoolsMapColor(AbstractCreature acrit) => Color.gray;

    public override string DevtoolsMapName(AbstractCreature acrit) => "Tar";

    
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

    public override IEnumerable<string> WorldFileAliases() => ["tar lizard", "tarlizard", "tar"];

    //public override ArtificialIntelligence? CreateRealizedAI(AbstractCreature acrit) => new LunarLizardAI(acrit, acrit.world);
    public override ArtificialIntelligence? CreateRealizedAI(AbstractCreature acrit) => new TarLizardAI(acrit, acrit.world);

    public override Creature CreateRealizedCreature(AbstractCreature acrit) => new TarLizard(acrit, acrit.world);

    public override CreatureState CreateState(AbstractCreature acrit) => new LizardState(acrit);

    public override void LoadResources(RainWorld rainWorld) { }
}