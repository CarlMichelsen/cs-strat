namespace Domain.Lobby;

/// <summary>
/// Grenade throw method.
/// </summary>
public enum ThrowMethod
{
    /// <summary>
    /// Throw the grenade normally.
    /// </summary>
    Throw,

    /// <summary>
    /// Run and throw.
    /// </summary>
    RunThrow,

    /// <summary>
    /// Jump and throw at the same time.
    /// </summary>
    JumpThrow,

    /// <summary>
    /// Run, then jump and throw at the same time.
    /// </summary>
    RunJumpThrow,
}
