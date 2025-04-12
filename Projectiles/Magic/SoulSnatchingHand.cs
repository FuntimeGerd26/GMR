using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Magic
{
	public class SoulSnatchingHand : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 6;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			Projectile.AddElement(-1);
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.98f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 velocity = (Vector2.UnitY * Main.rand.NextFloat(8f, 12f)).RotatedByRandom(MathHelper.ToRadians(360f));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Vector2.Normalize(velocity * 2f),
					velocity, ModContent.ProjectileType<SnatchedSoul>(), (int)(Projectile.damage / 3), Projectile.knockBack, Main.myPlayer);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = TextureAssets.Projectile[Type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;

			Color color26 = Color.White;

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					null, color27, num165, origin2, Projectile.scale, effects, 0);
			}
			return false;
		}
	}
}