namespace VionheartSweetroll.Actions;

public class AShuffleDiscardToDrawPile : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach (Card item in c.discard)
        {
            s.deck.Insert(s.rngShuffle.NextInt() % (s.deck.Count + 1), item);
            item.targetPos = Combat.deckPos + new Vec(2.0);
        }
        c.discard.Clear();
    }
}