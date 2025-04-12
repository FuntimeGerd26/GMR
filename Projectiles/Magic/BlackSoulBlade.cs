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
	public class BlackSoulBlade : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_873";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 6;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			Projectile.AddElement(-1);
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(-1f, -1f, -1f));

			var target = Projectile.FindTargetWithinRange(2000f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 24f, 0.25f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.GetModPlayer<GerdPlayer>().ShakeScreen(3, 0.5f);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 30, newColor: Color.Black, 1f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, newColor: Color.Black, Scale: 1f);
				d.velocity *= 0.4f;
				d.velocity += Projectile.velocity * 0.5f;
				d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				d.scale *= Projectile.scale * 0.6f;
				d.fadeIn = d.scale + 0.1f;
				d.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(ProjectileID.FairyQueenLance);
			Texture2D texture2D13 = TextureAssets.Projectile[ProjectileID.FairyQueenLance].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;

			Color color26 = Color.Black;

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
					null, color27, num165, origin2, new Vector2(Projectile.scale * 1.25f, Projectile.scale * 0.5f), effects, 0);
			}
			return false;
		}
	}
}