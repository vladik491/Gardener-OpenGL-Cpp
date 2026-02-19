namespace Gardener.gameClasses.Problems
{
    // Абстрактная фабрика для создания проблем
    public abstract class ProblemFactory
    {
        protected readonly Random _random;

        // Инициализирует фабрику с генератором случайных чисел
        protected ProblemFactory()
        {
            _random = new Random();
        }

        public abstract Problem CreateProblem(float currentTime);
    }

    // Фабрика проблем для яблони
    public class AppleTreeProblemFactory : ProblemFactory
    {
        // Вероятности проблем: Полив и Грибок - 60%, остальные - 40%
        private readonly (ProblemType type, double probability)[] _problemProbabilities =
        {
            (ProblemType.Watering, 0.214),    // 60 / 280
            (ProblemType.Fungus, 0.214),      // 60 / 280
            (ProblemType.Fertilizing, 0.143), // 40 / 280
            (ProblemType.Pests, 0.143),       // 40 / 280
            (ProblemType.Virus, 0.143),       // 40 / 280
            (ProblemType.Tending, 0.143)      // 40 / 280
        };

        // Создаёт проблему для яблони с учётом вероятностей
        public override Problem CreateProblem(float currentTime)
        {
            double roll = _random.NextDouble(); // Случайное число для выбора проблемы
            double cumulative = 0.0; // Накопленная вероятность

            foreach (var (type, probability) in _problemProbabilities)
            {
                cumulative += probability;
                if (roll <= cumulative)
                    return new Problem(type, currentTime, 30.0f); // 30 секунд на решение
            }

            return new Problem(ProblemType.Watering, currentTime, 30.0f);
        }
    }

    // Фабрика проблем для груши
    public class PearTreeProblemFactory : ProblemFactory
    {
        // Вероятности проблем: Ухаживание и Вредители - 60%, остальные - 40%
        private readonly (ProblemType type, double probability)[] _problemProbabilities =
        {
            (ProblemType.Tending, 0.214),     // 60 / 280
            (ProblemType.Pests, 0.214),       // 60 / 280
            (ProblemType.Watering, 0.143),    // 40 / 280
            (ProblemType.Fungus, 0.143),      // 40 / 280
            (ProblemType.Fertilizing, 0.143), // 40 / 280
            (ProblemType.Virus, 0.143)        // 40 / 280
        };

        // Создаёт проблему для груши с учётом вероятностей
        public override Problem CreateProblem(float currentTime)
        {
            double roll = _random.NextDouble();
            double cumulative = 0.0;

            foreach (var (type, probability) in _problemProbabilities)
            {
                cumulative += probability;
                if (roll <= cumulative)
                    return new Problem(type, currentTime, 30.0f); // 30 секунд на решение
            }

            return new Problem(ProblemType.Tending, currentTime, 30.0f);
        }
    }
}