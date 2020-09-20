namespace OrCor_GameName
{
    public class Enumerators
    {

        public enum ButtonState
        {
            ACTIVE,
            DEFAULT
        }

        public enum SoundType : int
        {
          //  CLICK,
          //  OTHER,
         //   BACKGROUND,
        }

        public enum NotificationType
        {
            LOG,
            ERROR,
            WARNING,

            MESSAGE
        }

        public enum Language
        {
            NONE,

            DE,
            EN,
            UK
        }

        public enum ScreenOrientationMode
        {
            PORTRAIT,
            LANDSCAPE
        }

        public enum CacheDataType
        {
            USER_LOCAL_DATA
        }

        public enum NotificationButtonState
        {
            ACTIVE,
            INACTIVE
        }

        public enum ShopReapeatType
        {
            REPEATING, ONE_TIME
        }

        public enum GameStateTypes
        {
            MENU, START_GAMEPLAY
        }
    }
}