using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackColor(0.855f, 0.8623f, 0.8705f)]
[TrackClipType(typeof(TextMeshProControlClip))]
[TrackBindingType(typeof(TextMeshProUGUI))]
public class TextMeshProControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TextMeshProControlMixerBehaviour>.Create(graph, inputCount);
    }
}