using MauiMastery.LayoutsDeepDive.Models;

namespace MauiMastery.LayoutsDeepDive.ViewModels;

public class LayoutsViewModel
{
    public List<LayoutItem> Items { get; } = new()
    {
        new("Grid", "Таблична розмітка з підтримкою рядків, стовпців, Auto та Proportional (*) розмірів", "Grid"),
        new("VerticalStackLayout", "Вертикальне лінійне розташування без накладних витрат старого StackLayout", "Stack"),
        new("HorizontalStackLayout", "Горизонтальне лінійне розташування елементів в один рядок", "Stack"),
        new("FlexLayout", "Адаптивний лейаут на основі CSS Flexible Box Module із перенесенням (Wrap)", "Flex"),
        new("AbsoluteLayout", "Точне пропорційне або абсолютне позиціонування за X, Y, Width, Height", "Absolute")
    };
}