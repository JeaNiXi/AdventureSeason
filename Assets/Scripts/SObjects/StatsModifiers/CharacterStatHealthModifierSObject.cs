using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DamageHealthModifier", menuName ="Stat Modifier/Damage Health")]

public class CharacterStatHealthModifierSObject : CharacterStatsModifierSObject
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Arzued Arzued = character.GetComponent<Arzued>();
        if (Arzued != null)
        {
            Debug.Log("minus health");
            Arzued.Health -= val;
        }
    }
}
