namespace VionheartSweetroll.Actions;

public class AForesightDraw : ACardSelect
{
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
		CardBrowse cardBrowse = new CardBrowse
		{
			mode = CardBrowse.Mode.Browse,
			browseSource = CardBrowse.Source.DrawPile,
			browseAction = new ChooseCardToPutInHand
            {
            },
			filterUnremovableAtShops = filterUnremovableAtShops,
			filterExhaust = filterExhaust,
			filterRetain = filterRetain,
			filterBuoyant = filterBuoyant,
			filterTemporary = filterTemporary,
			includeTemporaryCards = includeTemporaryCards,
			filterOutTheseRarities = filterOutTheseRarities,
			filterMinCost = filterMinCost,
			filterMaxCost = filterMaxCost,
			filterUpgrade = filterUpgrade,
			filterAvailableUpgrade = filterAvailableUpgrade,
			filterUUID = filterUUID,
			ignoreCardType = ignoreCardType,
			allowCancel = allowCancel,
			allowCloseOverride = allowCloseOverride
		};
		if (cardBrowse.GetCardList(g).Count == 0)
		{
			timer = 0.0;
			return null;
		}
		return cardBrowse;
	}
}