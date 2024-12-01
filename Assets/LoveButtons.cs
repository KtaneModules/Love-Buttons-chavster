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

    private bool[] LoveCaps = new bool[4];
    private bool ModuleSolved;

    void Awake()
    {
        _moduleId = ModuleIdCounter++;
        Module.OnActivate += Activate;
        /*
        foreach (KMSelectable object in keypad) {
            object.OnInteract += delegate () { keypadPress(object); return false; };
        }
        */

        //button.OnInteract += delegate () { buttonPress(); return false; };

        DisplayText.text = Keywords[Rnd.Range(0, Keywords.Length)];
    }

    void Activate()
    {

    }

    void Start()
    {
        
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
