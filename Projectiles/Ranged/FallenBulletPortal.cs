using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;


namespace GMR.Projectiles.Ranged
{
    public class FallenBulletPortal : ModProjectile
    {
        public override string Texture => "GMR/Assets/Images/JackRitual";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(0);
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 0.1f;
        }

        float scale2;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0f, 0f));

            Player player = Main.player[Projectile.owner];
            if (player.dead || Projectile.owner == Main.myPlayer && !player.controlUseItem)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.owner == Main.myPlayer && player.controlUseItem)
            {
                Projectile.timeLeft = 2;
            }

            Projectile.Center = Main.MouseWorld;
            Projectile.rotation = 0f;
            player.heldProj = Projectile.whoAmI;
            /*player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 50;*/

            if (Projectile.scale <= 1f)
                Projectile.scale += 0.1f;
            if (Projectile.scale > 1.025f)
                Projectile.scale -= 0.025f;
            if (scale2 > 0f)
                scale2 -= 0.01f;

            var target = Projectile.FindTargetWithinRange(600f);
            if (target != null)
            {
                if (++Projectile.ai[0] % (player.itemAnimationMax * 4) == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 newVelocity = (Vector2.Normalize(target.Center - Projectile.Center) * 16f * (1f - Main.rand.NextFloat(-0.5f, 0.35f))).RotatedByRandom(MathHelper.ToRadians(7f));
                        // Silver Bullets look cool.
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity, 981, (int)(player.HeldItem?.damage * 1.2f), Projectile.knockBack, Main.myPlayer);
                        Main.projectile[p].tileCollide = false;
                    }
                    SoundEngine.PlaySound(SoundID.Item36, player.position);

                    Projectile.scale = 1.2f;
                    scale2 = 0.15f;
                    starRot = 180f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 60, default(Color), 1f);
                dustId.noGravity = true;
            }
        }

        float starRot;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Texture2D texture2 = ModContent.Request<Texture2D>($"GMR/Assets/Images/Circle", AssetRequestMode.ImmediateLoad).Value;
            Vector2 origin2 = texture2.Size() / 2f;
            Texture2D texture3 = ModContent.Request<Texture2D>($"GMR/Assets/Images/Star08", AssetRequestMode.ImmediateLoad).Value;
            Vector2 origin3 = texture2.Size() / 2f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color color26 = new Color(255, 0, 0, 255);
            if (starRot > 0f)
                starRot -= 5f;

            Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26 * 0.6f, Projectile.rotation, origin2, Projectile.scale * 0.4f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26, Projectile.rotation, origin, Projectile.scale * 0.4f, spriteEffects, 0);

            Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26 * 0.6f, Projectile.rotation,
                origin2, Projectile.scale * (0.25f + scale2), spriteEffects, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26, Projectile.rotation, origin, Projectile.scale * (0.25f + scale2), spriteEffects, 0);

            Main.EntitySpriteDraw(texture3, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color26 * (0.1f + (scale2 * 4f)), MathHelper.ToRadians(starRot),
                origin3, Projectile.scale * (0.35f + scale2), spriteEffects, 0);
            return false;
        }
    }
}