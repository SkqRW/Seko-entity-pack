using System.Collections.Generic;
using LizardCosmetics;
using UnityEngine;

namespace Creatures.LunarLizard;

public class LunarLizardGraphics : LizardGraphics
{
    public LunarLizardGraphics(LunarLizard ow) : base(ow)
    {
        List<BodyPart> list = new List<BodyPart>();
        var state = Random.state;
        Random.InitState(ow.abstractPhysicalObject.ID.RandomSeed);


        //limbs
        this.limbs = new LizardLimb[6];
        for (int i = 0; i < this.limbs.Length; i++)
        {
            int num = (i < 2) ? 0 : 2;
                //num = i / 2;
            
            this.limbs[i] = new LizardLimb(this, base.owner.bodyChunks[num], i, 2.5f, 0.7f, 0.99f, this.lizard.lizardParams.limbSpeed, this.lizard.lizardParams.limbQuickness, (i % 2 == 1) ? this.limbs[i - 1] : null);
            list.Add(this.limbs[i]);
        }


        //tail
        for (int j = 0; j < this.lizard.lizardParams.tailSegments; j++)
        {
            list.Add(tail[j]);
        }

        //head
        list.Add(this.head);

        this.bodyParts = list.ToArray();

        var spriteIndex = startOfExtraSprites + extraSprites;
        //spriteIndex = this.AddCosmetic(spriteIndex, new Antennae(this, spriteIndex));
        Random.state = state;
    }

    public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
    {
        UnityEngine.Debug.Log($"{sLeaser != null}");
        UnityEngine.Debug.Log($"{sLeaser.sprites.Length} - {this.TotalSprites}");
        UnityEngine.Debug.Log($"{sLeaser.containers != null} AO");
        sLeaser.RemoveAllSpritesFromContainer();
        UnityEngine.Debug.Log($"{sLeaser.sprites.Length} - {this.TotalSprites} ||||");
        base.AddToContainer(sLeaser, rCam, newContatiner);
    }
}