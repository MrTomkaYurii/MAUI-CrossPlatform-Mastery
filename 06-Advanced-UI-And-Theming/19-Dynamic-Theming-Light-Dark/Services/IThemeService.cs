namespace MauiMastery.DynamicTheming.Services;

public enum AppThemeMode { System, Light, Dark }

public interface IThemeService
{
    AppThemeMode CurrentMode { get; }
    void ApplyTheme(AppThemeMode mode);
}

public class ThemeService : IThemeService
{
    public AppThemeMode CurrentMode { get; private set; } = AppThemeMode.System;

    public void ApplyTheme(AppThemeMode mode)
    {
        CurrentMode = mode;
    }
}