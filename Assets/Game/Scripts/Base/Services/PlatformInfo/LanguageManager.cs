namespace Game.Scripts.Base.Services.PlatformInfo
{
    public class LanguageManager
    {
        private static string English = "en";
        private static string Russian = "ru";
        private static string Turkish = "tr";
        private static string French = "fr";
        private static string Italian = "it";
        private static string German = "de";
        private static string Spanish = "es";
        private static string Chineese = "zh";
        private static string Portuguese = "pt";
        private static string Korean = "ko";
        private static string Japanese = "ja";
        private static string Arab = "ar";
        private static string Hindi = "hi";
        private static string Indonesian = "id";

        public static Language ConvertToEnum(string lang)
        {
            if ( lang == English )
                return Language.English;

            if ( lang == Russian )
                return Language.Russian;

            if ( lang == Turkish )
                return Language.Turkish;

            if ( lang == French )
                return Language.French;

            if ( lang == Italian )
                return Language.Italian;

            if ( lang == German )
                return Language.German;

            if ( lang == Spanish )
                return Language.Spanish;

            if ( lang == Chineese )
                return Language.Chinese;

            if ( lang == Portuguese )
                return Language.Portuguese;

            if ( lang == Korean )
                return Language.Korean;

            if ( lang == Japanese )
                return Language.Japanese;

            if ( lang == Arab )
                return Language.Arab;

            if ( lang == Hindi )
                return Language.Hindi;

            if ( lang == Indonesian )
                return Language.Indonesian;

            return Language.English;
        }
    }

    public enum Language : byte
    {
        English,
        Russian,
        Turkish,
        French,
        Italian,
        German,
        Spanish,
        Chinese,
        Portuguese,
        Korean,
        Japanese,
        Arab,
        Hindi,
        Indonesian,
    }
}