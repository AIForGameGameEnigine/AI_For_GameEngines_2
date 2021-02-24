using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
   public enum TeamType
   {
        Light,
        Dark
   };

    public TeamType teamType;

    public enum RoleType
    {
        Champion,
        Minion,
        Tower
    }

    public RoleType roleType;

    public enum LaneType
    {
        Top,
        Mid,
        Bot
    };

    public LaneType lane;
}
