public enum FishEncounterState
{
    None = 0,

    // The bob is being thrown into the water
    Throwing,

    // The bob is in the water without a hooked fish
    Idle,

    // A fish has been caught on the hook
    Hooked,

    // The player has successfully caught the fish
    Caught,

    // The player failed to catch the fish
    Failed
}
