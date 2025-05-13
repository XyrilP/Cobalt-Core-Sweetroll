namespace VionheartSweetroll.Actions;

public class ChooseCardToPutInDrawPile : CardAction
{
    public override void Begin(G g, State s, Combat c)
	{
		Card? card = selectedCard;
		if (card != null)
		{
			s.RemoveCardFromWhereverItIs(card.uuid);
			s.SendCardToDeck(card, false, true);
		}
	}
    public override string? GetCardSelectText(State s)
	{
		return "Pick a card. Put it in your draw pile.";
	}
}