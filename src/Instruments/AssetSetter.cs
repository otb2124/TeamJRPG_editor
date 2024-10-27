using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TeamJRPG_editor
{
    public class AssetSetter
    {


        public Image[][] sheets;


        public enum SheetCategory { tiles, character_bodies, mob_bodies, entity_icons, entity_icon_backgrounds, objects_decorative, objects_interractive, items, armor_bodies, battle_backgrounds };


        public AssetSetter() 
        {
            sheets = new Image[20][];
            for (int i = 0; i < sheets.Length; i++)
            {
                sheets[i] = new Image[10];
            }



            LoadMaps();
        }




        public void LoadMaps()
        {
            //tiles
            AddSheet(SheetCategory.tiles, 0, "tiles/tilemap_forest");

            //characterBodies
            AddSheet(SheetCategory.character_bodies, 0, "characters/bodies/body_male_spritesheet0");

            //characterDetails

            //mobBodies

            //entityIcons
            AddSheet(SheetCategory.entity_icons, 0, "characters/icons/icon0");

            //entityIconBackGrounds
            AddSheet(SheetCategory.entity_icon_backgrounds, 0, "characters/icons/iconbackgrounds");

            //objects_decorative
            AddSheet(SheetCategory.objects_decorative, 0, "objects/decorativeObjects/decorativeObjects");

            //objects_interractive
            AddSheet(SheetCategory.objects_interractive, 0, "objects/interractiveObjects/interractiveObjects");

            //items
            //weapons
            AddSheet(SheetCategory.items, 0, "items/weaponItems/tilemap_weapon_items");
            //armors
            AddSheet(SheetCategory.items, 1, "items/armorItems/tilemap_armor_items");
            //consumables
            AddSheet(SheetCategory.items, 2, "items/consumableItems/tilemap_consumable_items");
            //materials
            AddSheet(SheetCategory.items, 3, "items/materialItems/materialItem0");
            //valuables
            AddSheet(SheetCategory.items, 4, "items/valuableItems/valuableItem0");
            //questItems
            AddSheet(SheetCategory.items, 5, "items/questItems/questItem0");
            //currency
            AddSheet(SheetCategory.items, 6, "items/currencyItems/currencyItem0");

            //battleBackGrounds
            AddSheet(SheetCategory.battle_backgrounds, 0, "backgrounds/backgrounds_backgroundLayer");
            AddSheet(SheetCategory.battle_backgrounds, 1, "backgrounds/backgrounds_midgroundLayer");
            AddSheet(SheetCategory.battle_backgrounds, 2, "backgrounds/backgrounds_midbackgroundLayer");
            AddSheet(SheetCategory.battle_backgrounds, 3, "backgrounds/backgrounds_foregroundLayer");
        }





        public void AddSheet(SheetCategory cat, int id, string localPath)
        {
            sheets[SwitchSheetCategory(cat)][id] = GetImage(localPath + ".png");
        }


        public Image GetSheet(SheetCategory cat, int id)
        {
            return sheets[SwitchSheetCategory(cat)][id];
        }



        public Image GetImage(string path)
        {
            string imagePath = Path.Combine(Application.StartupPath, "res", path);
            if (File.Exists(imagePath))
            {
                return Image.FromFile(imagePath);
            }

            return null;
        }



        public int SwitchSheetCategory(SheetCategory cat)
        {
            switch (cat)
            {
                case SheetCategory.tiles:
                    return 0;
                case SheetCategory.character_bodies:
                    return 1;
                case SheetCategory.mob_bodies:
                    return 2;
                case SheetCategory.entity_icons:
                    return 3;
                case SheetCategory.objects_decorative:
                    return 4;
                case SheetCategory.objects_interractive:
                    return 5;
                case SheetCategory.items:
                    return 6;
                case SheetCategory.armor_bodies:
                    return 7;
                case SheetCategory.entity_icon_backgrounds:
                    return 8;
                case SheetCategory.battle_backgrounds:
                    return 9;
            }
            return -1;
        }
    }
}
