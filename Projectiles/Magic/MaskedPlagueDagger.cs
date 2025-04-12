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
	public class MaskedPlagueDagger : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(2);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 3;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ArmorPenetration = 15;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255)
            {
				Projectile.Kill();
			}
			if (Projectile.penetrate <= 0)
			{
				Projectile.alpha += 5;
				Projectile.velocity *= 0.85f;
			}
			else
				Projectile.velocity *= 0.95f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.statMana += 1;
			for (int i = 0; i < 5; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 30, default(Color), 1f);
				dustId.noGravity = true;
				Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 30, default(Color), 1.5f);
				dustId3.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;

			Color color26 = new Color(116, 234, 204, 125) * Projectile.Opacity;

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					null, color27, Projectile.rotation, origin2, new Vector2(Projectile.scale * 0.5f, Projectile.scale * 1.25f), effects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int i = 0; i < 2; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.75f, Projectile.velocity.Y * 0.75f, 30, default(Color), 1f);
				dustId.noGravity = true;
				Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 30, default(Color), 1.5f);
				dustId3.noGravity = true;
			}
		}
	}
}