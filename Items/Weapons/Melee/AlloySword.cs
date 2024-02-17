using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class AlloySword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloy Metal Sword");
			Tooltip.SetDefault($" Right-click to throw a dagger that can randomly multiplicate when hitting an enemy");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.width = 68;
			Item.height = 68;
			Item.rare = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 100);
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.knockBack = 4f;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.noMelee = true;
				Item.UseSound = SoundID.Item7;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrow>();
				Item.shootSpeed = 8f;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.noMelee = false;
				Item.noUseGraphic = false;
				Item.shoot = 0;
				Item.UseSound = SoundID.Item1;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = position - 10 * Vector2.UnitY;
			if (player.altFunctionUse == 2)
				type = Main.rand.Next(new int[] { ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrowMultiplicate>(),
					ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrow>(), ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrow>() });
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdOldSword");
			recipe.AddIngredient(null, "AlloyDagger");
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(null, "AlloyBox");
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}