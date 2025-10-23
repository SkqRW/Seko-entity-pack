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
        On.Lizard.ActAnimation -= ActAnimation;
        On.LizardAI.AggressiveBehavior -= AggressiveBehavior;
    }
    public static void Init()
    {
        On.LizardBreeds.BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate += On_LizardBreeds_BreedTemplate_Type_CreatureTemplate_CreatureTemplate_CreatureTemplate_CreatureTemplate;
        On.LizardVoice.GetMyVoiceTrigger += On_LizardVoice_GetMyVoiceTrigger;
        On.Lizard.ActAnimation += ActAnimation;
        On.LizardAI.AggressiveBehavior += AggressiveBehavior;
    }

    private static void AggressiveBehavior(On.LizardAI.orig_AggressiveBehavior orig, LizardAI self, Tracker.CreatureRepresentation target, float tongueChance)
    {
        orig(self, target, tongueChance);
        if (self.lizard is TarLizard)
        {
            if (target.VisualContact)
            {
                self.lizard.JawOpen = Mathf.Clamp(self.lizard.JawOpen + 0.4f, 0f, 1f);
            }
        }
    }

    private static float ActAnimation(On.Lizard.orig_ActAnimation orig, Lizard self)
    {
        float num = 0f;
        if (self is TarLizard tarLizard)
        {
            if (self.animation == Lizard.Animation.Spit)
            {
                num = 0f;
                self.bodyWiggleCounter = 0;
                self.JawOpen = Mathf.Clamp(self.JawOpen + 0.2f, 0f, 1f);
                if (!self.AI.redSpitAI.spitting && !self.safariControlled)
                {
                    self.EnterAnimation(Lizard.Animation.Standard, true);
                }
                else
                {
                    Vector2? vector2 = self.AI.redSpitAI.AimPos();
                    if (vector2 != null)
                    {
                        if (self.AI.redSpitAI.AtSpitPos)
                        {
                            Vector2 vector3 = self.room.MiddleOfTile(self.AI.redSpitAI.spitFromPos);
                            self.mainBodyChunk.vel += Vector2.ClampMagnitude(vector3 - Custom.DirVec(vector3, vector2.Value) * self.bodyChunkConnections[0].distance - self.mainBodyChunk.pos, 10f) / 5f;
                            self.bodyChunks[1].vel += Vector2.ClampMagnitude(vector3 - self.bodyChunks[1].pos, 10f) / 5f;
                        }
                        if (!self.AI.UnpleasantFallRisk(self.room.GetTilePosition(self.mainBodyChunk.pos)))
                        {
                            self.mainBodyChunk.vel += Custom.DirVec(self.mainBodyChunk.pos, vector2.Value) * 4f * (float)self.LegsGripping;
                            self.bodyChunks[1].vel -= Custom.DirVec(self.mainBodyChunk.pos, vector2.Value) * 2f * (float)self.LegsGripping;
                            self.bodyChunks[2].vel -= Custom.DirVec(self.mainBodyChunk.pos, vector2.Value) * 2f * (float)self.LegsGripping;
                        }
                        if (self.AI.redSpitAI.delay < 1)
                        {
                            Vector2 vector4 = self.bodyChunks[0].pos + Custom.DirVec(self.bodyChunks[1].pos, self.bodyChunks[0].pos) * 10f;
                            Vector2 vector5 = Custom.DirVec(vector4, vector2.Value);
                            if (Vector2.Dot(vector5, Custom.DirVec(self.bodyChunks[1].pos, self.bodyChunks[0].pos)) > 0.3f || self.safariControlled)
                            {
                                if (self.safariControlled)
                                {
                                    self.EnterAnimation(Lizard.Animation.Standard, true);
                                    self.LoseAllGrasps();
                                }
                                self.room.PlaySound(SoundID.Bomb_Explode, vector4, self.abstractCreature);
                                //self.room.AddObject(new LizardSpit(vector4, vector5 * 40f, self));
                                UnityEngine.Debug.Log("TarLizard Spit!");
                                
                                
                                UnityEngine.Debug.Log("TarLizard Spit the ROCK!");
                                self.AI.redSpitAI.delay = 8;
                                self.bodyChunks[2].pos -= vector5 * 8f;
                                self.bodyChunks[1].pos -= vector5 * 4f;
                                self.bodyChunks[2].vel -= vector5 * 2f;
                                self.bodyChunks[1].vel -= vector5 * 1f;
                                self.JawOpen = 2f;

                                Vector2 temp = self.mainBodyChunk.vel;
                                self.mainBodyChunk.vel.x = ((self.mainBodyChunk.vel.x > 0) ? 1 : -1) * 4f;
                                AbstractPhysicalObject abstractPhysicalObject = new AbstractPhysicalObject(self.abstractCreature.world,
                                                                                                        AbstractPhysicalObject.AbstractObjectType.Rock,
                                                                                                        null,
                                                                                                        self.room.ToWorldCoordinate(vector4),
                                                                                                        self.room.game.GetNewID());
                                
                                Rock rock = new Rock(abstractPhysicalObject, self.room.world);
                                rock.PlaceInRoom(self.room);
                                float a = vector5.x;
                                float b = vector5.y;
                                rock.Thrown(self,
                                            vector4 + vector5,
                                            null,
                                            new IntVector2((a > 0f) ? 1 : -1, (b > 0f) ? 1 : -1),
                                            0.8f,
                                            true);
                                UnityEngine.Debug.Log("TarLizard Spit the ROCK in the room! with velocity: " + rock.firstChunk.vel + "and lizard velocity: " + self.mainBodyChunk.vel + " ||| " + vector5);
                                self.mainBodyChunk.vel = temp;
                            }
                        }
                    }
                }
                return num;
            }
        }
        return orig(self);
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


            breedParams.terrainSpeeds[1] = new LizardBreedParams.SpeedMultiplier(3f, 3f, 0.8f, 2f);
			//list.Add(new TileTypeResistance(AItile.Accessibility.Floor, 1f, PathCost.Legality.Allowed));
			breedParams.terrainSpeeds[2] = new LizardBreedParams.SpeedMultiplier(2f, 0.9f, 1.4f, 1.9f);
			//list.Add(new TileTypeResistance(AItile.Accessibility.Corridor, 2f, PathCost.Legality.Allowed));
			temp.waterPathingResistance = 1.5f;
			//list2.Add(new TileConnectionResistance(MovementConnection.MovementType.DropToFloor, 40f, PathCost.Legality.Allowed));
			//list2.Add(new TileConnectionResistance(MovementConnection.MovementType.LizardTurn, 60f, PathCost.Legality.Allowed));
			breedParams.biteDelay = 6;
			breedParams.biteInFront = 15f;
			breedParams.biteHomingSpeed = 1.4f;
			breedParams.biteChance = 0.46511626f;
			breedParams.attemptBiteRadius = 90f;
			breedParams.getFreeBiteChance = 0.72f;
			breedParams.biteDamage = 1.2f;
			breedParams.biteDamageChance = 0.5f;
			breedParams.toughness = 2.5f;
			breedParams.stunToughness = 2.5f;
			breedParams.regainFootingCounter = 10;
			breedParams.baseSpeed = 0.65f;
			breedParams.bodyMass = 4f;
			breedParams.bodySizeFac = 1.75f;
			breedParams.floorLeverage = 0.2f;
			breedParams.maxMusclePower = 3f;
			breedParams.danger = 0.8f;
			breedParams.aggressionCurveExponent = 0.95f;
			breedParams.wiggleSpeed = 0f;
			breedParams.wiggleDelay = 55;
			breedParams.bodyStiffnes = 0.2f;
			breedParams.swimSpeed = 0.45f;
			breedParams.idleCounterSubtractWhenCloseToIdlePos = 0;
			breedParams.biteDominance = 0.4f;
			breedParams.headShieldAngle = 100f;
			breedParams.canExitLounge = false;
			breedParams.canExitLoungeWarmUp = false;
			breedParams.findLoungeDirection = 0.5f;
			breedParams.loungeDistance = 400f;
			breedParams.preLoungeCrouch = 40;
			breedParams.preLoungeCrouchMovement = -0.2f;
			breedParams.loungeSpeed = 4.5f;
			breedParams.loungeMaximumFrames = 20;
			breedParams.loungePropulsionFrames = 10;
			breedParams.loungeJumpyness = 1f;
			breedParams.loungeDelay = 200;
			breedParams.riskOfDoubleLoungeDelay = 0.1f;
			breedParams.postLoungeStun = 70;
			breedParams.loungeTendensy = 0.85f;
			temp.visualRadius = 2300f;
			temp.waterVision = 0.7f;
			temp.throughSurfaceVision = 0.95f;
			breedParams.perfectVisionAngle = Mathf.Lerp(1f, -1f, 0.44444445f);
			breedParams.periferalVisionAngle = Mathf.Lerp(1f, -1f, 0.7777778f);
			breedParams.biteDominance = 0.45f;
			breedParams.limbSize = 1.1f;
			breedParams.limbThickness = 1.5f;
			breedParams.stepLength = 0.5f;
			breedParams.liftFeet = 0.3f;
			breedParams.feetDown = 0.5f;
			breedParams.noGripSpeed = 0.1f;
			breedParams.limbSpeed = 5f;
			breedParams.limbQuickness = 0.5f;
			breedParams.limbGripDelay = 1;
			breedParams.smoothenLegMovement = true;
			breedParams.legPairDisplacement = 0.2f;
			breedParams.standardColor = new Color(0.55f, 0.4f, 0.2f);
			breedParams.walkBob = 4f;
			breedParams.tailSegments = 4;
			breedParams.tailStiffness = 250f;
			breedParams.tailStiffnessDecline = 0.2f;
			breedParams.tailLengthFactor = 1.2f;
			breedParams.tailColorationStart = 0.3f;
			breedParams.tailColorationExponent = 2f;
			breedParams.headSize = 1.2f;
			breedParams.neckStiffness = 0.2f;
			breedParams.jawOpenAngle = 90f;
			breedParams.jawOpenLowerJawFac = 0.6666667f;
			breedParams.jawOpenMoveJawsApart = 23f;
			breedParams.headGraphics = new int[]
			{
				1,
				1,
				1,
				1,
				1
			};
			breedParams.framesBetweenLookFocusChange = 160;
			breedParams.tamingDifficulty = 0.3f;
            temp.name = "TarLizard";
            temp.throwAction = "Spit";
            temp.requireAImap = true;
            temp.doPreBakedPathing = false;
            //breedParams.standardColor = new Color(27/255f, 32/255f, 36/255f);
            breedParams.standardColor = Color.gray;
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
