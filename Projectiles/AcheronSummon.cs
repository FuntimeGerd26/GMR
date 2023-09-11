using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.NPCs.Bosses.Jack;

namespace GMR.Projectiles
{
	public class AcheronSummon : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/AcheronShuriken";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Drone");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 240;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			Projectile.rotation += 18f * 0.03f;
			if (++Projectile.ai[0] % 30 == 0)
				Projectile.velocity.Y += -0.5f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var texture = TextureAssets.Projectile[Type].Value;
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(255, 5, 5, 0) * progress * opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, new Color(255, 5, 5, 0) * opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

			var swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(255, 105, 105, 0) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;
			float swishRotation = Projectile.rotation;
			for (int i = 0; i < 1; i++)
			{
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);
			}

			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			// Trail funny
			float modifier = Projectile.localAI[1] * MathHelper.Pi / 45;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Player player = Main.player[Projectile.owner];
				Texture2D glow = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
				Color color27 = Color.Lerp(new Color(255, 105, 105, 0), Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				float scale = Projectile.scale - (float)Math.Cos(Projectile.ai[0]) / 5;
				scale *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = Math.Max((int)i - 1, 0);
				Vector2 center = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], (1 - i % 1));
				float smoothtrail = i % 1 * (float)Math.PI / 3.45f;
				bool withinangle = Projectile.rotation > -Math.PI / 2 && Projectile.rotation < Math.PI / 2;
				if (withinangle && player.direction == 1)
					smoothtrail *= -1;
				else if (!withinangle && player.direction == -1)
					smoothtrail *= -1;
				center += Projectile.Size / 2;

				Vector2 offset = (Projectile.Size / 12).RotatedBy(Projectile.oldRot[(int)i] - smoothtrail * (-Projectile.direction));
				Main.spriteBatch.Draw(glow, center - offset - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity, Projectile.rotation, glow.Size() / 2, scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Player player= Main.player[Projectile.owner];
			if (!NPC.AnyNPCs(ModContent.NPCType<Acheron>()))
			{
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<Acheron>();
				int spawnX = (int)player.position.X + player.width / 2;
				int spawnY = (int)player.position.Y + player.height / 2 - 700;
				NPC.NewNPC(player.GetSource_FromThis(), spawnX, spawnY, type, player.whoAmI, -1f, -1f, -1f, -1f);
				Projectile.netUpdate = true;
			}
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * -0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId3].noGravity = true;
		}
	}
}