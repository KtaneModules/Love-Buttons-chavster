using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class LoveButtons : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] Buttons;
    public TextMesh DisplayText;

    private int _moduleId;
    private static int ModuleIdCounter = 1;

    private string[] Keywords = { "Do you love me?", "Do you want to\nwatch a movie?", "Do you want to\nhang out?", "Do you need\nsome water?", "Do you want\nto kiss?", "Do you want\nto walk?" };

    private Coroutine[] AnimCoroutines;
    private bool[] LoveCaps = new bool[4];
    private bool ModuleSolved;

    void Awake()
    {
        _moduleId = ModuleIdCounter++;
        Module.OnActivate += Activate;
        AnimCoroutines = new Coroutine[Buttons.Length];

        DisplayText.text = Keywords[Rnd.Range(0, Keywords.Length)];
        for (int i = 0; i < Buttons.Length; i++)
        {
            int x = i;
            Buttons[i].OnInteract += delegate { ButtonPress(x); return false; };
        }
    }

    void Activate()
    {

    }

    void Start()
    {

    }
    void ButtonPress(int pos)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Buttons[pos].transform);
        Buttons[pos].AddInteractionPunch();
        if (AnimCoroutines[pos] != null)
        {
            StopCoroutine(AnimCoroutines[pos]);
        }
        AnimCoroutines[pos] = StartCoroutine(ButtonAnim(pos));

    }
    private IEnumerator ButtonAnim(int pos, float duration = 0.075f)
    {
        float init = 0.004f;
        float depression = 0.002f;

        Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, init, Buttons[pos].transform.localPosition.z);

        float timer = 0;
        while (timer < duration) 
        {
            
            Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, Mathf.Lerp(init, init - depression, timer/duration), Buttons[pos].transform.localPosition.z);

            yield return null; 
            timer += Time.deltaTime;

            
        }
        Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, init - depression, Buttons[pos].transform.localPosition.z);



        timer = 0;
        while (timer < duration)
        {
            Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, Mathf.Lerp(init - depression, init, timer / duration), Buttons[pos].transform.localPosition.z);

            yield return null;
            timer += Time.deltaTime;


        }
        Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, init, Buttons[pos].transform.localPosition.z);
    }


#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string Command)
    {
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
    }
}
