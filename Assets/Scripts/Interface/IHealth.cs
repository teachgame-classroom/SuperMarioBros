using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    int maxHp { get; set; }
    int currentHp { get; }

    void ChangeHp(int amount);

    void Hit(int amount);

    void Heal(int amount);

    void Die();
}
