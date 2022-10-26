using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic
{
	public class MaskedPlagueStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Staff");
			Tooltip.SetDefault("Shoots 3 plague bolts that inflict poison");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 58;
			Item.rare = 3;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 16;
			Item.crit = 4;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.PlagueBolt>();
			Item.shootSpeed = 35f;
			Item.mana = 5;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(25);
			position += Vector2.Normalize(velocity);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
				Projectile.NewProjectile(Item.GetSource_FromThis(), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Magic.PlagueBolt>(), damage, knockback, player.whoAmI);
				type = 0;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 18);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.AddRecipeGroup("GMR:AnyGem", 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}