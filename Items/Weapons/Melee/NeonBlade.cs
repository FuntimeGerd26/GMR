using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class NeonBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Hitting an enemy will create an orb that will chase enemies after half second\n'1000 degrees knife'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 48;
			Item.rare = 4;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 70);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 25;
			Item.crit = 4;
			Item.knockBack = 3f;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(1))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 60);
			}
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(player.direction * -3f, 0f), ModContent.ProjectileType<Projectiles.NeonOrb>(), Item.damage, Item.knockBack, Main.myPlayer);
			target.AddBuff(BuffID.OnFire, 3000);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GoldBar, 18);
			recipe.AddIngredient(ItemID.CrimstoneBlock, 28);
			recipe.AddIngredient(ItemID.Ruby, 4);
			recipe.AddIngredient(null, "UpgradeCrystal", 40);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.GoldBar, 18);
			recipe2.AddIngredient(ItemID.EbonstoneBlock, 28);
			recipe2.AddIngredient(ItemID.Ruby, 4);
			recipe2.AddIngredient(null, "UpgradeCrystal", 40);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.PlatinumBar, 18);
			recipe3.AddIngredient(ItemID.CrimstoneBlock, 28);
			recipe3.AddIngredient(ItemID.Ruby, 4);
			recipe3.AddIngredient(null, "UpgradeCrystal", 40);
			recipe3.AddTile(TileID.Anvils);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.PlatinumBar, 18);
			recipe4.AddIngredient(ItemID.EbonstoneBlock, 28);
			recipe4.AddIngredient(ItemID.Ruby, 4);
			recipe4.AddIngredient(null, "UpgradeCrystal", 40);
			recipe4.AddTile(TileID.Anvils);
			recipe4.Register();
		}
	}
}