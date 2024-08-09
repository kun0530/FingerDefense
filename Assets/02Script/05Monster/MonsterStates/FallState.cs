using UnityEngine;

public class FallState : IState
{
    MonsterController controller;
    private Collider2D collider;
    private float startY;
    
    private float velocity;
    private float gravity = -9.8f;

    public FallState(MonsterController controller)
    {
        this.controller = controller;
        collider = controller.GetComponent<Collider2D>();
    }

    public void Enter()
    {
        startY = controller.transform.position.y;
        velocity = 0f;

        if (!controller.IsDead)
            controller.monsterAni.SetAnimation(MonsterSpineAni.MonsterState.LAYDOWN_AFTER, true, 1f);
    }

    public void Update()
    {
        velocity += gravity * Time.deltaTime;
        controller.transform.position += new Vector3(0, velocity * Time.deltaTime, 0);

        if (controller.transform.position.y <= controller.targetFallY)
        {
            controller.transform.position = new Vector3(controller.transform.position.x, controller.targetFallY, 0f);

            if (controller.Status.Data.Height <= startY - controller.targetFallY)
            {
                controller.Die(DamageReason.FALL_DAMAGE);
            }
            
            if (!controller.IsDead)
                controller.TryTransitionState<PatrolState>();
            else
                controller.TryTransitionState<IdleState<MonsterController>>();
        }
    }

    public void Exit()
    {
        collider.enabled = true;
    }
}
