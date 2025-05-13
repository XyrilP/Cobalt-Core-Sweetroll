using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace VionheartSweetroll.Cards;

public class Hypercognition : Card, IRegisterable
{
    private static ISpriteEntry? BaseArt { get; set; }
    private static ISpriteEntry? FlippedArt1 { get; set; }
    private static ISpriteEntry? FlippedArt2 { get; set; }
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        BaseArt = null; //Art used.
        FlippedArt1 = null; //Art used when card is flipped or flopped.
        FlippedArt2 = null;
        helper.Content.Cards.RegisterCard(new CardConfiguration
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new CardMeta
            {
                deck = VionheartSweetroll.Instance.Sweetroll_Deck.Deck, //Which deck should this card go to?
                rarity = Rarity.rare, //What rarity should this card be?
                dontOffer = false, //Should this card be offered to the player?
                upgradesTo = [Upgrade.A, Upgrade.B] //Does this card upgrade? and if it has an A or B upgrade.
            },
            Name = VionheartSweetroll.Instance.AnyLocalizations.Bind(["card", "Hypercognition", "name"]).Localize, //Card's name, localized.
            Art = BaseArt?.Sprite //Card art
        }
        );
    }
    public override CardData GetData(State state)
    {
        /*
        Add this for flipped art 
        art = !flipped ? FlippedArt1?.Sprite : FlippedArt2?.Sprite
        */
        return upgrade switch
        {
            Upgrade.None => new CardData
            {
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "Hypercognition", "description"]),
                cost = 1,
                exhaust = true
            },
            Upgrade.A => new CardData
            {
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "Hypercognition", "descA"]),
                cost = 0,
                exhaust = true
            },
            Upgrade.B => new CardData
            {
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "Hypercognition", "descB"]),
                cost = 2,
                exhaust = true
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
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AStatus
                {
                    status = VionheartSweetroll.Instance.ForesightDraw.Status,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ],
            Upgrade.A =>
            [
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AStatus
                {
                    status = VionheartSweetroll.Instance.ForesightDraw.Status,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ],
            Upgrade.B =>
            [
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AAddCard
                {
                    card = new Recall
                    {
                        discount = -1,
                        temporaryOverride = true,
                        singleUseOverride = true
                    },
                    destination = CardDestination.Deck
                },
                new AStatus
                {
                    status = VionheartSweetroll.Instance.ForesightDraw.Status,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ],
            _ => []
        };
    }
}