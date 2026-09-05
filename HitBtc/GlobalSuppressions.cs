using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
    Justification = "PublicEnum is retained for source and binary compatibility.",
    Scope = "type", Target = "~T:Hitbtc.PublicEnum")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
    Justification = "Error is retained for source and binary compatibility.",
    Scope = "type", Target = "~T:Hitbtc.HitBtcModel.Error")]
