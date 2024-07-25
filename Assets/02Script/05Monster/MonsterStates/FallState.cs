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
                if (controller.dragDeathSkill == null)
                {
                    Logger.Log("스킬이 존재하지 않습니다.");
                }
                else
                {
                    Logger.Log($"Drag Skill: {controller.dragDeathSkill.skillData.Id}");
                }
                controller.dragDeathSkill?.UseSkill();
                controller.PlayDeathAnimation();
            }
            else
                controller.TryTransitionState<PatrolState>();
        }
    }

    public void Exit()
    {
        collider.enabled = true;
    }
}
