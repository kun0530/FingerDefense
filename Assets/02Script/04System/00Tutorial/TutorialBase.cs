using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    public abstract void Enter();

    public abstract void Execute(TutorialController controller);
    
    public abstract void Exit();
}
