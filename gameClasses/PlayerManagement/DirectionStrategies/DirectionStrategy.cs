namespace Gardener.gameClasses.PlayerManagement.DirectionStrategies
{
    // Стратегия определения направления движения
    public class DirectionStrategy : IDirectionStrategy
    {

        private readonly List<(int DirectionIndex, Func<float, float, bool> Condition)> _strategies; // Список стратегий направлений

        // Инициализирует стратегии направлений
        public DirectionStrategy()
        {
            _strategies = new List<(int, Func<float, float, bool>)>
            {
                (0, (dx, dy) => dx == 0 && dy > 0),  // Вниз
                (1, (dx, dy) => dx > 0 && dy > 0),   // Вниз-вправо
                (2, (dx, dy) => dx > 0 && dy == 0),  // Вправо
                (3, (dx, dy) => dx > 0 && dy < 0),   // Вверх-вправо
                (4, (dx, dy) => dx == 0 && dy < 0),  // Вверх
                (5, (dx, dy) => dx < 0 && dy < 0),   // Вверх-влево
                (6, (dx, dy) => dx < 0 && dy == 0),  // Влево
                (7, (dx, dy) => dx < 0 && dy > 0)    // Вниз-влево
            };
        }

        // Свойства - индекс направления
        public int DirectionIndex
        {
            get
            {
                foreach (var strategy in _strategies)
                {
                    if (strategy.Condition != null)
                    {
                        return strategy.DirectionIndex;
                    }
                }
                return 0; // По умолчанию вниз
            }
        }

        // Проверяет, соответствует ли направление заданным смещениям
        public bool Matches(float dx, float dy)
        {
            foreach (var strategy in _strategies)
            {
                if (strategy.Condition(dx, dy))
                {
                    return true;
                }
            }
            return false;
        }

        // Возвращает стратегию для заданных смещений
        public IDirectionStrategy GetStrategy(float dx, float dy)
        {
            foreach (var strategy in _strategies)
            {
                if (strategy.Condition(dx, dy))
                {
                    return new DirectionStrategyWrapper(strategy.DirectionIndex, strategy.Condition);
                }
            }
            return new DirectionStrategyWrapper(0, _strategies[0].Condition);
        }

        // Внутренний класс-обёртка для стратегии направления
        private class DirectionStrategyWrapper : IDirectionStrategy
        {
            private readonly int _directionIndex; // Индекс направления
            private readonly Func<float, float, bool> _condition; // Условие направления

            // Инициализирует обёртку с заданным направлением и условием
            public DirectionStrategyWrapper(int directionIndex, Func<float, float, bool> condition)
            {
                _directionIndex = directionIndex;
                _condition = condition;
            }

            public int DirectionIndex => _directionIndex;

            // Проверяет соответствие смещений
            public bool Matches(float dx, float dy) => _condition(dx, dy);
        }
    }
}