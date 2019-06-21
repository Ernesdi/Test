using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConst{

    private static AppConst instance;

    private AppConst() { }

    public static AppConst Instance()
    {
        if (instance==null)
        {
            instance = new AppConst(); 
        }
        return instance;
    }

    private int circleSpeed = -130;
    private int pinShootSpeed = 30;

    public int CircleSpeed { get { return circleSpeed; } }
    public int PinShootSpeed { get { return pinShootSpeed; } }

}
