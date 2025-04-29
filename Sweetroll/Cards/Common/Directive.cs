using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace VionheartSweetroll.Cards;

public class Directive : Card, IRegisterable
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
            Name = VionheartSweetroll.Instance.AnyLocalizations.Bind(["card", "Directive", "name"]).Localize, //Card's name, localized.
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
                cost = 0
            },
            Upgrade.B => new CardData
            {
                cost = 1
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
                new ACardSelect
                {
                    browseAction = new ChooseCardToPutInHand
                    {
                    },
                    browseSource = CardBrowse.Source.DrawPile,
                    filterUUID = uuid
                }
            ],
            Upgrade.A =>
            [
                new ACardSelect
                {
                    browseAction = new ChooseCardToPutInHand
                    {
                    },
                    browseSource = CardBrowse.Source.DrawPile,
                    filterUUID = uuid
                }
            ],
            Upgrade.B =>
            [
                new ACardSelect
                {
                    browseAction = new ChooseCardToPutInHand
                    {
                    },
                    browseSource = CardBrowse.Source.DrawOrDiscardPile,
                    filterUUID = uuid
                }
            ],
            _ => []
        };
    }
}