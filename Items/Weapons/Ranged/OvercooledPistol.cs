using System;
using System.IO;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using GMR;

namespace GMR.Items.Weapons.Ranged
{
	public class OvercooledPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Shoots overcooled bullets");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(1);
            Item.AddElement(2);
        }

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 24;
			Item.rare = 1;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 140);
			Item.autoReuse = true;
            Item.UseSound = new SoundStyle("GMR/Sounds/Items/Ranged/BulletShot1") { Volume = 0.5f, PitchVariance = 0.2f, };
            Item.DamageType = DamageClass.Ranged;
			Item.damage = 15;
			Item.crit = 0;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ProjectileType<OvercooledPistolHeld>();
			Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
		}

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
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

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TheUndertaker);
            recipe.AddIngredient(ItemID.TissueSample, 14);
            recipe.AddIngredient(ItemID.IceBlock, 40);
            recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.Musket);
            recipe2.AddIngredient(ItemID.ShadowScale, 14);
            recipe2.AddIngredient(ItemID.IceBlock, 40);
            recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }

    // Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod
    public class OvercooledPistolHeld : ModProjectile, IDrawable
    {
        public int ChannelTime;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

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
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemType<OvercooledPistol>())
            {
                bulletType = ContentSamples.ItemsByType[itemSource.AmmoItemIdUsed].shoot;
            }
            else
            {
                Projectile.Kill();
            }
        }

        ref float MuzzleFlashAlpha => ref Projectile.ai[1];
        Vector2 MuzzlePosition => Projectile.Center + directionToMouse * 42f;

        private void ShootBullets(int bulletType, int amount, Vector2 from, Vector2 velocity)
        {
            for (int i = 0; i < amount; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    from,
                    velocity.RotatedByRandom(MathHelper.ToRadians(5f)),
                    ModContent.ProjectileType<Projectiles.Ranged.OvercooledBullet>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner);

                SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot1") { Volume = 0.5f, PitchVariance = 0.2f, }, Projectile.Center);
                Player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.25f);
            }

            MuzzleFlashAlpha = 1f;
        }

        Player Player => Main.player[Projectile.owner];

        Vector2 directionToMouse;
        Vector2 recoil;

        bool shotBullets;
        int shotspeed;
        public override void AI()
        {
            if (Main.MouseWorld.X < Player.Center.X)
                Player.direction = -1;
            else if (Main.MouseWorld.X > Player.Center.X)
                Player.direction = 1;

            if (Player.dead || !Player.controlUseItem || Player.HeldItem.type != ItemType<OvercooledPistol>())
            {
                Projectile.Kill();
                return;
            }

            if (Main.myPlayer == Player.whoAmI)
            {
                Vector2 shoulderPosition = Player.ShoulderPosition();
                directionToMouse = Player.ShoulderDirectionToMouse(ref shoulderPosition, 4f);
                Projectile.Center = shoulderPosition;

                float attackBuffs = (Player.GetAttackSpeed(DamageClass.Ranged) + Player.GetAttackSpeed(DamageClass.Generic)) * 1.1f;

                if (Player.controlUseItem)
                {
                    shotspeed = (int)(Player.HeldItem.useTime * 2 - (Player.HeldItem.useTime * attackBuffs));
                    if (shotspeed < 1)
                        shotspeed = 1;

                    if (++ChannelTime % shotspeed == 0)
                        shotBullets = false;
                }


                if (++Projectile.frameCounter % (int)(Player.HeldItem.useTime + (Player.HeldItem.useTime * attackBuffs)) == 0)
                {
                    Projectile.frame++;
                    if (Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.frame = 0;
                    }
                }

                if (!shotBullets)
                {
                    shotBullets = true;

                    int bulletCount = 1;
                    ShootBullets(bulletType, bulletCount, MuzzlePosition, directionToMouse * 18f);

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
            Vector2 normOrigin = new Vector2(8f, 8f) + Vector2.UnitX * recoil.X;

            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y3 = num156 * Projectile.frame;
            Rectangle rectangle = new Rectangle(0, y3, texture.Width, num156);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                new Microsoft.Xna.Framework.Rectangle?(rectangle),
                lightColor,
                Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
                Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
                Projectile.scale,
                Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
                );

            Main.EntitySpriteDraw(
                ModContent.Request<Texture2D>($"GMR/Items/Weapons/Ranged/OvercooledPistolHeld_Glow", AssetRequestMode.ImmediateLoad).Value,
                Projectile.Center - Main.screenPosition,
                new Microsoft.Xna.Framework.Rectangle?(rectangle),
                Color.White,
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
                Color.Lerp(Color.Blue, Color.Cyan, MuzzleFlashAlpha) * MuzzleFlashAlpha,
                muzzleRot,
                muzzleOrigin,
                MuzzleSize(MuzzleFlashAlpha),
                SpriteEffects.None,
                0
                );
        }
    }
}