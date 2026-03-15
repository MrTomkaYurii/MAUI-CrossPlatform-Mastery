# 21. Clean Architecture in Mobile Clients (Чиста архітектура у MAUI)

### Що це?
Поділ додатку на незалежні шари:
1. **Domain**: сутності, бізнес-правила (без жодних зовнішніх залежностей).
2. **Application**: сценарії використання (Use Cases), інтерфейси сервісів.
3. **Infrastructure**: реалізації API клієнтів, SQLite репозиторіїв, системних сервісів.
4. **Presentation**: XAML сторінки, ViewModels, конвертери.