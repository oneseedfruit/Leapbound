public interface ITurn
{
    bool IsTurn { get; }
    void EndTurn();
    void ResetTurn();
}