using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float lettersPerSecond;
    [SerializeField] float pauseInLineBreak = 0.5f;
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] List<ScenePart> Scenes;

    [Header("References")]
    [SerializeField] Image background;
    [SerializeField] Image prevBackground;
    [SerializeField] Image DialogBox;
    [SerializeField] TMP_Text dialogText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Sprite textBoxNoSpeaker;
    [SerializeField] Sprite textBoxSpeaker;
    bool isTyping = false;
    int currentLine = 0;
    int currentScene = 0;
    void Start()
    {
        dialogText.text = "";
        nameText.text = "";
        prevBackground.sprite = background.sprite;
        SetAlpha(prevBackground, 0f);
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
        SetScene(0);
        StartCoroutine(TypeDialog(Scenes[0].Lines[0]));
        CheckDialog();
    }
    private void SetScene(int idx)
    {
        prevBackground.sprite = background.sprite;
        SetAlpha(prevBackground, 1f);
        background.sprite = Scenes[idx].Background;
        prevBackground.DOFade(0f, idx == 0 ? 0f : fadeTime);
        //var seq = DOTween.Sequence(prevBackground.DOFade(0f,fadeTime));
    }

    void Update()
    {
        if (currentScene >= Scenes.Count) return;
        if (Keyboard.current.enterKey.wasPressedThisFrame && !isTyping)
        {
            currentLine++;
            if (currentLine < Scenes[currentScene].Lines.Count)
            {
                CheckDialog();
                StartCoroutine(TypeDialog(Scenes[currentScene].Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                currentScene++;
                if (currentScene >= Scenes.Count)
                {
                    StartTutorial();
                }
                SetScene(currentScene);
                StartCoroutine(TypeDialog(Scenes[currentScene].Lines[currentLine])); // This could cause a bug later on
                CheckDialog();
            }
        }
    }
    void CheckDialog()
    {
        var scene = Scenes[currentScene];
        if (scene.IsDialog)
        {
            DialogBox.sprite = textBoxSpeaker;
            nameText.text = scene.DialogOrder[currentLine];
        }
        else
        {
            DialogBox.sprite = textBoxNoSpeaker;
            nameText.text = "";
        }
    }
    void StartTutorial()
    {
        Debug.Log("Start Scene 5");
        Destroy(gameObject); // Not sure how to do this, probably with scene management
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
