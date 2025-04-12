using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Bows
{
	public class SpazChargeBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazmatanium Charge Bow");
			Tooltip.SetDefault($"'Dosen't actually charge, it's already charged'\nShoots 2 Projectiles in a row\n Replaces wooden arrows for a special projectile that splits into 3");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 66;
			Item.rare = 2;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 130);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 25;
			Item.crit = 4;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				velocity *= 2;
				type = ModContent.ProjectileType<Projectiles.Ranged.SpazArrow>();
				SoundEngine.PlaySound(SoundID.Item5, player.position);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AluminiumBow");
			recipe.AddRecipeGroup("IronBar", 25); // Iron/Lead
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}