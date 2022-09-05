using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class XShotBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra-Blue Bow");
			Tooltip.SetDefault("Increases all ranged weapon speed by 5%\nHas a chance to shoot an ultra-blue energy bolt which splits in 3\nUpon hitting enemies the projectiles will double their speed");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 56;
			Item.rare = 6;
			Item.useTime = 20;
			Item.useAnimation = 25;
			Item.reuseDelay = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 120);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.XShotArrow>();
			Item.shootSpeed = 40f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Ranged) += 0.5f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = Main.rand.Next(new int[] { ModContent.ProjectileType<Projectiles.Ranged.XShotArrow>(), ModContent.ProjectileType<Projectiles.Ranged.XShotArrow>(), ModContent.ProjectileType<Projectiles.XShotEnergy>()});
				SoundEngine.PlaySound(SoundID.Item5, player.position);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CobaltBar, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 28);
			recipe.AddIngredient(ItemID.AdamantiteBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PalladiumBar, 16);
			recipe2.AddIngredient(ItemID.SoulofNight, 18);
			recipe2.AddIngredient(ItemID.TitaniumBar, 10);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.PalladiumBar, 16);
			recipe3.AddIngredient(ItemID.SoulofNight, 28);
			recipe3.AddIngredient(ItemID.AdamantiteBar, 10);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.CobaltBar, 16);
			recipe4.AddIngredient(ItemID.SoulofNight, 28);
			recipe4.AddIngredient(ItemID.TitaniumBar, 10);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}