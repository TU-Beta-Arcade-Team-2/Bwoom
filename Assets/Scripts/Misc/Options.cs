
public class Options
{
    public static float MASTER_VOLUME;
    public static float MUSIC_VOLUME;
    public static float SFX_VOLUME;

    public enum eScreenResolution
    {
        r1920x1080,
        r1280x960,
        r1024x768,
        r960x540,
        r640x360,
        Count
    }

    public static eScreenResolution SCREEN_RESOLUTION;

    public enum eWindowMode
    {
        Windowed,
        Borderless,
        FullScreen,
        Count
    }

    public static eWindowMode SCREEN_MODE;

    public static bool VSYNC;

    public static bool HOLD_TO_COMBO;
    public static bool CONTROLLER_RUMBLE;

    public enum eColourBlindness
    {
        None,
        Achromo,
        Proto,
        Deutero,
        Trito,
        Count
    }

    public static eColourBlindness COLOUR_BLINDNESS;

}
