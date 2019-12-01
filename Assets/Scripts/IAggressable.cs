using UnityEngine;

public interface IAggressable
{
    bool HasAttackedThisTurn { get; set; }
    Vector2 AttackTarget { get; set; }

    void Aim(float x, float y);
    void Fire();
    void ResetAttackTurn();
}