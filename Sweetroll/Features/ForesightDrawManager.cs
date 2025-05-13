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

public class ForesightDrawManager : IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    public ForesightDrawManager(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        VionheartSweetroll.Instance.KokoroApi.StatusRendering.RegisterHook(this);
        VionheartSweetroll.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.DrawCards)),
            prefix: new HarmonyMethod(GetType(), nameof(Combat_DrawCards_Prefix))
        );
    }
    public static bool Combat_DrawCards_Prefix(Combat __instance, State s, int count)
    {
		bool flag = false;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			/* Patched in */
			var statusValue = s.ship.Get(VionheartSweetroll.Instance.ForesightDraw.Status);
			/* Patched in */
			if (__instance.hand.Count >= 10)
			{
				__instance.PulseFullHandWarning();
				break;
			}
			if (s.deck.Count == 0 || s.deck.Count < statusValue)
			{
				foreach (Card item in __instance.discard)
				{
					s.SendCardToDeck(item, doAnimation: true);
				}
				__instance.discard.Clear();
				s.ShuffleDeck(isMidCombat: true);
				if (s.deck.Count() == 0)
				{
					break;
				}
			}
			if (!flag)
			{
				Audio.Play(Event.CardHandling);
				flag = true;
			}
			/* __instance.DrawCardIdx(s, s.deck.Count - 1).waitBeforeMoving = (double)i * 0.09; */ //Patched out
            /* Patched in "Foresight Draw" */
            if (statusValue > 0)
            {
				__instance.QueueImmediate(
				[
					new AForesightDraw
					{
					}
				]
				);
                s.ship.Add(VionheartSweetroll.Instance.ForesightDraw.Status, -1);
            }
            else __instance.DrawCardIdx(s, s.deck.Count - 1).waitBeforeMoving = (double)i * 0.09;
            /* Patched in "Foresight Draw" */
			num++;
		}
		foreach (Artifact item2 in s.EnumerateAllArtifacts())
		{
			item2.OnDrawCard(s, __instance, num);
	    }
        return false;
	}
}
