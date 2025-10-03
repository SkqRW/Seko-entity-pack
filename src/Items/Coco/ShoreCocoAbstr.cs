using Items.ShoreCoco;

namespace Items.ShoreCoco;

public class ShoreCocoAbstr : AbstractPhysicalObject
{
    public ShoreCocoAbstr(World world, WorldCoordinate pos, EntityID ID) : base(world, Enums.AbstractObjectType.ShoreCoco, null, pos, ID)
    {
        scaleX = 1;
        scaleY = 1;
        saturation = 0.5f;
    }

    public override void Realize()
    {
        base.Realize();
        if (realizedObject == null)
            realizedObject = new ShoreCoco(this, Room.realizedRoom.world);
    }

    public override string ToString()
    {
        return this.SaveToString($"{saturation};{scaleX};{scaleY}");
    }

    public float saturation;
    public float scaleX;
    public float scaleY;
}
