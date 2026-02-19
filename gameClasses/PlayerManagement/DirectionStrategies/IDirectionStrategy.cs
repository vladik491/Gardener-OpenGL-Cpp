namespace Gardener.gameClasses.PlayerManagement.DirectionStrategies
{
    /// <summary>
    /// Интерфейс для стратегий определения направления движения
    /// </summary>
    public interface IDirectionStrategy
    {
        // Свойства - характеристики направления
        int DirectionIndex { get; }
        bool Matches(float dx, float dy);
    }
}