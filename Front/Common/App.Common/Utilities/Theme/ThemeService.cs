using MudBlazor;

namespace App.Common.Utilities.Theme;

public class ThemeService : IThemeService
{
    private readonly MudTheme _darkTheme;
    private readonly MudTheme _lightTheme;

    public ThemeService()
    {
        _lightTheme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary                  = "#8458B3", // Deep purple for main actions
                Secondary                = "#a0d2eb", // Soft blue for accents
                Tertiary                 = "#d0bdf4", // Light purple for highlights
                Background               = "#e5eaf5", // Subtle blueish background
                Surface                  = "#FFFFFF", // Clean white surfaces
                AppbarBackground         = "#8458B3", // Purple appbar
                AppbarText               = "#FFFFFF", // White for contrast
                DrawerBackground         = "#FFFFFF", // White drawer
                DrawerText               = "#2E2E2E", // Dark gray text
                TextPrimary              = "#2E2E2E", // Dark gray for readability
                TextSecondary            = "#5A5A5A", // Lighter gray for secondary
                ActionDefault            = "#a28089", // Muted purple for actions
                ActionDisabled           = "#B0B0B0", // Neutral disabled
                ActionDisabledBackground = "#E5E5E5", // Light disabled background
                Divider                  = "#D1D5DB", // Subtle gray divider
                Success                  = "#2E7D32", // Green for success
                Info                     = "#0288D1", // Blue for info
                Warning                  = "#FBC02D", // Yellow for warning
                Error                    = "#D32F2F"  // Red for error
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto", "Arial", "sans-serif" },
                    FontSize   = "0.875rem"
                },
                H1 = new H1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "2.125rem"
                },
                H2 = new H2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.75rem"
                },
                H3 = new H3Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.5rem"
                },
                H4 = new H4Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.25rem"
                },
                H5 = new H5Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.125rem"
                },
                H6 = new H6Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1rem"
                },
                Button = new ButtonTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "0.875rem"
                },
                Body1 = new Body1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Body2 = new Body2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle1 = new Subtitle1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle2 = new Subtitle2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Caption = new CaptionTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Overline = new OverlineTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                }
            }
        };

        _darkTheme = new MudTheme
        {
            PaletteDark = new PaletteDark
            {
                Primary                  = "#9e69d6", // Lighter purple for visibility
                Secondary                = "#c0fcff", // Softer blue for accents
                Tertiary                 = "#f9e2ff", // Muted light purple for highlights
                Background               = "#121212", // Standard dark background
                Surface                  = "#1E1E1E", // Slightly lighter surface
                AppbarBackground         = "#1E1E1E", // Dark gray appbar
                AppbarText               = "#E8ECEF", // Off-white text
                DrawerBackground         = "#1E1E1E", // Dark drawer
                DrawerText               = "#B0BEC5", // Light gray text
                TextPrimary              = "#E8ECEF", // Off-white for readability
                TextSecondary            = "#A0AEC0", // Muted gray for secondary
                ActionDefault            = "#c299a4", // Lighter muted purple for actions
                ActionDisabled           = "rgba(255,255,255, 0.3)",
                ActionDisabledBackground = "rgba(255,255,255, 0.08)",
                Divider                  = "rgba(255,255,255, 0.12)",
                Success                  = "#66BB6A", // Green for success
                Info                     = "#4FC3F7", // Light blue for info
                Warning                  = "#FFCA28", // Amber for warning
                Error                    = "#EF5350"  // Red for error
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto", "Arial", "sans-serif" },
                    FontSize   = "0.875rem"
                },
                H1 = new H1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "2.125rem"
                },
                H2 = new H2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.75rem"
                },
                H3 = new H3Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.5rem"
                },
                H4 = new H4Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.25rem"
                },
                H5 = new H5Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1.125rem"
                },
                H6 = new H6Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "1rem"
                },
                Button = new ButtonTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" },
                    FontSize   = "0.875rem"
                },
                Body1 = new Body1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Body2 = new Body2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle1 = new Subtitle1Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Subtitle2 = new Subtitle2Typography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Caption = new CaptionTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                },
                Overline = new OverlineTypography
                {
                    FontFamily = new[] { "Vazir", "Roboto" }
                }
            }
        };

        CurrentTheme = _darkTheme;
        IsDarkMode   = true;
    }

    public bool IsDarkMode { get; private set; }

    public event EventHandler? ThemeChanged;

    public MudTheme CurrentTheme { get; private set; }

    public void ToggleDarkLightMode(bool isDarkMode, bool refresh = true)
    {
        if (isDarkMode)
            SetDarkTheme(refresh);
        else
            SetLightTheme(refresh);
    }

    public void SetLightTheme(bool refresh)
    {
        CurrentTheme = _lightTheme;
        IsDarkMode   = false;

        if (refresh)
            OnThemeChanged();
    }

    public void SetDarkTheme(bool refresh)
    {
        CurrentTheme = _darkTheme;
        IsDarkMode   = true;

        if (refresh)
            OnThemeChanged();
    }

    public Task OnSystemPreferenceChanged(bool theme)
    {
        ToggleDarkLightMode(theme);
        return Task.CompletedTask;
    }

    protected virtual void OnThemeChanged()
    {
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }
}