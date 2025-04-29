using Nanoray.PluginManager;
using Nickel;

namespace VionheartSweetroll;

internal interface IRegisterable
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}