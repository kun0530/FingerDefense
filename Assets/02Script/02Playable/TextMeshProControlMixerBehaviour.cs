using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class TextMeshProControlMixerBehaviour : PlayableBehaviour
{
    private TextMeshProUGUI tmpComponent;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        tmpComponent = playerData as TextMeshProUGUI;

        if (tmpComponent == null)
            return;

        int inputCount = playable.GetInputCount();
        string currentText = "";

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0.5f)
            {
                ScriptPlayable<TextMeshProControlBehaviour> inputPlayable = (ScriptPlayable<TextMeshProControlBehaviour>)playable.GetInput(i);
                TextMeshProControlBehaviour input = inputPlayable.GetBehaviour();
                currentText = input.text;
            }
        }

        if (tmpComponent != null && Application.isPlaying)
        {
            tmpComponent.text = currentText;
        }
    }
}