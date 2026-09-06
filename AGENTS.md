# Working on Army Arrow Counter

A Mount & Blade II: Bannerlord mod that counts your army's remaining
ammunition and draws it in the corner of the screen. Roughly 20,000 Nexus
downloads, one maintainer, no team.

The job this repo is set up for is **shipping a compatibility update when the
game updates**, with as much decided by machine as possible and the human
called in only where nothing else can decide.

## Shipping a compatibility update

1. Find the new game version:
   `curl -s https://api.nuget.org/v3-flatcontainer/bannerlord.referenceassemblies.core/index.json`
   BUTR publish one package per game release, usually within days.
2. Set `BannerlordVersion` in `Directory.Build.props`. That is the only line
   tying the repo to a game release.
3. `dotnet build src/ArmyArrowCounter -c Release`
4. Fix what broke. **Read §Preconditions before writing the fix.**
5. `dotnet test test/ArmyArrowCounter.Tests`
6. On Windows with the game installed:
   `dotnet test test/ArmyArrowCounter.GameTests`
7. Bump `<Version>` in `src/ArmyArrowCounter/ArmyArrowCounter.csproj`.
8. Hand to the maintainer for a battle. **Do not skip to 9.**
9. `git tag vX.Y.Z && git push origin vX.Y.Z`, then run the two publish
   workflows from the Actions tab.

Steps 1-7 are yours. Step 8 is not, and no amount of green changes that.

## Preconditions: the rule that matters most

**For every game method your diff newly calls, or calls differently,
decompile its body and write down what it assumes about its arguments.**

A signature carries no preconditions. `Bannerlord.ReferenceAssemblies` is
metadata only - real signatures, stubbed bodies - so a clean compile against
the exact game version proves the symbol exists and nothing else.

This is not hypothetical. v1.8.0 shipped a call to
`MissionEquipment.GetMaxAmmo(EquipmentIndex)` that compiled cleanly, passed 74
tests, and crashed the game on entry to every battle. The method dereferences
the slot you name without checking it holds anything. Thirty seconds of
decompilation would have caught it.

```csharp
// ICSharpCode.Decompiler 8.2, against the installed game
var decompiler = new CSharpDecompiler(
    @"...\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.dll", new DecompilerSettings());
Console.WriteLine(decompiler.DecompileTypeAsString(
    new FullTypeName("TaleWorlds.MountAndBlade.MissionEquipment")));
```

Two corollaries:

- **A guard is part of your diff when you change what it protects.** The
  broken empty-slot guard behind the v1.8.0 crash was years old and harmless
  until a new call appeared underneath it.
- **Distrust any comparison against a sentinel like `X.Invalid` or `X.None`.**
  Most TaleWorlds structs do not override `Equals`, so `ValueType.Equals`
  compares reference fields by identity. `MissionWeapon` holds a `List<>` that
  every constructor allocates fresh, so `weapon.Equals(MissionWeapon.Invalid)`
  is never true. Use the type's own predicate - `IsEmpty` here.

## What the tests prove, and what they don't

| Tier | Where | Runs on | Covers |
|---|---|---|---|
| Unit | `test/ArmyArrowCounter.Tests` | anywhere | `src/ArmyArrowCounter/core/` only |
| Game assumption | `test/ArmyArrowCounter.GameTests` | Windows + game | claims about real game types |
| Battle | a human | Windows + game | everything else |

`core/` is free of TaleWorlds references by construction: the test project
globs the folder and compiles it, so adding a `using TaleWorlds.*` there breaks
the test build immediately. Keep that property.

**A green unit run says nothing about a compatibility update.** Everything a
game update can break lives outside `core/`. If your diff touches
`ArrowCounter`, `AmmoCount`, `AacMissionBehavior` or `AacUiApplier`, the unit
tests did not read it.

`GameTests` loads the installed game's assemblies in place through an
`AssemblyResolve` hook, with no engine, window or save, in milliseconds. It
compiles anywhere and skips where the game is absent. Set `BANNERLORD_BIN` to
point it at a non-standard install. **When you fix a bug that a game-type
assumption caused, add the assumption here as an assertion.**

Reachable there: `MissionWeapon`, `MissionEquipment` and its indexer, and
anything taking them. Out of reach: `Agent`, `Mission`, `ScreenManager`,
`GauntletLayer`, mission-mode transitions, prefab and brush loading - all
native-backed or engine-scheduled. Prefer signatures over `MissionEquipment`
to `Agent` so arithmetic stays in reach.

## Finding where an API went

Diff the two versions' metadata rather than guessing. `MetadataLoadContext`
over `ref/net472` in the two NuGet packages, with the net472 framework
references plus their `Facades` directory in the resolver path, then dump
members matching a name fragment. This is how `MissionBehavior.OnItemPickup`
was found to have become the `Mission.OnItemPickUp` event, capital U included.

## Publishing

Tagging publishes to GitHub Releases only. Nexus and Steam are
`workflow_dispatch` and stay that way. They need secrets - `NEXUSMODS_APIKEY`,
`STEAM_LOGIN`, `STEAM_PASSWORD`, `STEAM_AUTH_CODE` - and a battle behind them.

The comment at the top of both publish workflows says to run them once the
counter has been seen rendering in a battle. For v1.8.0 that instruction was
correct and was not followed.

## When something ships broken anyway

Crash dumps land in `C:\ProgramData\Mount and Blade II Bannerlord\crashes\`.
A watchdog `0xC0000005` with `Parameter-1` a small offset is a managed
`NullReferenceException`, not a native engine fault - don't go hunting in the
engine. ClrMD 3.1 (`Microsoft.Diagnostics.Runtime`, net8.0) reads the managed
stack straight out of `dump.dmp`:

```csharp
using var dt = DataTarget.LoadDump(dumpPath);
var runtime = dt.ClrVersions[0].CreateRuntime();
foreach (var thread in runtime.Threads)
    if (thread.CurrentException != null) { /* Type.Name, Message, StackTrace */ }
```

`crash_tags.txt` in the same folder gives the game build and which modules were
actually active, which settles "is this us" in seconds.

## Repo conventions

- Two versions, do not confuse them. `BannerlordVersion` in
  `Directory.Build.props` is the game. `<Version>` in the mod csproj is the
  mod, is written down exactly once, and is stamped into `SubModule.xml` at
  build time and read back off the assembly at startup.
- A build assembles the installable module into `build/ArmyArrowCounter`. That
  folder is the release artifact; never hand-assemble one.
- One binary serves Nexus and Steam. The config file is found relative to the
  mod's own assembly, so it works from either install location. Do not
  reintroduce a build-time flag to tell them apart.
- Commit messages: what and why, no diff inventory, 72 columns, no tool
  attribution or `Co-Authored-By` trailers.
- `CLAUDE.md` is a gitignored symlink to this file. Edit this one.
