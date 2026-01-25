using System.Globalization;

namespace MauiMastery.ConvertersAndTriggers.Converters;

public class BoolToColorConverter
{
    public string TrueColor { get; set; } = "#10B981"; // Зелений
    public string FalseColor { get; set; } = "#EF4444"; // Червоний

    public string Convert(bool value) => value ? TrueColor : FalseColor;
}