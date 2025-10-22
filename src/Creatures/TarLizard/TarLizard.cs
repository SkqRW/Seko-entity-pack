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


    protected new virtual float ActAnimation()
    {
        float num = 1f;
        if (this.animation == Lizard.Animation.Spit)
		{
			num = 0f;
			this.bodyWiggleCounter = 0;
			this.JawOpen = Mathf.Clamp(this.JawOpen + 0.2f, 0f, 1f);
            if (!this.AI.redSpitAI.spitting && !base.safariControlled)
            {
                this.EnterAnimation(Lizard.Animation.Standard, true);
            }
            else
            {
                Vector2? vector2 = this.AI.redSpitAI.AimPos();
                if (vector2 != null)
                {
                    if (this.AI.redSpitAI.AtSpitPos)
                    {
                        Vector2 vector3 = this.room.MiddleOfTile(this.AI.redSpitAI.spitFromPos);
                        base.mainBodyChunk.vel += Vector2.ClampMagnitude(vector3 - Custom.DirVec(vector3, vector2.Value) * this.bodyChunkConnections[0].distance - base.mainBodyChunk.pos, 10f) / 5f;
                        base.bodyChunks[1].vel += Vector2.ClampMagnitude(vector3 - base.bodyChunks[1].pos, 10f) / 5f;
                    }
                    if (!this.AI.UnpleasantFallRisk(this.room.GetTilePosition(base.mainBodyChunk.pos)))
                    {
                        base.mainBodyChunk.vel += Custom.DirVec(base.mainBodyChunk.pos, vector2.Value) * 4f * (float)this.LegsGripping;
                        base.bodyChunks[1].vel -= Custom.DirVec(base.mainBodyChunk.pos, vector2.Value) * 2f * (float)this.LegsGripping;
                        base.bodyChunks[2].vel -= Custom.DirVec(base.mainBodyChunk.pos, vector2.Value) * 2f * (float)this.LegsGripping;
                    }
                    if (this.AI.redSpitAI.delay < 1)
                    {
                        Vector2 vector4 = base.bodyChunks[0].pos + Custom.DirVec(base.bodyChunks[1].pos, base.bodyChunks[0].pos) * 10f;
                        Vector2 vector5 = Custom.DirVec(vector4, vector2.Value);
                        if (Vector2.Dot(vector5, Custom.DirVec(base.bodyChunks[1].pos, base.bodyChunks[0].pos)) > 0.3f || base.safariControlled)
                        {
                            if (base.safariControlled)
                            {
                                this.EnterAnimation(Lizard.Animation.Standard, true);
                                this.LoseAllGrasps();
                            }
                            this.room.PlaySound(SoundID.Red_Lizard_Spit, vector4, base.abstractCreature);
                            this.room.AddObject(new LizardSpit(vector4, vector5 * 40f, this));
                            this.AI.redSpitAI.delay = 12;
                            base.bodyChunks[2].pos -= vector5 * 8f;
                            base.bodyChunks[1].pos -= vector5 * 4f;
                            base.bodyChunks[2].vel -= vector5 * 2f;
                            base.bodyChunks[1].vel -= vector5 * 1f;
                            this.JawOpen = 1f;
                        }
                    }
                }
            }
            return num;
		}
        return base.ActAnimation();
    }
}