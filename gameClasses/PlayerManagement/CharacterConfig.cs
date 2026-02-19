namespace Gardener.gameClasses.PlayerManagement
{
    // Хранит конфигурацию персонажа (спрайты, текстуры)
    public class CharacterConfig
    {
        // Блок: Данные персонажа
        public string FolderName { get; } // Папка с текстурами
        public (int Width, int Height, int SheetWidth)[] SpriteData { get; } // Размеры спрайтов

        // Конструктор: создаёт конфигурацию
        private CharacterConfig(string folderName, (int, int, int)[] spriteData)
        {
            FolderName = folderName;
            SpriteData = spriteData;
        }

        /// <summary>
        /// Возвращает конфигурацию персонажа по его ID.
        /// </summary>
        /// <param name="characterId">ID персонажа</param>
        /// <returns>Конфигурация персонажа</returns>
        public static CharacterConfig GetConfig(int characterId)
        {
            return characterId switch
            {
                1 => new CharacterConfig("jewCharacter", new (int, int, int)[8]
                {
                    (16, 28, 96), (18, 28, 108), (17, 28, 102), (16, 28, 96),
                    (16, 28, 96), (16, 28, 96), (17, 28, 102), (16, 28, 96)
                }),
                2 => new CharacterConfig("blackCharacter", new (int, int, int)[8]
                {
                    (16, 21, 96), (18, 21, 108), (17, 21, 102), (16, 22, 96),
                    (16, 22, 96), (16, 22, 96), (17, 21, 102), (18, 21, 108)
                }),
                3 => new CharacterConfig("japanCharacter", new (int, int, int)[8]
                {
                    (16, 22, 96), (18, 22, 108), (17, 22, 102), (16, 23, 96),
                    (16, 23, 96), (16, 23, 96), (17, 22, 102), (18, 22, 108)
                }),
                _ => new CharacterConfig("jewCharacter", new (int, int, int)[8]
                {
                    (16, 28, 96), (18, 28, 108), (17, 28, 102), (16, 28, 96),
                    (16, 28, 96), (16, 28, 96), (17, 28, 102), (16, 28, 96)
                })
            };
        }
    }
}