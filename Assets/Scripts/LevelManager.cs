using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelNo = 0;
    public LevelInfo[] levelInfos;

    public LevelInfo GetLevelInfo()
    {
        if (levelInfos[levelNo] != null)
            return levelInfos[levelNo];

        return null;
    }
}
