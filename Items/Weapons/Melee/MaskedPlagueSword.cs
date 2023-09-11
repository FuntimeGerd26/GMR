using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class MaskedPlagueSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Sword");
			Tooltip.SetDefault("Shoots swords from the sky");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.width = 88;
			Item.height = 88;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.rare = 4;
			Item.value = Item.sellPrice(silver: 110);
			Item.DamageType = DamageClass.Melee;
			Item.crit = 14;
			Item.knockBack = 3.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.MaskedPlagueGigantSword>();
			Item.shootSpeed = 6f;
			Item.autoReuse = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(1))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 61);
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (velocity.Y > 0f)
			{
				velocity.Y *= -1;
			}

			for (int i = 0; i < 2; i++)
			{
				position = Main.MouseWorld;
				position.Y = player.Center.Y - 700 - Main.rand.Next(150);
				Vector2 heading = new Vector2(0f, -velocity.Y);
				heading = heading.RotatedByRandom(MathHelper.ToRadians(7));
				heading.Normalize();
				heading *= velocity.Length();
				Projectile.NewProjectile(source, position, heading, ModContent.ProjectileType<Projectiles.Melee.MaskedPlagueGigantSword>(), damage, knockback, player.whoAmI);
			}

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 12); // Iron/Lead
			recipe.AddIngredient(ItemID.Feather, 18);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.AddIngredient(ItemID.Bone, 30);
			recipe.AddRecipeGroup("GMR:AnyGem", 7);
			recipe.AddIngredient(ItemID.SoulofNight, 14);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}