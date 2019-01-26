﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour {

    private Text text;
    private bool nextEvent;
    private GameObject nextPromptContinue;
    private GameObject nextPromptEnd;
    private bool isPrinting;
    public float PrintSpeed;

	void Start () {
        text = transform.Find("Text").gameObject.GetComponent<Text>();
        nextEvent = false; // if the user has requested the next dialogue
        nextPromptContinue = transform.Find("NextContinue").gameObject;
        nextPromptEnd = transform.Find("NextEnd").gameObject;

        // DEBUG
        string max_msg = "Lorem Lorem Lorem Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
        string max_msg2 = " Lorem Lorem Lorem Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
        StartCoroutine(ShowText(max_msg + max_msg2));
	}
	
    //
    // Public
    //

    public void ClickEvent() {
        nextEvent = true;
        HideNextPrompt();
    }

    public IEnumerator ShowText(string msg)
    {
        // will show the given text on the box delimited by 
        // click events, and hide the text box once complete
        RaiseBox();

        // display msg on the text box in chunks of maxMsgLength
        List<string> chunks = ChunkMsg(msg);

        // show next dialogue until there is none left
        for (int i = 0; i < chunks.Count; i++) {

            // show the next text
            bool isLast = (i == chunks.Count - 1);

            isPrinting = true;
            StartCoroutine(PushText(chunks[i], isLast));
            yield return new WaitUntil(() => !isPrinting);
            ShowNextPrompt(isLast);

            // wait for the user to press next
            yield return new WaitUntil(() => nextEvent);
            nextEvent = false;

        }

        // clear text box
        text.text = "";

        // hide the text box
        LowerBox();

    }

    //
    // Private
    //

    private IEnumerator PushText(string msg, bool isFinal) {
        text.text = "";
        foreach (char c in msg) {
            yield return new WaitForSeconds(PrintSpeed);
            text.text += c;
        }
        isPrinting = false;
    }

    private void RaiseBox() {
        // the text box should animate up onto the screen
        Debug.Log("Raise text box");
    } 

    private void LowerBox() {
        // the text box should lower until it is hidden off screen
        Debug.Log("Lower text box");
    } 

    private List<string> ChunkMsg(string msg) {
        
        List<string> chunks = new List<string>();
        int length = MaxMsgLength();
        int numChunks = (int)Mathf.Ceil(msg.Length / length);

        for (int i = 0; i <= numChunks; i++)
        {
            if (i * length + length <= msg.Length)
            {
                chunks.Add(msg.Substring(i * length, length) + "...");
            }
            else
            {
                chunks.Add(msg.Substring(i * length));
            }
        }
        return chunks;
    }

    private void ShowNextPrompt(bool isFinal) {
        if (!isFinal) {
            nextPromptContinue.SetActive(true);

        } else {
            nextPromptEnd.SetActive(true);

        }
    }

    private void HideNextPrompt() {
        nextPromptContinue.SetActive(false);
        nextPromptEnd.SetActive(false);
    }

    private int MaxMsgLength() {
        // returns the max text length that the box can show
        string max_msg = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
        return max_msg.Length;
    }

	void Update () {
        if (Input.GetKeyUp("space")) {
            ClickEvent();
        }
	}
}
