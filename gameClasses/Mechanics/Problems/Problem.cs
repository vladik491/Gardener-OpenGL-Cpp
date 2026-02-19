namespace Gardener.gameClasses.Problems
{
    // Проблема дерева
    public class Problem
    {
        public ProblemType Type { get; }
        public float SpawnTime { get; }
        public float TimeLeft { get; set; }

        // Создаёт проблему с заданным типом, временем появления и лимитом времени
        public Problem(ProblemType type, float spawnTime, float timeLimit)
        {
            Type = type;
            SpawnTime = spawnTime;
            TimeLeft = timeLimit;
        }
    }
}