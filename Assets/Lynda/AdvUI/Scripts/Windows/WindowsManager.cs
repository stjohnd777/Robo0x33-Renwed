using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : MonoBehaviour {

    public GenericWindow[] windows;

    public int currentwndowId;

    public int defaultWindowId;

    void Start()
    {
        GenericWindow.windowManager = this;
        Open(defaultWindowId);
    }

    public GenericWindow GetWindow(int id)
    {
        if (id < 0 || id >= windows.Length)
        {
            return null;
        }
        return windows[id];
    }


    public GenericWindow Open (int id){

        if ( id < 0 || id >= windows.Length)
        {
            return null;
        }
       ToggleWindow(id);
       return GetWindow(id);

    }
    private void ToggleWindow(int id)
    {
       foreach(GenericWindow w in windows)
       {
            if (w.gameObject.activeSelf)
            {
                w.Close();
            }
       }
      windows[id].Open();
    }

}
