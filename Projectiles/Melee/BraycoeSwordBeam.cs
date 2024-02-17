using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class BraycoeSwordBeam : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/BraycoeSword";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			Projectile.AddElement(0);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 1.1f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 15;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			if (Projectile.alpha > 40 && Projectile.scale > 1f)
            {
				Projectile.scale += -0.001f;
			}

			if (++Projectile.ai[2] % 30 == 0)
				Projectile.velocity *= 0.2f;
			Projectile.velocity *= 1.025f;

			Projectile.alpha += 4;
			if (Projectile.alpha > 255)
            {
                Projectile.Kill();
			}

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.5f, 60, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];

			Vector2 projPos = new Vector2(target.Center.X + Main.rand.Next(-30, 30) * 2, target.Center.Y + Main.rand.Next(-30, 30) * 2);
			Vector2 velocityToTarget = ((new Vector2(target.Center.X, target.Center.Y)) - projPos) * 0.15f;
			Projectile.NewProjectile(player.GetSource_FromThis(), projPos, velocityToTarget,
				ModContent.ProjectileType<CoolSwords.BraycoeExplosion>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.5f, 60, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			var Opacity = Projectile.Opacity;

			Color color26 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
					color27 * 0.5f * Opacity, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
				color26 * 0.85f * Opacity, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			return false;
		}
	}
}