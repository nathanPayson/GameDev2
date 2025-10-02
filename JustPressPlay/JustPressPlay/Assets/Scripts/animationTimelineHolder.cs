using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using Unity.VisualScripting;

public class animationTimelineHolder : MonoBehaviour
{
    public PlayableDirector director;
    public static UnityEvent animationComplete = new UnityEvent();
    public GameObject objects;

    private void Awake()
    {

    }
    private void OnEnable()
    {
        gmAnimationState.playAnimation.AddListener(PlayTimeline);
    }
    private void OnDisable()
    {
        gmAnimationState.playAnimation.RemoveListener(PlayTimeline);
    }
    void PlayTimeline()
    {
        director.Play();
    }
    public void AnimationComplete()
    {
        Debug.Log("Animation Complete");
        animationComplete?.Invoke();
        objects.SetActive(false);
    }
}
