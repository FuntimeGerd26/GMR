using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.IO;
using Terraria.Audio;
using System;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR.Items.Weapons.Ranged
{
	public class JackRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Rifle");
			Tooltip.SetDefault($"'Your enemies wish they don't get hit by this'\n Has a chance to shoot an explosive projectile" +
                "\nInflicts 'Partially Crystalized' on enemies, deals massive damage over time\n[c/44FFAA:This item uses special code and might break]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 30;
			Item.rare = 6;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 140);
			Item.autoReuse = true;
            Item.UseSound = new SoundStyle("GMR/Sounds/Items/Ranged/BulletShot2") { Volume = 0.75f, PitchVariance = 0.2f, };
            Item.DamageType = DamageClass.Ranged;
			Item.damage = 30;
			Item.crit = 100;
			Item.knockBack = 5f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ProjectileType<JackRifleHeldProj>();
			Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.expert = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(
				source,
				position,
				velocity,
				Item.shoot,
				damage,
				knockback,
				player.whoAmI,
				1f
				);

			return false;
		}
    }


    // Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod
    public class JackRifleHeldProj : ModProjectile, IDrawable
    {
        public override string Texture => base.Texture.Replace("HeldProj", string.Empty);
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 999;
            if (Projectile.extraUpdates > 1 || Projectile.extraUpdates < 1)
                Projectile.extraUpdates = 1;
        }

        int bulletType;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemType<JackRifle>())
            {
                bulletType = ContentSamples.ItemsByType[itemSource.AmmoItemIdUsed].shoot;
            }
            else
            {
                Projectile.Kill();
            }
        }

        ref float MuzzleFlashAlpha => ref Projectile.ai[1];
        Vector2 MuzzlePosition => Projectile.Center + directionToMouse * 48f;

        private void ShootBullets(int bulletType, int amount, Vector2 from, Vector2 velocity)
        {
            for (int i = 0; i < amount; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    from,
                    velocity.RotatedByRandom(MathHelper.PiOver4 * 0.05f) * Main.rand.NextFloat(0.85f, 1.25f),
                    Main.rand.NextBool(10) ? ModContent.ProjectileType<Projectiles.Ranged.JackExplosiveBullet>() : ModContent.ProjectileType<Projectiles.Ranged.JackBullet>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner);

                Player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.27f);
            }

            MuzzleFlashAlpha = 1f;
        }

        Player Player => Main.player[Projectile.owner];

        Vector2 directionToMouse;
        Vector2 recoil;

        bool shotBullets;
        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded || Player.HeldItem.type != ItemType<JackRifle>())
            {
                Projectile.Kill();
                return;
            }

            if (Main.myPlayer == Player.whoAmI)
            {
                Vector2 shoulderPosition = Player.ShoulderPosition();
                directionToMouse = Player.ShoulderDirectionToMouse(ref shoulderPosition, 4f);
                Projectile.Center = shoulderPosition;

                if (!shotBullets)
                {
                    shotBullets = true;

                    int bulletCount = 1;
                    ShootBullets(bulletType, bulletCount, MuzzlePosition, directionToMouse * 12f);

                    recoil += new Vector2(8f, Main.rand.NextFloat(0, 0.2f));
                }
            }

            Projectile.rotation = directionToMouse.ToRotation() + recoil.Y * -Player.direction;

            Player.heldProj = Projectile.whoAmI;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            recoil *= 0.875f;
            MuzzleFlashAlpha *= 0.8f;
        }

        public override bool ShouldUpdatePosition() => false;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(directionToMouse);
            writer.WriteVector2(recoil);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            directionToMouse = reader.ReadVector2();
            recoil = reader.ReadVector2();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 normOrigin = new Vector2(14f, 10f) + Vector2.UnitX * recoil.X;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
                Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
                Projectile.scale,
                Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
                );

            return false;
        }

        DrawLayer IDrawable.DrawLayer => DrawLayer.BeforeProjectiles;
        public void Draw(Color lightColor)
        {
            Texture2D muzzleFlash = Request<Texture2D>("GMR/Assets/Images/MuzzleFlash", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 muzzleOrigin = muzzleFlash.Size() * 0.5f + Vector2.UnitX * 40;
            float muzzleRot = directionToMouse.ToRotation() + MathHelper.Pi;

            static Vector2 MuzzleSize(float flashAlpha) => 0.6f * new Vector2(1, 1 + MathF.Pow(flashAlpha, 2) * 0.4f) * flashAlpha;

            Main.EntitySpriteDraw(
                muzzleFlash,
                MuzzlePosition - Main.screenPosition,
                null,
                Color.Lerp(Color.Red, Color.Yellow, MuzzleFlashAlpha) * MuzzleFlashAlpha,
                muzzleRot,
                muzzleOrigin,
                MuzzleSize(MuzzleFlashAlpha),
                SpriteEffects.None,
                0
                );
        }
    }
}