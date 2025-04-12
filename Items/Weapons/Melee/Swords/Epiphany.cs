using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class Epiphany : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'The realization of it's acts awakened great power'\nShoots a projectile that slowly fades" +
                "\nInflicts 'Chilling Flames' and 'Thoughtful' to enemies\n[c/DD1166:--Special Melee Weapon--]");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(1);
            Item.AddElement(2);
        }

        public override void SetDefaults()
        {
            Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.EpiphanySlash>(28);
            Item.useTime /= 2;
            Item.SetWeaponValues(66, 8f, 4);
            Item.width = 84;
            Item.height = 84;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = 5;
            Item.autoReuse = true;
            Item.reuseDelay = 5;
        }

        public override bool MeleePrefix()
        {
            return true;
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "AluminiumSword");
            recipe.AddIngredient(null, "JackSword");
            recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
            recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
            recipe.AddIngredient(ItemID.SoulofSight, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}