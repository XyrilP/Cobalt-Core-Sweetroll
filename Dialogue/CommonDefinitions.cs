using System.Linq;
using VionheartSweetroll;
using Nickel;

namespace VionheartSweetroll.Dialogue;

internal static class CommonDefinitions
{
	internal static VionheartSweetroll Instance => VionheartSweetroll.Instance;
	internal static string Eunice => Deck.eunice.Key();
	internal static Deck Eunice_Deck => Deck.eunice;
	internal static string Riggs => Deck.riggs.Key();
	internal static string Cat => "comp";
	internal static string EvilRiggs => "pirateBoss";
	internal static string Brimford => "walrus";
}