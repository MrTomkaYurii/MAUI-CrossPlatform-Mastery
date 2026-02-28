# 16. Platform Handlers Architecture (Архітектура Handlers замість Renderers)

### Що це?
У .NET MAUI замість громіздких Custom Renderers із Xamarin.Forms представлена легка архітектура **Handlers**:
- Повна відсутність залежностей від елементів Xamarin.
- Швидкість ініціалізації вища на 30-50%.
- Можливість модифікувати властивості нативних віджетів через `ModifyMapping` без створення підкласів!