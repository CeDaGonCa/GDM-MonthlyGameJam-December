using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BetweenScenesController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float lettersPerSecond;
    [SerializeField] float pauseInLineBreak = 0.5f;
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] List<ScenePart> Scenes;

    [Header("References")]
    [SerializeField] Image background;
    [SerializeField] Image DialogBox;
    [SerializeField] TMP_Text dialogText;
    
    bool isTyping = false;
    int currentLine = 0;
    int currentScene = 0;
    
    void Start()
    {
        dialogText.text = "";
        StartScene();
    }
    private void SetAlpha(Image image, float value)
    {
        var TmpColor = image.color;
        TmpColor.a = value;
        image.color = TmpColor;
    }
    public void StartScene()
    {
        if (Scenes.Count == 0)
        {
            Debug.Log("No references on Scenes");
            return;
        }
        StartCoroutine(TypeDialog(Scenes[0].Lines[0]));
    }

    void Update()
    {
        if (currentScene >= Scenes.Count) return;
        if (Keyboard.current.enterKey.wasPressedThisFrame && !isTyping)
        {
            currentLine++;
            if (currentLine < Scenes[currentScene].Lines.Count)
            {
                StartCoroutine(TypeDialog(Scenes[currentScene].Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                currentScene++;
                if (currentScene >= Scenes.Count)
                {
                    TransitionController.TransitionScene("TEST GAMEPLAY SCENE");
                }
                StartCoroutine(TypeDialog(Scenes[currentScene].Lines[currentLine])); // This could cause a bug later on
            }
        }
    }
    
    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            if (letter == '\n') yield return new WaitForSeconds(pauseInLineBreak);
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }
}
