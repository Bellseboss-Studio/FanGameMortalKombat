using System.Runtime.CompilerServices;

// Testability seam: the EditMode and PlayMode test assemblies need access to
// internal seams (TargetFocus.AddEnemy, InputPlayerV2.GetDirectionFromVector,
// MovementRigidbodyV2.CalculateDirection) so module logic is covered without
// scenes (spec V3). Player builds exclude test assemblies entirely.
[assembly: InternalsVisibleTo("MortalKombat.Player.Tests")]
[assembly: InternalsVisibleTo("MortalKombat.Player.PlayModeTests")]