namespace Gardener.gameClasses.Rendering
{
    // Хранит и загружает текстуры для рендеринга
    public class TextureRegistry
    {
        private readonly TextureManager _textureManager; // Менеджер текстур

        // Блок: Идентификаторы текстур
        public int WaterTextureId { get; private set; }
        public int GrassTextureId { get; private set; }
        public int TropaTextureId { get; private set; }
        public int BushTextureId { get; private set; }
        public int BrevnoTextureId { get; private set; }
        public int BarrelTextureId { get; private set; }
        public int PondTextureId { get; private set; }
        public int OnlyBushTextureId { get; private set; }
        public int OnlyBrevnoTextureId { get; private set; }
        public int OnlyBarrelTextureId { get; private set; }
        public int OnlyPondTextureId { get; private set; }
        public int FenceTextureId { get; private set; }
        public int Table1TextureId { get; private set; }
        public int Table2TextureId { get; private set; }
        public int TreeTextureId { get; private set; }
        public int FruitShadowTextureId { get; private set; }
        public int BoxOfApplesTextureId { get; private set; }
        public int BoxOfPearsTextureId { get; private set; }
        public int AppleTextureId { get; private set; }
        public int PearTextureId { get; private set; }
        public int DropIconId { get; private set; }
        public int Level1FertilizerIconId { get; private set; }
        public int Level2FertilizerIconId { get; private set; }
        public int Level3FertilizerIconId { get; private set; }
        public int FungusIconId { get; private set; }
        public int CrossedOutInsectIconId { get; private set; }
        public int VirusIconId { get; private set; }
        public int SyringeIconId { get; private set; }
        public int AppleCharacter1PanelId { get; private set; }
        public int AppleCharacter2PanelId { get; private set; }
        public int AppleCharacter3PanelId { get; private set; }
        public int PearCharacter1PanelId { get; private set; }
        public int PearCharacter2PanelId { get; private set; }
        public int PearCharacter3PanelId { get; private set; }
        public int WateringCanTextureId { get; private set; }
        public int FertilizerLevel1TextureId { get; private set; }
        public int FertilizerLevel2TextureId { get; private set; }
        public int FertilizerLevel3TextureId { get; private set; }
        public int InsectSprayTextureId { get; private set; }
        public int FungusSprayTextureId { get; private set; }
        public int VirusSprayTextureId { get; private set; }
        public int SyringeTextureId { get; private set; }
        public int AppleIconTextureId { get; private set; }
        public int PearIconTextureId { get; private set; }
        public int EmptyIconTextureId { get; private set; }
        public int[] NumberTextureIds { get; private set; }
        public int ThreeStartTextureId { get; private set; }
        public int TwoStartTextureId { get; private set; }
        public int OneStartTextureId { get; private set; }
        public int GoStartTextureId { get; private set; }
        public int WinnerTextureId { get; private set; }
        public int DownRightJewTextureId { get; private set; }
        public int DownRightBlackTextureId { get; private set; }
        public int DownRightJapanTextureId { get; private set; }
        public int GlavnoeMenuTextureId { get; private set; }
        public int Player1TextureId { get; private set; }
        public int Player2TextureId { get; private set; }

        // Конструктор: Инициализирует реестр текстур
        public TextureRegistry(TextureManager textureManager)
        {
            _textureManager = textureManager ?? throw new ArgumentNullException(nameof(textureManager));
            NumberTextureIds = new int[8];
            LoadTextures();
        }

        // Загружает все текстуры из файлов
        private void LoadTextures()
        {
            WaterTextureId = _textureManager.LoadTexture("Assets/Game/general/tileWater.png");
            GrassTextureId = _textureManager.LoadTexture("Assets/Game/general/tileGrass.png");
            TropaTextureId = _textureManager.LoadTexture("Assets/Game/general/tropaOsnova.png");
            BushTextureId = _textureManager.LoadTexture("Assets/Game/level1/bush.png");
            BrevnoTextureId = _textureManager.LoadTexture("Assets/Game/level1/brevno.png");
            BarrelTextureId = _textureManager.LoadTexture("Assets/Game/level2/barrel.png");
            PondTextureId = _textureManager.LoadTexture("Assets/Game/level2/pond.png");
            OnlyBushTextureId = _textureManager.LoadTexture("Assets/Game/level1/onlyBush.png");
            OnlyBrevnoTextureId = _textureManager.LoadTexture("Assets/Game/level1/onlyBrevno.png");
            OnlyPondTextureId = _textureManager.LoadTexture("Assets/Game/level2/onlyPond.png");
            OnlyBarrelTextureId = _textureManager.LoadTexture("Assets/Game/level2/onlyBarrel.png");
            FenceTextureId = _textureManager.LoadTexture("Assets/Game/general/fence.png");
            Table1TextureId = _textureManager.LoadTexture("Assets/Game/level1/table1.png");
            Table2TextureId = _textureManager.LoadTexture("Assets/Game/level2/table2.png");
            TreeTextureId = _textureManager.LoadTexture("Assets/Game/general/tree.png");
            FruitShadowTextureId = _textureManager.LoadTexture("Assets/Game/general/fruitShadow.png");
            BoxOfApplesTextureId = _textureManager.LoadTexture("Assets/Game/level1/boxOfApplesTile.png");
            BoxOfPearsTextureId = _textureManager.LoadTexture("Assets/Game/level2/boxOfPearsTile.png");
            AppleTextureId = _textureManager.LoadTexture("Assets/Game/general/apple.png");
            PearTextureId = _textureManager.LoadTexture("Assets/Game/general/pear.png");
            DropIconId = _textureManager.LoadTexture("Assets/Game/icons/drop.png");
            Level1FertilizerIconId = _textureManager.LoadTexture("Assets/Game/icons/1levelFertilizer.png");
            Level2FertilizerIconId = _textureManager.LoadTexture("Assets/Game/icons/2levelFertilizer.png");
            Level3FertilizerIconId = _textureManager.LoadTexture("Assets/Game/icons/3levelFertilizer.png");
            FungusIconId = _textureManager.LoadTexture("Assets/Game/icons/fungus.png");
            CrossedOutInsectIconId = _textureManager.LoadTexture("Assets/Game/icons/crossedOutInsect.png");
            VirusIconId = _textureManager.LoadTexture("Assets/Game/icons/virus.png");
            SyringeIconId = _textureManager.LoadTexture("Assets/Game/icons/syringe.png");
            AppleCharacter1PanelId = _textureManager.LoadTexture("Assets/Game/UI/appleCharacter1Panel.png");
            AppleCharacter2PanelId = _textureManager.LoadTexture("Assets/Game/UI/appleСharacter2Panel.png");
            AppleCharacter3PanelId = _textureManager.LoadTexture("Assets/Game/UI/appleCharacter3Panel.png");
            PearCharacter1PanelId = _textureManager.LoadTexture("Assets/Game/UI/pearСharacter1Panel.png");
            PearCharacter2PanelId = _textureManager.LoadTexture("Assets/Game/UI/pearCharacter2Panel.png");
            PearCharacter3PanelId = _textureManager.LoadTexture("Assets/Game/UI/pearCharacter3Panel.png");
            WateringCanTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/wateringCanForWood.png");
            FertilizerLevel1TextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/fertilizer_1level.png");
            FertilizerLevel2TextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/fertilizer_2level.png");
            FertilizerLevel3TextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/fertilizer_3level.png");
            InsectSprayTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/insectSpray.png");
            FungusSprayTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/flungusSpray.png");
            VirusSprayTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/virusSpray.png");
            SyringeTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/SyringeWithInscriptions.png");
            AppleIconTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/appleIcon.png");
            PearIconTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/pearIcon.png");
            EmptyIconTextureId = _textureManager.LoadTexture("Assets/Game/iconsForPanel/emptyIcon.png");

            for (int i = 0; i < 8; i++)
            {
                NumberTextureIds[i] = _textureManager.LoadTexture($"Assets/Game/iconsForPanel/button/{i + 1}.png");
            }

            ThreeStartTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/3Start.png");
            TwoStartTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/2Start.png");
            OneStartTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/1Start.png");
            GoStartTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/goStart.png");
            WinnerTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/winner.png");
            DownRightJewTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/downRightJew.png");
            DownRightBlackTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/downRightBlack.png");
            DownRightJapanTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/downRightJapan.png");
            GlavnoeMenuTextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/glavnoeMenu.png");
            Player1TextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/player1.png");
            Player2TextureId = _textureManager.LoadTexture("Assets/Game/startAndEnd/player2.png");
        }
    }
}