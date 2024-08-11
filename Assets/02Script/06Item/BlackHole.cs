using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private float lifeTime = 5f;
    private float timer = 0f;

    public float pullSpeed = 10f;  // 블랙홀로 빨아들이는 속도
    public float rotateSpeed = 200f;  // 오브젝트가 블랙홀 주위를 회전하는 속도 deg/sec

    private List<GameObject> monsters;

    private void OnEnable()
    {
        // var targeting = new FindingTargetInCircle(gameObject.transform, 5f, Defines.Layers.MONSTER_LAYER);
        // monsters = targeting.FindTargets();
    }

    private void Update()
    {
        // timer += Time.deltaTime;
        // if (timer >= lifeTime)
        // {
        //     Destroy(gameObject);
        // }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (!other.TryGetComponent<MonsterController>(out var monster)
    //         || !monster.IsTargetable)
    //         return;

    //     var buff = new BuffData
    //     {
    //         LastingTime = 10f
    //     };
    //     buff.BuffActions.Add((1, -100));

    //     monster.TakeBuff(buff);
    // }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent<MonsterController>(out var monster)
            || !monster.IsTargetable)
            return;

        Vector2 center = transform.position;
        Vector2 target = other.transform.position;

        var rotatePos = Utils.RotatePosition(center, target, rotateSpeed * Time.deltaTime);
        other.transform.position = rotatePos;

        Vector3 pullVec = (center - rotatePos).normalized * pullSpeed * Time.deltaTime;
        other.transform.position += pullVec;

        // var angle = rotateSpeed * Time.deltaTime * Mathf.Deg2Rad;
        // other.transform.RotateAround(transform.position, Vector3.forward, 360f * Time.deltaTime);
    }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (!other.TryGetComponent<MonsterController>(out var monster)
    //         || !monster.IsTargetable)
    //         return;

    //     other.transform.rotation = Quaternion.Euler(Vector3.zero);
    // }
}
