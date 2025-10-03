using System.Collections.Generic;
using Fisobs.Core;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace Creatures.LunarLizard;

public class Main
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
        if (type == Enums.CreatureTemplateType.LunarLizard)
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
            temp.name = "LunarLizard";
            temp.throwAction = "Hiss";
            temp.requireAImap = true;
            temp.doPreBakedPathing = false;
            breedParams.standardColor = Color.cyan;
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
            if (l is Creatures.LunarLizard.LunarLizard)
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
        public static CreatureTemplate.Type LunarLizard = new(nameof(LunarLizard), true);
        public void UnregisterValues()
        {
            if (LunarLizard != null)
            {
                LunarLizard.Unregister();
                LunarLizard = null;
            }
        }
    }

    public class SandboxUnlockID
    {
        public static MultiplayerUnlocks.SandboxUnlockID LunarLizard = new(nameof(LunarLizard), true);

        public void UnregisterValues()
        {
            if (LunarLizard != null)
            {
                LunarLizard.Unregister();
                LunarLizard = null;
            }
        }
    }
}
