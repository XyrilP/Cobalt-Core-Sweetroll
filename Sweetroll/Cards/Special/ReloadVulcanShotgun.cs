using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace VionheartSweetroll.Cards;

public class ReloadVulcanShotgun : Card, IRegisterable
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
                dontOffer = true, //Should this card be offered to the player?
                upgradesTo = [Upgrade.A, Upgrade.B] //Does this card upgrade? and if it has an A or B upgrade.
            },
            Name = VionheartSweetroll.Instance.AnyLocalizations.Bind(["card", "ReloadVulcanShotgun", "name"]).Localize, //Card's name, localized.
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
                cost = 2,
                singleUse = true
            },
            Upgrade.A => new CardData
            {
                cost = 1,
                singleUse = true
            },
            Upgrade.B => new CardData
            {
                cost = 2,
                singleUse = true
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
                    card = new VulcanShotgun
                    {
                    },
                    destination = CardDestination.Hand
                }
            ],
            Upgrade.A =>
            [
                new AAddCard
                {
                    card = new VulcanShotgun
                    {
                        upgrade = Upgrade.A
                    },
                    destination = CardDestination.Hand
                }
            ],
            Upgrade.B =>
            [
                new AAddCard
                {
                    card = new VulcanShotgun
                    {
                        upgrade = Upgrade.B
                    },
                    destination = CardDestination.Hand
                }
            ],
            _ => []
        };
    }
}