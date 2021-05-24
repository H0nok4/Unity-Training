using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffEffect
{
    void StateChange(ref int stateValue,SrpgClassPropertyType type);
    void OnAttack(SrpgClassUnit unit, SrpgClassUnit target,ref int damage);
    void OnDefend(SrpgClassUnit unit, SrpgClassUnit attacker,ref int damage);
    void OnTurnStart(SrpgClassUnit unit);
    void OnTurnEnd(SrpgClassUnit unit);

}
