using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Abilities", menuName = "Skill/Ability")]
public class NewAbilities : ScriptableObject
{
    public string abilityName;

    public float cooldownTime;
    public float activeTime;
    public float abilityForce;
}
