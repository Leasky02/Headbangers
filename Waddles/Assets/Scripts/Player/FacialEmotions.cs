using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialEmotions : MonoBehaviour
{
    [SerializeField] private SpriteRenderer eyebrows;
    [SerializeField] private SpriteRenderer eyes;
    [SerializeField] private SpriteRenderer mouth;

    [SerializeField] private Sprite[] eyebrow_sprite;
    //0 = happy eyebrows
    //1 = neutral eyebrows
    //2 = sad eyebrows
    [SerializeField] private Sprite[] eye_sprite;
    //0 = open eye
    //1 = closed eye
    //2 = dead eye
    [SerializeField] private Sprite[] mouth_sprite;
    //0 = happy mouth
    //1 = happy open mouth
    //2 = sad mouth
    //3 = sad open mouth

    private bool blink = true;
    [SerializeField] private float blinkLength;

    private void Start()
    {
        StartCoroutine(Blink());

        InvokeRepeating("RandomEmotion", 1f, 1f);
    }

    private IEnumerator Blink()
    {
        if(blink)
        {
            Sprite currentEyeState = eyes.sprite;

            eyes.sprite = eye_sprite[1];

            yield return new WaitForSeconds(blinkLength);

            eyes.sprite = currentEyeState;
        }

        float nextBlink = Random.Range(0.5f, 4f);

        yield return new WaitForSeconds(nextBlink);

        StartCoroutine(Blink());
    }

    public void SetBlink(bool blinkState)
    {
        blink = blinkState;
    }

    public void RandomEmotion()
    {
        string eyebrowState = "", eyeState = "", mouthState = "";
        int eyebrowIndex, eyeIndex, mouthIndex;

        eyebrowIndex = Random.Range(0, 3);
        eyeIndex = Random.Range(0, 2);
        mouthIndex = Random.Range(0, 4);

        switch(eyebrowIndex)
        {
            case 0:
                eyebrowState = "happy";
                break;

            case 1:
                eyebrowState = "neutral";
                break;

            case 2:
                eyebrowState = "sad";
                break;
        }

        switch (eyeIndex)
        {
            case 0:
                eyeState = "open";
                break;

            case 1:
                eyeState = "closed";
                break;
        }

        switch (mouthIndex)
        {
            case 0:
                mouthState = "happy";
                break;

            case 1:
                mouthState = "happy open";
                break;

            case 2:
                mouthState = "sad";
                break;

            case 3:
                mouthState = "sad open";
                break;
        }

        ChangeEmotion(eyebrowState, eyeState, mouthState);
    }

    public void ChangeEmotion(string eyebrowState, string eyeState, string mouthState)
    {
        //set eyebrows
        switch(eyebrowState)
        {
            case"happy":
                eyebrows.sprite = eyebrow_sprite[0];
                break;

            case "neutra":
                eyebrows.sprite = eyebrow_sprite[1];
                break;

            case "sad":
                eyebrows.sprite = eyebrow_sprite[2];
                break;
        }

        //set eyes
        switch (eyeState)
        {
            case "open":
                eyes.sprite = eye_sprite[0];
                break;

            case "closed":
                eyes.sprite = eye_sprite[1];
                break;

            case "dead":
                eyes.sprite = eye_sprite[2];
                break;
        }

        //set mouth
        switch (mouthState)
        {
            case "happy":
                mouth.sprite = mouth_sprite[0];
                break;

            case "happy open":
                mouth.sprite = mouth_sprite[1];
                break;

            case "sad":
                mouth.sprite = mouth_sprite[2];
                break;

            case "sad open":
                mouth.sprite = mouth_sprite[3];
                break;
        }
    }
}
