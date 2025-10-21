using System.Collections.Generic;
using LizardCosmetics;
using UnityEngine;

namespace Creatures.TarLizard;

public class TarLizardGraphics : LizardGraphics
{
    public TarLizardGraphics(TarLizard ow) : base(ow)
    {
        List<BodyPart> list = new List<BodyPart>();
        var state = Random.state;
        Random.InitState(ow.abstractPhysicalObject.ID.RandomSeed);


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
        sLeaser.RemoveAllSpritesFromContainer();
        base.AddToContainer(sLeaser, rCam, newContatiner);
    }
}