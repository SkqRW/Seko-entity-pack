

using Items.ShoreCoco.Effects;

namespace Items.ShoreCoco;

public partial class ShoreCoco : Rock
{
    public ShoreCoco(AbstractPhysicalObject abstr, World world) : base(abstr, world)
    {
        bodyChunks = new BodyChunk[1];
        bodyChunks[0] = new BodyChunk(this, 0, new Vector2(0f, 0f), radius, 0.2f);
        bodyChunkConnections = [];
        collisionLayer = DefaultCollLayer;
    }

    public override int DefaultCollLayer => 1;
    public int spark_iteration => 10;
    public float radius => 12f;

    public override void HitWall()
    {
        if (this.room.BeingViewed)
        {
            for (int i = 0; i < spark_iteration; i++)
            {
                this.room.AddObject(new Spark(base.firstChunk.pos + this.throwDir.ToVector2() * (base.firstChunk.rad - 1f), Custom.DegToVec(UnityEngine.Random.value * 360f) * 10f * UnityEngine.Random.value + -this.throwDir.ToVector2() * 10f, new Color(1f, 1f, 1f), null, 2, 4));
            }
        }
        color = Color.red;
        this.room.ScreenMovement(new Vector2?(base.firstChunk.pos), this.throwDir.ToVector2() * 1.5f, 0f);
        this.room.PlaySound(SoundID.Rock_Hit_Wall, base.firstChunk);
        this.SetRandomSpin();
        this.ChangeMode(Weapon.Mode.Free);
        Broke();
    }

    public override void HitByWeapon(Weapon weapon)
    {
        base.HitByWeapon(weapon);
        color = Color.yellow;

        if (weapon is Spear)
        {
            color = Color.magenta;
            Broke();
        }
    }

    private void Broke()
    {
        // Obtener la velocidad actual del coco para usar como velocidad de impacto
        Vector2 currentVelocity = base.firstChunk.vel;
        
        if (this.room.BeingViewed)
        {
            for (int i = 0; i < spark_iteration; i++)
            {
                this.room.AddObject(new Spark(base.firstChunk.pos + Custom.DegToVec(UnityEngine.Random.value * 360f) * UnityEngine.Random.value * 10f, Custom.DegToVec(UnityEngine.Random.value * 360f) * 10f * UnityEngine.Random.value, new Color(1f, 1f, 1f), null, 2, 4));
            }
            
            // Agregar efecto de agua derramándose con velocidad de impacto
            for (int i = 0; i < 3; i++)
            {
                Vector2 spillDirection = Custom.DegToVec(UnityEngine.Random.value * 180f - 90f); // Hacia abajo principalmente
                this.room.AddObject(new CocoWater(base.firstChunk.pos, currentVelocity));
            }
        }
        
        // Sonido de líquido derramándose
        this.room.PlaySound(SoundID.Slugcat_Eat_Dangle_Fruit, base.firstChunk, false, 0.8f, 0.8f);
        
        // MAKE THE SEED OBJECT HERE
        UnityEngine.Debug.Log("ShoreCoco Broke");

        AbstractPhysicalObject abstractPhysicalObject = new AbstractPhysicalObject(this.room.world, AbstractPhysicalObject.AbstractObjectType.Rock, null, room.GetWorldCoordinate(this.firstChunk.pos), this.room.game.GetNewID());
        Rock slimeMold = new Rock(abstractPhysicalObject, this.room.world);
        slimeMold.PlaceInRoom(this.room);
        UnityEngine.Debug.Log("ShoreCoco Broke water release");
        visible = false;
        this.Destroy();
    }
    public bool visible = true;
}
