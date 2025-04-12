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
	public class DreamingSiltcurrentProj : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_927";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0f, 1f, 0.85f));
			Projectile.velocity.Y *= 1.01f;
			if (++Projectile.ai[0] % 15 == 0)
				Projectile.velocity.X = Main.rand.NextFloat(-8f, 8f);
			if (Projectile.velocity.Y < 1f)
				Projectile.velocity.Y = 12f;

			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90f);
			
			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 156, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.2f, 30, default(Color), 1f);
			dustId.noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Dust dustId = Dust.NewDustDirect(target.position, target.width, target.height, 156, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 30, default(Color), 1f);
			dustId.noGravity = true;
			Dust dustId3 = Dust.NewDustDirect(target.position, target.width, target.height, 156, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 30, default(Color), 1.5f);
			dustId3.noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = TextureAssets.Projectile[Type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;
			var opacity = Projectile.Opacity;
			Color color26 = new Color(55, 255, 255, 100) * opacity;

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
				color26, Projectile.rotation + MathHelper.ToRadians(90f), origin2, new Vector2(Projectile.scale * 1f, Projectile.scale * 0.5f), SpriteEffects.None, 0);
			return false;
		}
	}
}