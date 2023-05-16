using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMode
{
    private string _name;
    private FullScreenMode _fullScreenMode;

    public ScreenMode(string name, FullScreenMode fullScreenMode)
    {
        _name = name;
        _fullScreenMode = fullScreenMode;
    }

    public string Name => _name;
    public FullScreenMode FullScreenMode => _fullScreenMode;
}
