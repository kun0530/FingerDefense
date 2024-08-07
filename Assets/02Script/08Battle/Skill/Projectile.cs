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

            initialTargetPos = target.transform.position;
            direction = (initialTargetPos - new Vector2(transform.position.x, transform.position.y)).normalized;
        }
    }
    private Vector2 initialTargetPos;
    private Vector3 direction;
    public float speed = 20f;

    public SkillType skill;
    [HideInInspector] public bool isBuffApplied = false;
    [HideInInspector] public int skillType;

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

        if (skillType == 0)
            MoveToTarget();
        else
            MoveToPosition();
    }

    private void MoveToTarget()
    {
        Vector2 targetPos = target.transform.position;
        Vector2 projectilePos = transform.position;

        var displacement = (targetPos - projectilePos).normalized * speed * Time.deltaTime;
        transform.position += new Vector3(displacement.x, displacement.y, 0f);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f || transform.position.y < initialTargetPos.y)
        {
            skill?.UseSkill(target, isBuffApplied);
            Destroy(gameObject);
        }
    }

    private void MoveToPosition()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (transform.position.y <= initialTargetPos.y)
        {
            skill?.UseSkill(gameObject, isBuffApplied);
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
