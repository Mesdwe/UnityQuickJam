using System;

namespace QuickJam.Save
{
    // A base class for game data, to be extended
    [Serializable]
    public class GameData
    {
        public int playerID = 0;
        public float playTime = 0f;
        public bool isFirstTime = true;
    }
}