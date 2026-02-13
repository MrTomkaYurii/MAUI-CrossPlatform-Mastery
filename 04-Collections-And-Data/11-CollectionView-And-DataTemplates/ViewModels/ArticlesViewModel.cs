using System.Collections.ObjectModel;
using MauiMastery.CollectionViewVirtualization.Models;

namespace MauiMastery.CollectionViewVirtualization.ViewModels;

public class ArticlesViewModel
{
    public ObservableCollection<Article> Articles { get; } = new()
    {
        new(1, "Огляд нововведень .NET 10 у кросплатформеній розробці", "Technology", true, DateTime.Now.AddDays(-1)),
        new(2, "Оптимізація пам'яті в CollectionView: віртуалізація та повторне використання", "Architecture", false, DateTime.Now.AddDays(-2)),
        new(3, "Побудова адаптивного інтерфейсу для складних екранів", "Design", false, DateTime.Now.AddDays(-3)),
        new(4, "Впровадження чистої архітектури в мобільні клієнти", "Architecture", true, DateTime.Now.AddDays(-4))
    };
}