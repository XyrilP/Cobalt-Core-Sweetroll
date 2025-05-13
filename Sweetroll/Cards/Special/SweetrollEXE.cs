using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VionheartSweetroll.Cards;

public class SweetrollEXE : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                rarity = Rarity.common,
                dontOffer = false,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = VionheartSweetroll.Instance.AnyLocalizations.Bind(["card", "SweetrollEXE", "name"]).Localize
        }
        );
    }
    public override CardData GetData(State state)
    {
        return upgrade switch
        {
            Upgrade.None => new CardData
            {
                cost = 1,
                exhaust = true,
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "SweetrollEXE", "description"]),
                artTint = "5F00BC"
            },
            Upgrade.A => new CardData
            {
                cost = 0,
                exhaust = true,
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "SweetrollEXE", "descA"]),
                artTint = "5F00BC"
            },
            Upgrade.B => new CardData
            {
                cost = 1,
                exhaust = true,
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "SweetrollEXE", "descB"]),
                artTint = "5F00BC"
            },
            _ => new CardData{}
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return upgrade switch
        {
            Upgrade.None =>
            [
                new ACardOffering
                {
                    amount = 2,
                    limitDeck = VionheartSweetroll.Instance.Sweetroll_Deck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                    dialogueSelector = ".summonSweetroll"
                }
            ],
            Upgrade.A =>
            [
                new ACardOffering
                {
                    amount = 2,
                    limitDeck = VionheartSweetroll.Instance.Sweetroll_Deck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                    dialogueSelector = ".summonSweetroll"
                }
            ],
            Upgrade.B =>
            [
                new ACardOffering
                {
                    amount = 3,
                    limitDeck = VionheartSweetroll.Instance.Sweetroll_Deck.Deck,
                    makeAllCardsTemporary = true,
                    overrideUpgradeChances = false,
                    canSkip = false,
                    inCombat = true,
                    discount = -1,
                    dialogueSelector = ".summonSweetroll"
                }
            ],
            _ => []
        };
    }
}