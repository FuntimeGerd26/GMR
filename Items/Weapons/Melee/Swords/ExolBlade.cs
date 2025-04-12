using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class ExolBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magma's Edge");
			Tooltip.SetDefault("Summons swords from the sides of your cursor");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.width = 54;
			Item.height = 106;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 280);
			Item.DamageType = DamageClass.Melee;
			Item.crit = 0;
			Item.knockBack = 8f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.MagmaEdge>();
			Item.shootSpeed = 18f;
			Item.autoReuse = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(1))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float x = 450f;
			float y = 150f;
			int amount = 6;
			var posX = Main.MouseWorld.X;
			var posY = Main.MouseWorld.Y - y / 2f;
			float yAdd = y / (amount / 2);
			for (int i = 0; i < amount; i++)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), new Vector2(posX + x, posY + yAdd * i), new Vector2(-36f, 0f), type, Item.damage, 1f, Main.myPlayer);
				Projectile.NewProjectile(player.GetSource_FromThis(), new Vector2(posX - x, posY + yAdd * i), new Vector2(36f, 0f), type, Item.damage, 1f, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.Item45, player.Center);
			}
			return false;
		}
	}
}