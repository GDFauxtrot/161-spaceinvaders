using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Animatey : MonoBehaviour
{
    public List<Sprite> sprites;
    public bool useCoroutine;
    public float coroutineTimer;

    int currentAnimIndex;
    SpriteRenderer sprRenderer;

    void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        if (!sprRenderer)
        {
            Debug.LogWarning(
                string.Format(
                "Animatey requires a Sprite Renderer on \"{0}\" to work! Creating one now.",
                gameObject.name));
            sprRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        SetFrame(0);

        if (useCoroutine)
        {
            StartCoroutine(AnimCoroutine());
        }
    }

    /// <summary>
    /// Set the sprite to be the next frame in its animation list. Loops when needed.
    /// </summary>
    public void CycleAnimation()
    {
        SetFrame(currentAnimIndex == sprites.Count - 1 ? 0 : currentAnimIndex + 1);
    }

    /// <summary>
    /// Force the animation frame to be a certain index.
    /// </summary>
    public void SetAnimFrame(int frameIndex)
    {
        if (frameIndex < 0 || frameIndex > sprites.Count-1)
        {
            Debug.LogError(string.Format("Setting anim on \"{0}\" to invalid index!", gameObject.name));
            return;
        }
        SetFrame(frameIndex);
    }

    /// <summary>
    /// Internal function for setting the frame that is shown.
    /// </summary>
    private void SetFrame(int frameIndex)
    {
        currentAnimIndex = frameIndex;
        sprRenderer.sprite = sprites[currentAnimIndex];
    }

    private IEnumerator AnimCoroutine()
    {
        float timer = 0;

        while (true)
        {
            float coroutineTime = coroutineTimer <= 0 ? 1 : coroutineTimer;

            if (!useCoroutine)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                timer = 0;
                continue;
            }
            
            if (timer < coroutineTime)
            {
                timer += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            else
            {
                // Cycle anim for the amount of time elapsed;
                // if coroutineTime = Time.deltaTime/2f, anim get cycled twice, for instance.
                for (int i = 0; i < (int) Mathf.Floor(timer / coroutineTime); ++i)
                {
                    CycleAnimation();
                }

                timer = 0;
            }
        }
    }
}
