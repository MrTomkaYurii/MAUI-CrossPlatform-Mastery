namespace MauiMastery.GraphicsAndCanvas.Graphics;

public class ChartDrawable
{
    public List<double> Values { get; set; } = new() { 20, 45, 80, 60, 95, 120 };

    public void Draw(object canvasMock, int width, int height)
    {
        // Метод виконує розрахунок точок сплайну та градієнтну заливку графіка
    }
}