using UnityEngine;

public interface IShootable
{
    GameObject Projectile { get; set; }
    int ProjectileCount { get; set; }
    bool HasAttackedThisTurn { get; set; }
    Vector2 AttackTarget { get; set; }

    void Aim(float x, float y);
    void Fire();
    void ResetAttackTurn();
}