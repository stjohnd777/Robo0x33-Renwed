using UnityEngine;
using System.Collections;

public interface IHealth
{

   void ApplyDamage(int damange);

   void ApplyHeal(int heal);

   int GetHealth();

   float GetHealthPercent();

    void SetIsImmunityFromDamage(bool isImune);

    void SetImmunityForSec(float sec);

   bool GetHasImmunity();
}
