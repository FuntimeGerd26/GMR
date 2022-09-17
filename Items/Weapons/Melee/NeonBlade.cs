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
			Tooltip.SetDefault("While hitting enemies will create mines which last for 10 seconds\n'1000 degrees knife'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 48;
			Item.rare = 5;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 70);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 44;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.scale = 1.5f;
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
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, player.velocity * 0, ModContent.ProjectileType<Projectiles.NeonMine>(), Item.damage, Item.knockBack, Main.myPlayer);
			target.AddBuff(BuffID.OnFire, 6000);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "VoidSword");
			recipe.AddIngredient(ItemID.OrichalcumBar, 18);
			recipe.AddIngredient(ItemID.SoulofSight, 14);
			recipe.AddIngredient(ItemID.Ruby, 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}