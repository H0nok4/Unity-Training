using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffEffect
{
    void StateChange(ref int stateValue,SrpgClassPropertyType type);
    void OnAttack(SrpgClassUnit unit, SrpgClassUnit target,DamageDetail damage,DamageDetail originDamageDetail);
    void OnDefend(SrpgClassUnit unit, SrpgClassUnit attacker,DamageDetail damage,DamageDetail originDamageDetail);
    void OnTurnStart(SrpgClassUnit unit);
    void OnTurnEnd(SrpgClassUnit unit);
    void OnBuffAdd();
    void OnBuffRemove();

}
