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

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class YinYangGuns : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(0);
        }

		public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 28;
            Item.rare = 2;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(silver: 75);
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item41;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 30;
            Item.crit = 6;
            Item.knockBack = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.shoot = ProjectileType<YinYangGunsHeld>();
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 1f);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 2f);
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "YinGun");
            recipe.AddIngredient(null, "YangGun");
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    // Disclaimer: I have no idea how this works, All code using this is possible thanks to Radiant Mod
    public class YinYangGunsHeld : ModProjectile, IDrawable
    {
        public int ChannelTime;
        public override string Texture => "GMR/Empty";
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
            Projectile.timeLeft = 30;
            if (Projectile.extraUpdates > 1 || Projectile.extraUpdates < 1)
                Projectile.extraUpdates = 1;
        }

        int bulletType;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemType<YinYangGuns>())
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

        Player Player => Main.player[Projectile.owner];

        int damage;
        private void ShootBullets(int bulletType, int amount, Vector2 from, Vector2 velocity)
        {
            if (Projectile.ai[0] == 1f)
                bulletType = ModContent.ProjectileType<Projectiles.Ranged.GungeonBulletFlip>();
            else if (Projectile.ai[0] == 2f)
                bulletType = ModContent.ProjectileType<Projectiles.Ranged.GungeonBullet>();

            damage = Projectile.damage;

            if (Player.GPlayer().YinEmpower == true && Projectile.ai[0] == 1f)
            {
                damage = damage * 2;
            }

            if (Player.GPlayer().YangEmpower == true && Projectile.ai[0] == 2f)
            {
                damage = damage * 2;
            }

            for (int i = 0; i < amount; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    from,
                    velocity,
                    bulletType,
                    damage,
                    Projectile.knockBack,
                    Projectile.owner);

                SoundEngine.PlaySound(SoundID.Item41, Projectile.Center);
            }
            MuzzleFlashAlpha = 1f;
        }

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

            if (Player.dead || !Player.ItemAnimationActive || Player.HeldItem.type != ItemType<YinYangGuns>())
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
                    ShootBullets(bulletType, bulletCount, MuzzlePosition, directionToMouse * 8f);

                    recoil += new Vector2(4f, Main.rand.NextFloat(0.05f, 0.2f));
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
            Texture2D texture = ModContent.Request<Texture2D>($"GMR/Items/Weapons/Ranged/Guns/YangGun", AssetRequestMode.ImmediateLoad).Value;
            if (Projectile.ai[0] == 1f)
                texture = ModContent.Request<Texture2D>($"GMR/Items/Weapons/Ranged/Guns/YinGun", AssetRequestMode.ImmediateLoad).Value;
            else if (Projectile.ai[0] == 2f)
                texture = ModContent.Request<Texture2D>($"GMR/Items/Weapons/Ranged/Guns/YangGun", AssetRequestMode.ImmediateLoad).Value;

            Vector2 normOrigin = new Vector2(-8f, 10f) + Vector2.UnitX * recoil.X;

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
                Color.Lerp(Color.Red, Color.Orange, MuzzleFlashAlpha) * MuzzleFlashAlpha,
                muzzleRot,
                muzzleOrigin,
                MuzzleSize(MuzzleFlashAlpha),
                SpriteEffects.None,
                0
                );
        }
    }
}