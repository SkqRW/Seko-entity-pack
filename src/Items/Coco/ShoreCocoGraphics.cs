namespace Items.ShoreCoco;

public partial class ShoreCoco : IDrawable
{
    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[1];
        sLeaser.sprites[0] = new FSprite("Circle20", true);
        sLeaser.sprites[0].scale = this.bodyChunks[0].rad / 10f;
        AddToContainer(sLeaser, rCam, null);
    }

    public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        sLeaser.sprites[0].x = this.bodyChunks[0].pos.x - camPos.x;
        sLeaser.sprites[0].y = this.bodyChunks[0].pos.y - camPos.y;
        sLeaser.sprites[0].color = this.color;

        sLeaser.sprites[0].isVisible = visible;

        if (base.slatedForDeletetion || this.room != rCam.room)
        {
            sLeaser.CleanSpritesAndRemove();
        }
    }

    public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        color = Color.blue;
        sLeaser.sprites[0].color = color;
    }

    public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        newContainer ??= rCam.ReturnFContainer("Items");
        foreach (FSprite sprite in sLeaser.sprites)
            newContainer.AddChild(sprite);
    }
}
