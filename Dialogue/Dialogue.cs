using System;
using Microsoft.Extensions.Logging;
using Nickel;

namespace VionheartSweetroll.Dialogue;
internal static class Dialogue
{
	internal static void Inject()
	{
		StoryDialogue.Inject();
		CombatDialogue.Inject();
		CardDialogue.Inject();
		ArtifactDialogue.Inject();
		
	}

	public static void ApplyInjections()
	{
		try
		{
			if (!VionheartSweetroll.Instance.modDialogueInited)
			{
				VionheartSweetroll.Instance.modDialogueInited = true;
			}
		}
		catch (Exception exception)
		{
			VionheartSweetroll.Instance.Logger.LogError(exception, "Failed to inject dialogue for modded stuff");
		}
	}
}
