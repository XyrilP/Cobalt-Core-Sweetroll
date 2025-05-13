using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace VionheartSweetroll.Cards;

public class Recall : Card, IRegisterable
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
                rarity = Rarity.common, //What rarity should this card be?
                dontOffer = false, //Should this card be offered to the player?
                upgradesTo = [Upgrade.A, Upgrade.B] //Does this card upgrade? and if it has an A or B upgrade.
            },
            Name = VionheartSweetroll.Instance.AnyLocalizations.Bind(["card", "Recall", "name"]).Localize, //Card's name, localized.
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
                cost = 1
            },
            Upgrade.A => new CardData
            {
                cost = 1
            },
            Upgrade.B => new CardData
            {
                cost = 1,
                infinite = true
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
                new ADrawCard
                {
                    count = 1
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
                new ADrawCard
                {
                    count = 3
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
                new ADrawCard
                {
                    count = 1
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