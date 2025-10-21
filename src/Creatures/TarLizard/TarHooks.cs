using System.Collections.Generic;
using Fisobs.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.TarLizard;

public class Hooks
{
    public static void Remove()
    {
        On.LizardBreeds.BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate -= On_LizardBreeds_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate;
        On.LizardVoice.GetMyVoiceTrigger -= On_LizardVoice_GetMyVoiceTrigger;
    }
    public static void Init()
    {
        On.LizardBreeds.BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate += On_LizardBreeds_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate;
        On.LizardVoice.GetMyVoiceTrigger += On_LizardVoice_GetMyVoiceTrigger;
    }
    internal static CreatureTemplate On_LizardBreeds_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate(On.LizardBreeds.orig_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate orig, CreatureTemplate.Type type, CreatureTemplate lizardAncestor, CreatureTemplate pinkTemplate, CreatureTemplate blueTemplate, CreatureTemplate greenTemplate)
    {
        CreatureTemplate temp;
        LizardBreedParams breedParams;
        if (type == Enums.CreatureTemplateType.TarLizard)
        {
            temp = orig(CreatureTemplate.Type.GreenLizard, lizardAncestor, pinkTemplate, blueTemplate, greenTemplate);
            breedParams = (LizardBreedParams)temp.breedParameters;
            breedParams.template = type;
            temp.dangerousToPlayer = breedParams.danger = .65f;
            temp.type = type;
            breedParams.bodyMass = 4f;
            breedParams.bodySizeFac = 2f;
            //breedParams.bodyLengthFac = 1f;
            //breedParams.bodyRadFac = 1f;
            breedParams.tailSegments = 7;
			breedParams.tailStiffness = 300f;
			breedParams.tailStiffnessDecline = 0.6f;
			breedParams.tailLengthFactor = 0.9f;
			breedParams.tailColorationStart = 0.05f;
			breedParams.tailColorationExponent = 4f;
            temp.name = "TarLizard";
            temp.throwAction = "Hiss";
            temp.requireAImap = true;
            temp.doPreBakedPathing = false;
            breedParams.standardColor = new Color(27/255f, 32/255f, 36/255f);
            temp.preBakedPathingAncestor = StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.WhiteLizard);
            return temp;
        }
        return orig(type, lizardAncestor, pinkTemplate, blueTemplate, greenTemplate);
    }

    // voice
    internal static SoundID On_LizardVoice_GetMyVoiceTrigger(On.LizardVoice.orig_GetMyVoiceTrigger orig, LizardVoice self)
    {
        var res = orig(self);
        List<SoundID> list;
        SoundID soundID;
        if (self.lizard is Lizard l)
        {
            if (l is Creatures.TarLizard.TarLizard)
            {
                // the voice here is green lizard, you can change that if needed
                var array = new[]
                {
                    SoundID.Lizard_Voice_Green_A
                };
                list = [];
                for (var i = 0; i < array.Length; i++)
                {
                    soundID = array[i];
                    if (soundID.Index != -1 && l.abstractPhysicalObject.world.game.soundLoader.workingTriggers[soundID.Index])
                        list.Add(soundID);
                }
                if (list.Count == 0)
                    res = SoundID.None;
                else
                    res = list[Random.Range(0, list.Count)];
            }

        }
        return res;
    }
}


public class Enums
{
    public class CreatureTemplateType
    {
        public static CreatureTemplate.Type TarLizard = new(nameof(TarLizard), true);
        public void UnregisterValues()
        {
            if (TarLizard != null)
            {
                TarLizard.Unregister();
                TarLizard = null;
            }
        }
    }

    public class SandboxUnlockID
    {
        public static MultiplayerUnlocks.SandboxUnlockID TarLizard = new(nameof(TarLizard), true);

        public void UnregisterValues()
        {
            if (TarLizard != null)
            {
                TarLizard.Unregister();
                TarLizard = null;
            }
        }
    }
}
