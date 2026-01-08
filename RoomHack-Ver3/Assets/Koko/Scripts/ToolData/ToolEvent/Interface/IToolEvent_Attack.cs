using UnityEngine;

public interface IToolEvent_Attack
{
    IDamageable damageable { get; set; }

    int damage { get; set; }

    void GetDamageable(GameObject _hackTargetObject);
}
