# .NET MAUI XAML & Layouts Cheat Sheet

Швидка практична шпаргалка для щоденної розробки інтерфейсів на .NET MAUI.

---

## 1. Контейнери розмітки (Layout Containers)

### Grid (Таблиця)
```xml
<Grid RowDefinitions="Auto, 100, *" ColumnDefinitions="2*, *">
    <!-- Auto: за розміром контенту -->
    <!-- 100: фіксовані пікселі -->
    <!-- *: пропорційна частка вільного простору -->
    <Label Grid.Row="0" Grid.Column="0" Text="Заголовок" />
    <Button Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="2" Text="На всю ширину" />
</Grid>
```

### FlexLayout (CSS Flexbox)
```xml
<FlexLayout Direction="Row" Wrap="Wrap" JustifyContent="SpaceBetween" AlignItems="Center">
    <Button Text="Тег 1" Margin="4" />
    <Button Text="Тег 2" Margin="4" />
    <Button Text="Тег 3" Margin="4" />
</FlexLayout>
```

---

## 2. Синтаксис Data Binding

```xml
<!-- OneWay: тільки читання -->
<Label Text="{Binding FullName, Mode=OneWay}" />

<!-- TwoWay: двостороннє редагування -->
<Entry Text="{Binding Email, Mode=TwoWay}" />

<!-- StringFormat з локалізованим форматуванням -->
<Label Text="{Binding Price, StringFormat='Ціна: {0:C2}'}" />

<!-- FallbackValue та TargetNullValue -->
<Label Text="{Binding OptionalBio, TargetNullValue='Опис відсутній', FallbackValue='Помилка завантаження'}" />
```

---

## 3. Адаптивність тем (AppThemeBinding)

```xml
<ContentPage BackgroundColor="{AppThemeBinding Light=#FFFFFF, Dark=#121212}">
    <Label Text="Динамічний текст"
           TextColor="{AppThemeBinding Light=#1F2937, Dark=#F9FAFB}" />
</ContentPage>
```

---

## 4. VisualStateManager (VSM)

```xml
<Button Text="Натисни мене">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup x:Name="CommonStates">
            <VisualState x:Name="Normal">
                <VisualState.Setters>
                    <Setter Property="Scale" Value="1.0" />
                    <Setter Property="BackgroundColor" Value="#512BD4" />
                </VisualState.Setters>
            </VisualState>
            <VisualState x:Name="Pressed">
                <VisualState.Setters>
                    <Setter Property="Scale" Value="0.95" />
                    <Setter Property="BackgroundColor" Value="#3B1F9E" />
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
</Button>
```
