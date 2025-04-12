using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Books
{
	public class FearOfBeingForgotten : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.rare = 4;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 36;
			Item.crit = 0;
			Item.knockBack = 18f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.FearShard>();
			Item.shootSpeed = 24f;
			Item.mana = 20;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position.Y = player.Center.Y - 500;
			position.X = Main.MouseWorld.X;
			for (int i = 0; i < 4; i++)
			{
				Dust dustId = Dust.NewDustDirect(position, 5, 5, 6, 0f, -3f, 114, default(Color), 2f);
				dustId.noGravity = true;
				Dust dustId3 = Dust.NewDustDirect(position, 5, 5, 6, 0f, -3f, 114, default(Color), 2f);
				dustId3.noGravity = true;

				Projectile.NewProjectile(source, position, velocity * 0f, type, damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AncientInfraRedPlating", 40);
			recipe.AddIngredient(ItemID.SoulofNight, 25);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}