namespace VionheartSweetroll.Actions;

public class AShuffleExhaustToDiscardPile : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach (Card item in c.exhausted)
        {
            c.discard.Insert(s.rngShuffle.NextInt() % (c.discard.Count + 1), item);
            item.targetPos = Combat.discardPos + new Vec(2.0);
        }
        c.exhausted.Clear();
    }
}