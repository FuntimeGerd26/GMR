using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class BrokenMagmaticSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 52;
			Item.rare = 2;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 95);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 34;
			Item.crit = 0;
			Item.knockBack = 2.25f;
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
			Vector2 ProjVel = new Vector2(Main.rand.NextFloat(-3f, 3f), 24f);
			Vector2 PositionOfFireball = new Vector2(target.Center.X + Main.rand.Next(-target.width / 2, target.width / 2), target.Center.Y - 400);
			Projectile.NewProjectile(player.GetSource_FromThis(), PositionOfFireball,
				ProjVel, ModContent.ProjectileType<Projectiles.Melee.MagmaticFireball>(), Item.damage, Item.knockBack, Main.myPlayer);

			// Projectile Spawn Dust for visuals
			for (int i = 0; i < 10; i++)
			{
				Dust dustId = Dust.NewDustDirect(PositionOfFireball, 5, 5, 6, 0f, -ProjVel.Y * 1.5f, 60, default(Color), 1f);
				dustId.noGravity = true;

				Dust dustId2 = Dust.NewDustDirect(PositionOfFireball, 5, 10, 6, ProjVel.X * 2.5f, ProjVel.Y * 0.75f, 60, default(Color), 1.5f);
				dustId2.noGravity = true;

				Dust dustId3 = Dust.NewDustDirect(PositionOfFireball, 5, 10, 6, -ProjVel.X * 2.5f, ProjVel.Y * 0.75f, 60, default(Color), 1.5f);
				dustId3.noGravity = true;
			}
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Generic) += 0.07f;
		}
	}
}