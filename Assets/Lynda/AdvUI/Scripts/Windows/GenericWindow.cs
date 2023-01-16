using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour
{

    public static WindowsManager windowManager;

    public GameObject firstSelected;

    protected virtual void Awake()
    {
        Close();
    }

    protected EventSystem eventSystem
    {
        get
        {
            return GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
    }

    public void OnFocus()
    {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    public virtual void Open()
    {
        Display(true);
        OnFocus();
    }

    public virtual void Close()
    {
        Display(false);
    }

    protected void Display(bool value)
    {
        gameObject.SetActive(value);
    }


    public virtual void NewGame()
    {
        Debug.Log("New Game");
    }

    public void Continue()
    {
        Debug.Log("Continue Game");
    }

    public void Options()
    {
        Debug.Log("Options");
    }
}
