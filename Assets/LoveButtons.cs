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

    private string[] Keywords = { "Do you love me?",
"Do you want to\nwatch a movie?",
"Do you want to\nhang out?",
"Do you need\nsome water?",
"Do you want\nto kiss?",
"Do you want\nto walk?",
"Are you ready for\nthe luncheon?",
"Do you have\nany plushies?",
"Are you\nalso cold?"  };
    private int ChosenKeyword;
    private bool IsAnswerYes;
    private int PreviousStrikes;

    private Coroutine[] AnimCoroutines;
    private bool ModuleSolved;

    void Awake()
    {
        _moduleId = ModuleIdCounter++;
        Module.OnActivate += Activate;
        AnimCoroutines = new Coroutine[Buttons.Length];

        ChosenKeyword = Rnd.Range(0, Keywords.Length);
        DisplayText.text = Keywords[ChosenKeyword];
        Debug.LogFormat("[Love Buttons #{0}] The displayed text is: {1}", _moduleId, DisplayText.text.Replace('\n', ' '));
        for (int i = 0; i < Buttons.Length; i++)
        {
            int x = i;
            Buttons[i].OnInteract += delegate { ButtonPress(x); return false; };
        }
    }

    void Start()
    {
        Calculate();
        Debug.LogFormat("[Love Buttons #{0}] This means that you should press {1}.", _moduleId, IsAnswerYes? "YES" : "NO");
    }

    void Activate()
    {

    }

    void Update()
    {
        if (Bomb.GetStrikes() != PreviousStrikes)
        {
            Calculate();
            PreviousStrikes = Bomb.GetStrikes();

            Debug.LogFormat("[Love Buttons #{0}] The number of strikes has changed! The new answer is {1}!", _moduleId, IsAnswerYes ? "YES!" : "NO!");
        }
    }

    void Calculate()
    {

        switch (ChosenKeyword)
        {
            case 0:
                IsAnswerYes = Bomb.GetBatteryCount() % 2 == 0;
                break;

            case 1:
                IsAnswerYes = Bomb.GetSolvableModuleNames().Count() % 2 == 1;
                break;

            case 2:
                IsAnswerYes = Bomb.GetSerialNumberNumbers().Sum() >= 8;
                break;

            case 3:
                IsAnswerYes = !Bomb.GetSerialNumberLetters().Any(x => "AEIOU".Contains(x));
                break;

            case 4:
                IsAnswerYes = Bomb.GetStrikes() % 2 == 1;
                break;

            case 5:
                IsAnswerYes = Bomb.GetPortCount() % 2 == 1;
                break;

            case 6:
                IsAnswerYes = Bomb.GetIndicators().Count() >= 3;
                break;
            case 7:
                var indsConcatenated = Bomb.GetOnIndicators().Join("");
                IsAnswerYes = "CHAV".All(x => indsConcatenated.Contains(x)) || "SALT".All(x => indsConcatenated.Contains(x));
                break;
            case 8:
                IsAnswerYes = Bomb.GetOnIndicators().Count() >= 2;
                break;
            

        }
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

        if ((pos == 0 && !IsAnswerYes) || (pos == 1 && IsAnswerYes))
        {
            Module.HandlePass();
            Debug.LogFormat("[Love Buttons #{0}] You pressed {1}, which was correct. Module solved! You're winner!", _moduleId, pos == 1 ? "YES!" : "NO!");
        }
        else
        {
            Module.HandleStrike();
            Debug.LogFormat("[Love Buttons #{0}] You pressed {1}, which was incorrect. Strike! You're loser!", _moduleId, pos == 1 ? "YES!" : "NO!");
        }
    }
    private IEnumerator ButtonAnim(int pos, float duration = 0.075f)
    {
        float init = 0.004f;
        float depression = 0.002f;

        Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, init, Buttons[pos].transform.localPosition.z);

        float timer = 0;
        while (timer < duration)
        {

            Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, Mathf.Lerp(init, init - depression, timer / duration), Buttons[pos].transform.localPosition.z);

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
    private readonly string TwitchHelpMessage = @"Use '!{0} YES/NO' to press YES! or NO!.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string Command)
    {
        Command = Command.ToLowerInvariant();
        if (Command != "yes" && Command != "no")
        {
            yield return "sendtochaterror Invalid command.";
            yield break;
        }
        yield return null;
        if (Command == "yes")
        {
            Buttons[1].OnInteract();
        }
        else Buttons[0].OnInteract();
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
        Buttons[IsAnswerYes ? 1 : 0].OnInteract();
    }
    
}
