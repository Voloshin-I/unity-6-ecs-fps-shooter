# Code Style Guide

## Модификаторы доступа

- Все приватные методы имеют явный модификатор `private`
- Публичные поля, нужные только для сериализации → приватные с `[SerializeField]`

## Именование полей

- Все приватные поля начинаются с `_`

## Порядок членов класса

1. Сериализованные приватные поля (`[SerializeField] private ...`)
2. Публичные поля/свойства
3. Методы (Unity lifecycle, public, private)
4. Несериализованные приватные поля (в конце класса)

## Константы

- Все численные константы объявляются как `private const`
- Никаких "магических чисел" внутри кода

## Пример

```csharp
public class ExampleAuthoring : MonoBehaviour
{
    // Сериализованные поля — в начале
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _maxHealth = 100;

    // Публичные свойства
    public float MoveSpeed => _moveSpeed;

    // Константы
    private const float MinSpeed = 0.1f;
    private const int DefaultDamage = 10;

    // Unity lifecycle
    private void Awake()
    {
        Initialize();
    }

    // Публичные методы
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
    }

    // Приватные методы
    private void Initialize()
    {
        _currentHealth = _maxHealth;
    }

    // Несериализованные приватные поля — в конце
    private int _currentHealth;
    private bool _isInitialized;
}
```


