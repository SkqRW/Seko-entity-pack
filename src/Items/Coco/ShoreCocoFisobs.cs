namespace Items.ShoreCoco;

public class ShoreCocoFisob : Fisob
{

    public ShoreCocoFisob() : base(Enums.AbstractObjectType.ShoreCoco)
    {
        SandboxPerformanceCost = new(linear: 0.2f, 0f);
        RegisterUnlock(Enums.SandboxUnlockID.ShoreCoco, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat, data: 0);
    }

    public override AbstractPhysicalObject Parse(World world, EntitySaveData entitySaveData, SandboxUnlock unlock)
    {
        // ShoreCoco data is floats separated by ; characters.
        string[] p = entitySaveData.CustomData.Split(';');

        if (p.Length < 3)
        {
            p = new string[3];
        }

        var result = new ShoreCocoAbstr(world, entitySaveData.Pos, entitySaveData.ID)
        {
            saturation = float.TryParse(p[0], out var s) ? s : 0.5f,
            scaleX = float.TryParse(p[1], out var x) ? x : 1f,
            scaleY = float.TryParse(p[2], out var y) ? y : 1f
        };

        // If this is coming from a sandbox unlock, apply any special logic based on unlock data
        if (unlock is SandboxUnlock u)
        {
            // You can add custom logic here based on u.Data if needed
            // For example: result.saturation = u.Data / 100f;
        }

        return result;
    }

    private static readonly ShoreCocoProperties properties = new();

    public override ItemProperties Properties(PhysicalObject forObject)
    {
        return properties;
    }
}

public class Enums
{
    public class AbstractObjectType
    {
        public static AbstractPhysicalObject.AbstractObjectType ShoreCoco = new(nameof(ShoreCoco), true);
        public void UnregisterValues()
        {
            if (ShoreCoco != null)
            {
                ShoreCoco.Unregister();
                ShoreCoco = null;
            }
        }
    }

    public class SandboxUnlockID
    {
        public static MultiplayerUnlocks.SandboxUnlockID ShoreCoco = new(nameof(ShoreCoco), true);

        public void UnregisterValues()
        {
            if (ShoreCoco != null)
            {
                ShoreCoco.Unregister();
                ShoreCoco = null;
            }
        }
    }
}

