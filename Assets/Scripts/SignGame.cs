using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SignGame : MonoBehaviour
{
    public Text TargetWord;
    public Text DisplayCurrentMessage;
    private string receivedLetters = "";
    private int wordIndex = 0;
    private List<string> wordBank = new List<string>();
    public static event System.Action<int> OnCorrectWordReceived;

    public bool isGameInProgress;
    public string curLetter;

    private void Start()
    {
        //TargetWord = GameObject.Find("WordToSpell").GetComponent<Text>();
        //DisplayCurrentMessage = GameObject.Find("CurrentMessage").GetComponent<Text>();
        isGameInProgress = false;
        int wordIndex = 0;
        TargetWord.text = "CYTOPLASM";

        wordBank.Add("CYTOPLASM");
        wordBank.Add("EXAMPLE");
        wordBank.Add("HELLO");

        SetNextTargetWord();
    }
    

    // The method that will handle the received data
    public void ReceiveData(string message)
    {
        curLetter = message;
        Debug.Log(curLetter);
    }
    
     public void StartGame()
    {
        isGameInProgress = true;
        Debug.Log("Game is now active!");
        while(isGameInProgress){
            Debug.Log("We active!");
        }
    }

    public void Game(){
       // Debug.Log("is game in progress? " + isGameInProgress);
       /*
        if(isGameInProgress){
        // Compare the received message with the next letter in the TargetWord
        if (message.Length == 1 && receivedLetters.Length < TargetWord.text.Length)
        {
            char nextLetter = TargetWord.text[receivedLetters.Length];
            if (message.ToUpper() == nextLetter.ToString().ToUpper())
            {
                receivedLetters += message.ToUpper();

                // Check if the received letters form the complete TargetWord
                if (receivedLetters == TargetWord.text.ToUpper())
                {
                    // Update the UI element on the main thread using UnityMainThreadDispatcher
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                    DisplayCurrentMessage.text = receivedLetters;
                    });

                    OnCorrectWordReceived?.Invoke(5);
                    isGameInProgress = false;
                    wordIndex++;
                    SetNextTargetWord();
                }
            }
        }

        // Update the UI element on the main thread using UnityMainThreadDispatcher
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                DisplayCurrentMessage.text = receivedLetters;
            });

        }
        else{
            Debug.Log("not in the loop");
        }
    }*/
    }
        

    private void SetNextTargetWord()
    {
        Debug.Log("Resetting");
        if (wordIndex < wordBank.Count)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {                    
                        TargetWord.text = wordBank[wordIndex];
                        receivedLetters = ""; // Reset the received letters for the new word
                    });
        }
    }

    // public void EndGame()
    // {
    //     isGameActive = false;
    //     Debug.Log("Game is now inactive!");
    // }

}
