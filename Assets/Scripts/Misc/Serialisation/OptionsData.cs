
using System.IO;

public class OptionsData : Serialisable
{
    public static readonly OptionsData Defaults = new(
        0.5f,
        0.5f,
        0.5f,
        eScreenResolution.r1920x1080,
        eWindowMode.FullScreen,
        false,
        false,
        false,
        eColourBlindness.None
        );


    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;

    public enum eScreenResolution
    {
        r1920x1080,
        r1280x960,
        r1024x768,
        r960x540,
        r640x360,
        Count
    }

    public eScreenResolution ScreenResolution;

    public enum eWindowMode
    {
        Windowed,
        Borderless,
        FullScreen,
        Count
    }

    public eWindowMode WindowMode;

    public bool VSync;

    public bool HoldToCombo;
    public bool ControllerRumble;

    public enum eColourBlindness
    {
        None,
        Achroma,
        Proto,
        Deutero,
        Trito,
        Count
    }

    public eColourBlindness ColourBlindness;

    public OptionsData(OptionsData od)
    {
        MasterVolume = od.MasterVolume;
        MusicVolume = od.MusicVolume;
        SfxVolume = od.SfxVolume;
        ScreenResolution = od.ScreenResolution;
        WindowMode = od.WindowMode;
        VSync = od.VSync;
        HoldToCombo = od.HoldToCombo;
        ControllerRumble = od.ControllerRumble;
        ColourBlindness = od.ColourBlindness;
    }

    public OptionsData(float masterVolume, float musicVolume, float sfxVolume, eScreenResolution screenRes, eWindowMode windowMode, bool vSync, bool hold2Combo, bool rumble, eColourBlindness colourBlindness)
    {
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        SfxVolume = sfxVolume;
        ScreenResolution = screenRes;
        WindowMode = windowMode;
        VSync = vSync;
        HoldToCombo = hold2Combo;
        ControllerRumble = rumble;
        ColourBlindness = colourBlindness;
    }

    // Serialised in order of the member variables... enum members are serialised as integers, boolean members are serialised as either 1 or 0
    public override void Serialise(StreamWriter writer)
    {
        writer.WriteLine(MasterVolume);
        writer.WriteLine(MusicVolume);
        writer.WriteLine(SfxVolume);

        writer.WriteLine((int)ScreenResolution);
        writer.WriteLine((int)WindowMode);

        SerialiseBool(VSync, writer);
        SerialiseBool(HoldToCombo, writer);
        SerialiseBool(ControllerRumble, writer);

        writer.WriteLine((int)ColourBlindness);
    }

    public override void Deserialise(StreamReader reader)
    {
        BetterDebugging.Assert(reader != null);

        MasterVolume = float.Parse(reader.ReadLine());
        MusicVolume = float.Parse(reader.ReadLine());
        SfxVolume = float.Parse(reader.ReadLine());

        ScreenResolution = (eScreenResolution)int.Parse(reader.ReadLine());
        WindowMode = (eWindowMode)int.Parse(reader.ReadLine());

        VSync = DeserialiseBool(reader);
        HoldToCombo = DeserialiseBool(reader);
        ControllerRumble = DeserialiseBool(reader);

        ColourBlindness = (eColourBlindness)int.Parse(reader.ReadLine());
    }
}
