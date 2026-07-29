using System;

namespace FNF_plus.Tool.ConfigTool;

public static class GameSetting
{
    
    public static class KeyBinding
    {
        public enum Key
        {
            None,
            Left,
            Down,
            Up,
            Right,
        }

        public static string[] Value = new string[Enum.GetValues<Key>().Length];

        static KeyBinding()
        {
            for (int i = 0; i < Enum.GetValues<Key>().Length; i++)
            {
                if (i == (int)Key.None) continue;
                Value[i] = (string)ConfigHandler.Config.GetValue("KeyBind", Enum.GetNames<Key>()[i].ToLower());
            }
        }

        public static string GetKey(Key key)
        {
            return Value[(int)key];
        }
        

        public static void Initialize() {}

    }
    
    
    
    
}