using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private float lifeTime = 5f;
    private float timer = 0f;

    private List<GameObject> monsters;
    private float speed = 10f;

    private bool isAttract = true;

    private void OnEnable()
    {
        var targeting = new FindingTargetInCircle(gameObject.transform, 5f, Defines.Layers.MONSTER_LAYER);
        monsters = targeting.FindTargets();
    }

    private void LateUpdate()
    {
        if (isAttract)
        {
            isAttract = !isAttract;
            AttractMonsters();
        }
        else
            isAttract = !isAttract;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            SpreadMonsters();
            Destroy(gameObject);
        }
    }

    private void AttractMonsters()
    {
        foreach (var monster in monsters)
        {
            if (!monster)
                monsters.Remove(monster);

            var direction = gameObject.transform.position - monster.transform.position;
            monster.transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void SpreadMonsters()
    {

    }
}
