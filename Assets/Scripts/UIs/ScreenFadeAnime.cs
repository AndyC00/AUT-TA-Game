using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ScreenFadeAnime : MonoBehaviour
{
    Animator anim;
    static readonly int FadeHash = Animator.StringToHash("Fade");

    public static ScreenFadeAnime Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Play()
    {
        gameObject.SetActive(true);
        anim.Play(FadeHash, 0, 0f);

        StartCoroutine(AutoDisable(anim.GetCurrentAnimatorStateInfo(0).length));
    }

    System.Collections.IEnumerator AutoDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}