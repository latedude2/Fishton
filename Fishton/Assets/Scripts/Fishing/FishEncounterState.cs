public enum FishEncounterState
{
    None = 0,

    // The bob is in the water without a hooked fish
    Idle = 1,

    // A fish has been caught on the hook
    Hooked = 2,

    // The player has successfully caught the fish
    Caught = 3,

    // The player failed to catch the fish
    Failed = 4
}
