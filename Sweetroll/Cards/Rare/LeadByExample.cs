using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace VionheartSweetroll.Cards;

public class LeadByExample : Card, IRegisterable
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
            Name = VionheartSweetroll.Instance.AnyLocalizations.Bind(["card", "LeadByExample", "name"]).Localize, //Card's name, localized.
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
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "LeadByExample", "description"]),
                cost = 3
            },
            Upgrade.A => new CardData
            {
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "LeadByExample", "description"]),
                cost = 2
            },
            Upgrade.B => new CardData
            {
                description = VionheartSweetroll.Instance.Localizations.Localize(["card", "LeadByExample", "description"]),
                cost = 3,
                retain = true
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
                new AAttack
                {
                    damage = GetDmg(s, 3)
                },
                new ACardSelect
                {
                    browseSource = CardBrowse.Source.Hand,
                    browseAction = new ChooseCardInYourHandToPlayForFree
                    {
                    }
                }
            ],
            Upgrade.A =>
            [
                new AAttack
                {
                    damage = GetDmg(s, 3)
                },
                new ACardSelect
                {
                    browseSource = CardBrowse.Source.Hand,
                    browseAction = new ChooseCardInYourHandToPlayForFree
                    {
                    }
                }
            ],
            Upgrade.B =>
            [
                new AAttack
                {
                    damage = GetDmg(s, 3)
                },
                new ACardSelect
                {
                    browseSource = CardBrowse.Source.Hand,
                    browseAction = new ChooseCardInYourHandToPlayForFree
                    {
                    }
                }
            ],
            _ => []
        };
    }
}