using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Menu
{
    MainMenu, 
    Credits,
    Options
}

// usually for a proper main menu, it'd be good to have a seperate controller script per "screen" (Settings, MainMenu, Credits, etc.)
// that all inherit from a generic Menu script, much better for inputs and menu switching. however, i just wanna get ts done
public class MenuController : MonoBehaviour
{
    private Menu CurrentMenu = Menu.MainMenu;
    
    public CanvasGroup MainMenuGroup;
    public CanvasGroup CreditsGroup;
    public CanvasGroup OptionsGroup;

    public CanvasGroup BlackScreenOfDeath;

    public float MenuTransitionTime = 0.5f;
    
    public void OnStartGame()
    {
        TransitionController.TransitionScene("TEST GAMEPLAY SCENE");
    }
    public void OnOpenOptions() => OnOpenMenu(Menu.Options);
    public void OnOpenCredits() => OnOpenMenu(Menu.Credits);

    public void OnQuitGame()
    {
        BlackScreenOfDeath.DOFade(1f, 2.5f).OnComplete(
            () => Application.Quit());
    }

    public void OnOpenMenu(Menu MenuType)
    {
        switch (MenuType)
        {
            case Menu.MainMenu:
                MainMenuGroup.DOFade(1f, MenuTransitionTime);
                MainMenuGroup.GetComponent<RectTransform>().DOAnchorPosY(0f, MenuTransitionTime);

                if (CurrentMenu == Menu.Credits)
                {
                    CreditsGroup.DOFade(0f, MenuTransitionTime);
                    CreditsGroup.GetComponent<RectTransform>().DOAnchorPosY(-1080f, MenuTransitionTime);
                }
                else
                {
                    OptionsGroup.DOFade(0f, MenuTransitionTime);
                    OptionsGroup.GetComponent<RectTransform>().DOAnchorPosY(1080f, MenuTransitionTime);
                }
                
                break;
            
            case Menu.Credits:
                MainMenuGroup.DOFade(0f, MenuTransitionTime);
                MainMenuGroup.GetComponent<RectTransform>().DOAnchorPosY(1080, MenuTransitionTime);
                CreditsGroup.DOFade(1f, MenuTransitionTime);
                CreditsGroup.GetComponent<RectTransform>().DOAnchorPosY(0, MenuTransitionTime);
                break;
            
            case Menu.Options:
                MainMenuGroup.DOFade(0f, MenuTransitionTime);
                MainMenuGroup.GetComponent<RectTransform>().DOAnchorPosY(-1080, MenuTransitionTime);
                OptionsGroup.DOFade(1f, MenuTransitionTime);
                OptionsGroup.GetComponent<RectTransform>().DOAnchorPosY(0, MenuTransitionTime);
                break;
        }
        
        CurrentMenu = MenuType;
    }

    // please never do ts in an actual project
    public void OnEscape()
    {
        switch (CurrentMenu)
        {
            case Menu.MainMenu:
                return;
            case Menu.Credits:
                OnOpenMenu(Menu.MainMenu);
                break;
            case Menu.Options:
                OnOpenMenu(Menu.MainMenu);
                break;
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            OnEscape();
    }
}
