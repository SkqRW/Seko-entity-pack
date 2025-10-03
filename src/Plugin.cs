using BepInEx;
using BepInEx.Logging;
using System.Security.Permissions;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace TestMod;

[BepInPlugin("seko_entity_pack", "Seko Mini Entity Pack", "0.0.1"), BepInDependency("io.github.dual.fisobs")]
sealed class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    bool IsInit;

    public void OnEnable()
    {
        Logger = base.Logger;
        On.RainWorld.OnModsInit += OnModsInit;
        RegisterFisobs();
    }

    private void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        if (IsInit) return;
        IsInit = true;

        // Initialize assets, your mod config, and anything that uses RainWorld here
        Logger.LogDebug("Hello world!");

        Creatures.LunarLizard.Main.Init();

    }

    private void RegisterFisobs()
    {
        Content.Register(new Creatures.LunarLizard.LunarLizardCritob());

        Content.Register(new Items.ShoreCoco.ShoreCocoFisob());
    }
}
