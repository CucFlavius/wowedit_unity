using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MenuBarHandler handles the reproduced Winform UI Menu bar at the top of the screen.
/// Mainly menu boxes visibility states. The rest of the UI logic is done in Unity Editor.
/// </summary>

public class MenuBarHandler : MonoBehaviour
{
    public GameObject PanelClickToHideMenus;
    public GameObject[] MenuBoxes;
    public GameObject OpenedMenuBox;
    public GameObject HighlightedMenuButton;
    public bool MouseOvered = false;

    void Start()
    {
        // reset menus at start //
        CloseAllMenus();
    }

    public void OpenMenuBox(int menuNumber)
    {
        // enable the hidden panel that allos click-out to close the active menu//
        PanelClickToHideMenus.SetActive(true);
        // if clicked the same menu button //
        if (OpenedMenuBox == MenuBoxes[menuNumber])
        {
            OpenedMenuBox.SetActive(false);
            OpenedMenuBox = null;
        }
        else
        {
            // if first menu fade it in //
            if (OpenedMenuBox == null)
            {
                OpenedMenuBox = MenuBoxes[menuNumber];
                MenuBoxes[menuNumber].SetActive(true);
                Menu_FadeIn();
            }
            else
            {
                // close previous open menu //
                foreach (GameObject menu in MenuBoxes)
                {
                    OpenedMenuBox.SetActive(false);
                }
                OpenedMenuBox = MenuBoxes[menuNumber];
                // open menu //
                MenuBoxes[menuNumber].SetActive(true);
            }
        }
    }

    public void Menu_FadeIn()
    {
        OpenedMenuBox.GetComponent<CanvasRenderer>().SetAlpha(0.1f);
        OpenedMenuBox.GetComponent<UnityEngine.UI.Image>().CrossFadeAlpha(1f, .1f, false);
    }

    public void CloseAllMenus()
    {
        foreach (GameObject menu in MenuBoxes)
        {
            menu.SetActive(false);
            OpenedMenuBox = null;
            PanelClickToHideMenus.SetActive(false);
        }
    }

    public void PointerEnterMenuButton (int menuNumber)
    {
        if (OpenedMenuBox != null && OpenedMenuBox != MenuBoxes[menuNumber])
            OpenMenuBox(menuNumber);
    }

    public void ClickedMenuButton (int menuNumber)
    {
        OpenMenuBox(menuNumber);
    }
}