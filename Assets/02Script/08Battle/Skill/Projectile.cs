using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 initialPosOffset = new Vector3(1f, 2f, 0f);

    private GameObject caster;
    public GameObject Caster
    {
        get => caster;
        set
        {
            caster = value;
            if (!caster)
                return;

            transform.position = caster.transform.position + initialPosOffset;
        }
    }

    private GameObject target;
    public GameObject Target{
        get => target;
        set
        {
            isTargetSet = true;

            target = value;
            if (!target)
                return;

            targetPos = target.transform.position;
            direction = (targetPos - new Vector2(transform.position.x, transform.position.y)).normalized;
        }
    }
    private Vector2 targetPos;
    private Vector2 direction;
    private float speed = 10f;

    public SkillType skill;
    public bool isBuffApplied = false;

    private bool isTargetSet = false;

    private void OnEnable()
    {
        isTargetSet = false;
    }

    private void Update()
    {
        if (!isTargetSet)
            return;

        if (!Target)
        {
            Destroy(gameObject);
            return;
        }

        targetPos = target.transform.position;
        direction = (targetPos - new Vector2(transform.position.x, transform.position.y)).normalized;
        var displacement = direction * speed * Time.deltaTime;
        transform.position += new Vector3(displacement.x, displacement.y, 0f);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f || transform.position.y < targetPos.y)
        {
            if (Target.TryGetComponent<ITargetable>(out var targetable) && targetable.IsTargetable)
            {
                skill?.UseSkill(target, isBuffApplied);
            }
            Destroy(gameObject);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject != Target)
    //         return;

    //     skill?.UseSkill(Target, isBuffApplied);
    // }

    // protected abstract void MoveToTarget();
}
