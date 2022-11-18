using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class SpazChargeBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazmatanium Charge Bow");
			Tooltip.SetDefault("'will take a little to charge'\nProjectiles have a chance to split into 3");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 50;
			Item.rare = 3;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.reuseDelay = 45;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 130);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 30;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.SpazArrow>();
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.SpazArrow>();
				SoundEngine.PlaySound(SoundID.Item5, player.position);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Lens, 6);
			recipe.AddIngredient(ItemID.IronBar, 25);
			recipe.AddIngredient(ItemID.Silk, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Lens, 6);
			recipe2.AddIngredient(ItemID.LeadBar, 25);
			recipe2.AddIngredient(ItemID.Silk, 12);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}