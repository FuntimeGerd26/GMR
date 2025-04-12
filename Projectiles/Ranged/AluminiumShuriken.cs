using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
	public class AluminiumShuriken : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Ranged/Others/AluminiumShuriken";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Shuriken");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			Projectile.AddElement(1);
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 1200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			Projectile.rotation += 0.15f;
			if (++Projectile.localAI[0] == 15)
            {
				Projectile.damage += 3;
				Projectile.localAI[0] = 0;
            }
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
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			Projectile.width = Projectile.height = 26;
			Projectile.position.X -= (float)(Projectile.width / 2);
			Projectile.position.Y -= (float)(Projectile.height / 2);
		}
	}
}