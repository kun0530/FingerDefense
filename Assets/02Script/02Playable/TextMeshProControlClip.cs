using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TextMeshProControlClip : PlayableAsset
{
    public string text;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextMeshProControlBehaviour>.Create(graph);
        TextMeshProControlBehaviour behaviour = playable.GetBehaviour();
        behaviour.text = text;
        return playable;

    }
}

