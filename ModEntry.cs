using Nickel;
using Nickel.Common;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using VionheartSweetroll.ExternalAPI;
using VionheartSweetroll.Cards;
using VionheartSweetroll.Dialogue;
using VionheartSweetroll.Artifacts;



namespace VionheartSweetroll;
internal class VionheartSweetroll : SimpleMod
{
    /* Declare stuff! */

    internal static VionheartSweetroll Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; private set; } = null;
    internal IDuoArtifactsApi? DuoArtifactsApi { get; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    public bool modDialogueInited;
    internal string UniqueName { get; private set; }
    public LocalDB localDB { get; set; } = null!;

    internal IDeckEntry Sweetroll_Deck;

    /* VionheartSweetroll Content */
    private static List<Type> Colorless_Common_Card_Types = [
        /* Common cards. */
    ];
    private static List<Type> Colorless_Uncommon_Card_Types = [
        /* Uncommon cards. */
    ];
    private static List<Type> Colorless_Rare_Card_Types = [
        /* Rare cards. */
    ];
    private static List<Type> Colorless_Special_Card_Types = [
        /* Special cards. */
    ];
    private static IEnumerable<Type> Colorless_All_Card_Types =
        Colorless_Common_Card_Types
            .Concat(Colorless_Uncommon_Card_Types)
            .Concat(Colorless_Rare_Card_Types)
            .Concat(Colorless_Special_Card_Types);
    private static List<Type> Colorless_Common_Artifact_Types = [
        /* Common artifacts. */
    ];
    private static List<Type> Colorless_Boss_Artifact_Types = [
        /* Boss artifacts. */
    ];
    private static List<Type> Colorless_Event_Artifact_Types = [
        /* Event artifacts. */
    ];
    private static IEnumerable<Type> Colorless_All_Artifact_Types =
        Colorless_Common_Artifact_Types
            .Concat(Colorless_Boss_Artifact_Types)
            .Concat(Colorless_Event_Artifact_Types);
    private static List<Type> Ship_Artifact_Types = [
        /* Ship artifacts */
    ];
    internal static IReadOnlyList<Type> Event_Types { get; } = [
        /* Events */
    ];
    private static IEnumerable<Type> Vionheart_Content =
        Colorless_All_Card_Types
            .Concat(Colorless_All_Artifact_Types)
            .Concat(Ship_Artifact_Types)
            .Concat(Event_Types);
    /* VionheartSweetroll Content */
    /* Sweetroll Content */
    private static List<Type> Sweetroll_Common_Card_Types = [
        /* Sweetroll's common cards. */
        typeof(Directive),
        typeof(VulcanShotgun)
    ];
    private static List<Type> Sweetroll_Uncommon_Card_Types = [
        /* Sweetroll's uncommon cards. */
    ];
    private static List<Type> Sweetroll_Rare_Card_Types = [
        /* Sweetroll's rare cards. */
    ];
    private static List<Type> Sweetroll_Special_Card_Types = [
        /* Sweetroll's special cards. */
        typeof(ReloadVulcanShotgun)
    ];
        /* Concat all Sweetroll cards. */
    private static IEnumerable<Type> Sweetroll_All_Card_Types = 
        Sweetroll_Common_Card_Types
            .Concat(Sweetroll_Uncommon_Card_Types)
            .Concat(Sweetroll_Rare_Card_Types)
            .Concat(Sweetroll_Special_Card_Types);
    private static List<Type> Sweetroll_Common_Artifact_Types = [
        /* Sweetroll's common artifacts. */
        typeof(Foresight)
    ];
    private static List<Type> Sweetroll_Boss_Artifact_Types = [
        /* Sweetroll's boss artifacts. */
    ];
        /* Concat all Sweetroll artifacts. */
    private static IEnumerable<Type> Sweetroll_All_Artifact_Types =
        Sweetroll_Common_Artifact_Types
            .Concat(Sweetroll_Boss_Artifact_Types);
    private static IEnumerable<Type> Sweetroll_Content =
        Sweetroll_All_Card_Types
            .Concat(Sweetroll_All_Artifact_Types);
    /* Sweetroll Content */
    /* Concat everything for registration. */
    private static IEnumerable<Type> AllRegisterableTypes =
        Vionheart_Content
            .Concat(Sweetroll_Content);
    public VionheartSweetroll(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {

        Instance = this;
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2; //Updated to V2!
        Harmony = new Harmony("VionheartSweetroll"); //New API? (Harmony)
        MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties", (SemanticVersion?)null);
        DuoArtifactsApi = helper.ModRegistry.GetApi<IDuoArtifactsApi>("Shockah.DuoArtifacts");
        modDialogueInited = false;
        UniqueName = package.Manifest.UniqueName;
        /* Urufudoggo's new Dialogue Machine */
        helper.Events.OnModLoadPhaseFinished += (_, phase) =>
        {
            if (phase == ModLoadPhase.AfterDbInit)
            {
                localDB = new(helper, package);
            }
        };
        helper.Events.OnLoadStringsForLocale += (_, thing) =>
        {
            foreach (KeyValuePair<string, string> entry in localDB.GetLocalizationResults())
            {
                thing.Localizations[entry.Key] = entry.Value;
            }
        };
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        /* Urufudoggo's new Dialogue Machine */
        /* Sweetroll Content */
        Sweetroll_Deck = helper.Content.Decks.RegisterDeck("SweetrollDeck", new DeckConfiguration
        {
            Definition = new DeckDef
            {
                color = new Color("5F00BC"), //old color: 560319
                titleColor = new Color("000000")
            },
            DefaultCardArt = RegisterSprite(package, "assets/cards/cardbg_blank.png").Sprite,
            BorderSprite = RegisterSprite(package, "assets/cards/border_sweetroll.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "Sweetroll", "name"]).Localize
        }
        );
            /* Sweetroll Sprites */
                /* Sweetroll NEUTRAL */
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Sweetroll_Deck.Deck.Key(),
            LoopTag = "neutral",
            Frames = [
                RegisterSprite(package, "assets/characters/sweetroll_neutral_0.png").Sprite,
            ]
        }
        );

                /* Sweetroll MINI */
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Sweetroll_Deck.Deck.Key(),
            LoopTag = "mini",
            Frames = [
                RegisterSprite(package, "assets/characters/sweetroll_mini_0.png").Sprite,
            ]
        }
        );
            /* Register Sweetroll as a Playable Character plus his Deck */
        helper.Content.Characters.V2.RegisterPlayableCharacter("Sweetroll", new PlayableCharacterConfigurationV2
        {
            Deck = Sweetroll_Deck.Deck,
            BorderSprite = RegisterSprite(package, "assets/characters/char_sweetroll.png").Sprite,
            Starters = new StarterDeck
            {
                cards = [
                    new Directive(),
                    new VulcanShotgun()
                ],
                artifacts = [
                    new Foresight()
                ]
            },
            Description = AnyLocalizations.Bind(["character", "Sweetroll", "description"]).Localize
        }
        );
        /* Register all artifacts and cards into the game, allowing it to be played. (Based on AllRegisterableTypes) */
        foreach (var type in AllRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
        /* Inject Dialogue */
        Dialogue.Dialogue.Inject();
    }

    /* New function to register sprites better (New method). */
    public static ISpriteEntry RegisterSprite(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile(dir));
    }
    /* New function to register animations better (New method). */
    public static void RegisterAnimation(IPluginPackage<IModManifest> package, string tag, string dir, int frames)
    {
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Instance.Sweetroll_Deck.Deck.Key(),
            LoopTag = tag,
            Frames = Enumerable.Range(0, frames)
                .Select(i => RegisterSprite(package, dir + i + ".png").Sprite)
                .ToImmutableList()
        });
    }


}