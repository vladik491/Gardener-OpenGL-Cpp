namespace Gardener.gameClasses.ScreenManagement
{
    // Интерфейс для экранов игры
    public interface IScreen
    {
        // Обновляет состояние экрана
        void Update(float deltaTime);

        // Рендерит экран
        void Render(int width, int height, int playerId);

        // Свойство: Активен ли экран
        bool IsActive { get; }
    }
}