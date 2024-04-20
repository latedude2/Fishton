[System.Flags]
public enum FishEncounterState
{
    None = 0,

    // The bob is being thrown into the water
    Throwing    = 0b_0000_0001,

    // The bob is in the water without a hooked fish
    Idle        = 0b_0000_0010,

    // A fish has been caught on the hook
    Hooked      = 0b_0000_0100,

    // The player has successfully caught the fish
    Caught      = 0b_0000_1000,

    // The player failed to catch the fish
    Failed      = 0b_0001_0000,

    Finished = Caught | Failed
}
