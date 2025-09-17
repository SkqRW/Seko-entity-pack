namespace Creatures.LunarLizard;

public class Enums
{

    public class CreatureTemplateType
    {
        public static CreatureTemplate.Type LunarLizard = new (nameof(LunarLizard), true);
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
