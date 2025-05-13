using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using VionheartSweetroll.ExternalAPI;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FSPRO;
using VionheartSweetroll.Actions;

namespace VionheartSweetroll.Features;

public class AkashicDownloadManager : IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public AkashicDownloadManager(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        VionheartSweetroll.Instance.KokoroApi.StatusRendering.RegisterHook(this);
		VionheartSweetroll.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) =>
        {
			var statusValue = state.ship.Get(VionheartSweetroll.Instance.AkashicDownload.Status);
			state.ship.Add(VionheartSweetroll.Instance.ForesightDraw.Status, statusValue);
		}
		);
    }
}
