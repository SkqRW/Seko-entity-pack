using System.Collections.Generic;
using Fisobs.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Butterflies;

public class Main
{
    public static void Remove()
    {
    }
    public static void Init()
    {
    }
    
}


public class Enums
{
    public class CreatureTemplateType
    {
        public static CreatureTemplate.Type Butterflies = new(nameof(Butterflies), true);
        public void UnregisterValues()
        {
            if (Butterflies != null)
            {
                Butterflies.Unregister();
                Butterflies = null;
            }
        }
    }

    public class SandboxUnlockID
    {
        public static MultiplayerUnlocks.SandboxUnlockID Butterflies = new(nameof(Butterflies), true);

        public void UnregisterValues()
        {
            if (Butterflies != null)
            {
                Butterflies.Unregister();
                Butterflies = null;
            }
        }
    }
}
