using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IState
{
    MonsterController monster;
    private Collider2D collider;
    private float startY;
    
    private float velocity;
    private float gravity = -9.8f;

    public FallState(MonsterController monster)
    {
        this.monster = monster;
        collider = monster.GetComponent<BoxCollider2D>();
    }

    public void Enter()
    {
        startY = monster.transform.position.y;
        velocity = 0f;
    }

    public void Update()
    {
        velocity += gravity * Time.deltaTime;
        monster.transform.position += new Vector3(0, velocity * Time.deltaTime, 0);

        if (monster.transform.position.y <= monster.targetFallY)
        {
            monster.transform.position = new Vector3(monster.transform.position.x, monster.targetFallY, 0f);

            if (monster.Status.data.Height <= startY - monster.targetFallY)
                monster.DamageHp(monster.Status.currentHp);
            else
                monster.TryTransitionState<PatrolState>();
        }
    }

    public void Exit()
    {
        collider.enabled = true;
    }
}
