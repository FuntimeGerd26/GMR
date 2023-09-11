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
	public class InfraRedCrate : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Crate");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 4;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			if (++Projectile.localAI[0] > 60)
			{
				if (Projectile.velocity.Y > 0f || Projectile.velocity.Y < 0f)
				{
					Projectile.velocity.Y = 0f;
				}
				if (Projectile.velocity.X > 0f || Projectile.velocity.X < 0f)
				{
					Projectile.velocity.X = 0f;
				}
				Projectile.rotation = 0f + MathHelper.ToRadians(-90f); ;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			float numberProjectiles = 4;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = Vector2.UnitX.RotatedBy(2 * Math.PI / numberProjectiles * i) * 2f;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Magic.JackStaffProj>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			}
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * 0.7f, 30, Color.White, 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
}