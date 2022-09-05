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
			Tooltip.SetDefault("Right-click to throw a dagger that can randomly multiplicate when hitting an enemy");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.width = 56;
			Item.height = 56;
			Item.rare = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 100);
			Item.DamageType = DamageClass.Melee;
			Item.crit = 4;
			Item.knockBack = 2f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 30;
				Item.useTime = 12;
				Item.useAnimation = 12;
				Item.autoReuse = false;
				Item.noMelee = true;
				Item.UseSound = SoundID.Item7;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrow>();
				Item.shootSpeed = 6f;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.damage = 35;
				Item.useTime = 16;
				Item.useAnimation = 16;
				Item.autoReuse = true;
				Item.noMelee = false;
				Item.noUseGraphic = false;
				Item.UseSound = SoundID.Item1;
				Item.shoot = 0;
				Item.scale = 1.5f;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			type = Main.rand.Next(new int[] { ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrowMultiplicate>(), ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrow>(), ModContent.ProjectileType<Projectiles.Melee.AlloySwordThrow>() });
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdOldSword");
			recipe.AddIngredient(null, "AlloyDagger");
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}