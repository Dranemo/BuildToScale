using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Voice : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    private Dictionary<int, string> sousTitres = new Dictionary<int, string>();
    [SerializeField] AudioSource source;

    TextMeshProUGUI text;

    public bool tutoFinished = false;
    public bool startTimer = false;

    private void Start()
    {
        text = GameObject.Find("Canvas").transform.Find("Sous titres").GetComponent<TextMeshProUGUI>();

        sousTitres.Add(0, "Hum Experiment n°3201? Can you hear me? Yes? Perfect!");
        sousTitres.Add(1, "You're here today to prerform some tests.");
        sousTitres.Add(2, "You will have to reproduce the shape that is presented to you inside the glass box.");
        sousTitres.Add(3, "You need to make sure that the rotation, scale and position is correct. You can help you with the colors.");
        sousTitres.Add(4, "It should have descended by now.");

        sousTitres.Add(5, "Well? 3201? Don't you know how to spawn a block perhaps?");
        sousTitres.Add(6, "I heard it's quite easy for robots like you.");
        sousTitres.Add(7, "Didn't the Builders taught you? You can summon and model matter around you.");
        sousTitres.Add(8, "Well, I'm no technician, but I heard you just need to press A to create a block, and E to destroy it.");
        sousTitres.Add(9, "As I mentionned before, you also can rescale it and move it around.");
        sousTitres.Add(10, "Now, try to reproduce the shape.");

        // Congrats
        sousTitres.Add(11, "Well done! You're a natural!");
        sousTitres.Add(12, "Congrats!");

        //Bad
        sousTitres.Add(13, "Oh no! That's not the right shape!");
        sousTitres.Add(14, "Try again!");
        sousTitres.Add(15, "You can do it!");
        sousTitres.Add(16, "Even 3200 was better...");




        StartCoroutine(TutoVoice());
    }

    public void PlayVoice(int index)
    {
        source.clip = clips[index];
        text.text = sousTitres[index];
        source.Play();
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            text.text = "";
        }
    }

    public void StopVoice()
    {
        source.Stop();
    }

    public IEnumerator TutoVoice()
    {
        bool voiceFinished = true;

        for (int i = 0; i < 5; i++)
        {
            PlayVoice(i);
            voiceFinished = false;
            while (!voiceFinished)
            {
                if (!source.isPlaying)
                {
                    voiceFinished = true;
                }
                yield return null;
            }
        }

        while (!voiceFinished)
        {
            if (!source.isPlaying)
            {
                voiceFinished = true;
            }
            yield return null;
        }


        startTimer = true;
        yield return new WaitForSeconds(4);
        for (int i = 5; i < 11; i++)
        {
            PlayVoice(i);
            voiceFinished = false;
            while (!voiceFinished)
            {
                if (!source.isPlaying)
                {
                    voiceFinished = true;
                }
                yield return null;
            }
            if(i == 7)
            {
                Player.GetPlayer().GetComponent<PlayerPower>().tutoAllowingSummon = true;
            }
        }

        while (!voiceFinished)
        {
            if (!source.isPlaying)
            {
                voiceFinished = true;
            }
            yield return null;
        }
        tutoFinished = true;

    }
}
