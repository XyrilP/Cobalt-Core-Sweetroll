using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VionheartSweetroll;
using Microsoft.Extensions.Logging;
using Nickel;
using VionheartSweetroll.Artifacts;
using static VionheartSweetroll.Dialogue.CommonDefinitions;

namespace VionheartSweetroll.Dialogue;

internal static class ArtifactDialogue
{
    internal static VionheartSweetroll Instance => VionheartSweetroll.Instance;
    internal static string F(this string Name)
    {
        return $"{Instance.UniqueName}::{Name}";
    }
    internal static void Inject()
    {
    }
}