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
	public class FearShard : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 1200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.75f, 0.3f));

			var target = Projectile.FindTargetWithinRange(300f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 12f, 0.09f);
			}
			else
            {
				Projectile.velocity.Y += 0.3f;
				if (++Projectile.ai[0] % 15 == 0)
					Projectile.velocity.X = Main.rand.NextFloat(-7f, 7f);
            }

			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90f);
			
			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 114, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.2f, 30, default(Color), 1f);
			dustId.noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 1800);
		}

		public override void Kill(int timeleft)
		{
			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 114, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.2f, 30, default(Color), 1f);
			dustId.noGravity = true;
			Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 114, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.2f, 30, default(Color), 1.5f);
			dustId3.noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Color.White;
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}