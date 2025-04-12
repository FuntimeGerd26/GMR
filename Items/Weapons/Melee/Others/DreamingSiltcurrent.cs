using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Weapons.Melee.Others
{
	public class DreamingSiltcurrent : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 76;
			Item.height = 76;
			Item.rare = 7;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 380);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 80;
			Item.crit = 0;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.DreamingSiltcurrent>();
			Item.shootSpeed = 8f;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(1))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 156);
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 5; i++)
			{
				position.X = Main.MouseWorld.X + Main.rand.NextFloat(-80f, 80f);
				position.Y = player.Center.Y - 800 - Main.rand.NextFloat(0f, 200f);
				Vector2 heading = new Vector2(0f, 12f);
				heading = heading.RotatedByRandom(MathHelper.ToRadians(7));
				Projectile.NewProjectile(source, position, heading, ModContent.ProjectileType<Projectiles.Melee.DreamingSiltcurrentProj>(), (int)(damage * 0.8f), knockback, player.whoAmI);
			}

			type = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.DreamingSiltcurrent>();
			Projectile.NewProjectile(source, position, velocity * 0f, type, damage, 0f, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);

			return false;
		}
	}
}