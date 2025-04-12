using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class PhoenixSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts 'Hellfire' to enemies\nHitting enemies causes an explosion that deals [c/FF4444:50%] damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 70;
			Item.rare = 2;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.buyPrice(silver: 125);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 30;
			Item.crit = 0;
			Item.knockBack = 2f;
			Item.useTurn = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(2))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
			}
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.SmallExplosion>()] < 1)
				Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, player.velocity, ModContent.ProjectileType<Projectiles.SmallExplosion>(), Item.damage / 2, Item.knockBack, Main.myPlayer);
			target.AddBuff(BuffID.OnFire3, 300);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 18); // Iron/Lead
			recipe.AddIngredient(ItemID.CrimstoneBlock, 45);
			recipe.AddRecipeGroup(0); // Any Bird
			recipe.AddIngredient(null, "UpgradeCrystal", 18);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddRecipeGroup("IronBar", 18); // Iron/Lead
			recipe2.AddIngredient(ItemID.EbonstoneBlock, 45);
			recipe2.AddRecipeGroup(0); // Any Bird
			recipe2.AddIngredient(null, "UpgradeCrystal", 18);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}