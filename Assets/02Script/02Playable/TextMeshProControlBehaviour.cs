using UnityEngine.Playables;
using TMPro;

public class TextMeshProControlBehaviour : PlayableBehaviour
{
    public string text;

    private TextMeshProUGUI tmpComponent;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        tmpComponent = playerData as TextMeshProUGUI;

        if (tmpComponent != null)
        {
            tmpComponent.text = text;
        }
    }
}