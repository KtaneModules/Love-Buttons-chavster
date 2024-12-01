using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;
using UnityEngine.Collections;

public class LoveButtons : MonoBehaviour {
 
   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMSelectable[] Button;
    public TextMesh DisplayText;

    string[] Keyword = { "Do you love me?", "Do you want to watch a movie?", "Do you want to hang out?", "Do you need some water?", "Do you want to kiss?", "Do you want to walk?" };

    bool[] LoveCap = new bool[4];
   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */

      //button.OnInteract += delegate () { buttonPress(); return false; };

   }

   void OnDestroy () { //Shit you need to do when the bomb ends
      
   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

    void Start() { //Shit that you calculate, usually a majority if not all of the module
        DisplayText.text = Keyword[Rnd.Range(0, Keyword.Length)];

        
   }

   void Update () { //Shit that happens at any point after initialization

   }

   void Solve () {
      
   }

    void Strike()
    {
      
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   IEnumerator TwitchHandleForcedSolve () {
      yield return null;
   }
}
