using System;
using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class PrismaticSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prism Sniper");
			Tooltip.SetDefault($" Shoots a homing and piercing bullet that can pierce 5 enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(1);
            Item.AddElement(2);
        }

		public override void SetDefaults()
		{
			Item.width = 140;
			Item.height = 42;
			Item.rare = 8;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 140);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 88;
			Item.crit = 14;
			Item.knockBack = 3f;
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<PrismaticSniperHeldProj>();
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            Color color27 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 125), Color.Transparent, 0 / 3 + 0.3f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value, position, frame,
                Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value, position, frame,
                color27, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Player player = Main.LocalPlayer;
            Texture2D texture = ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value;
            Vector2 position = new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f);
            Color color27 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 125), Color.Transparent, 0 / 3 + 0.3f);

            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color27, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "PrismaticRifle");
            recipe.AddIngredient(null, "InfraRedBar", 24);
            recipe.AddIngredient(ItemID.Ectoplasm, 32);
            recipe.AddRecipeGroup("Wood", 28);
            recipe.AddIngredient(ItemID.Diamond, 6);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, 1f);
			return false;
		}
	}


    // Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod
    public class PrismaticSniperHeldProj : ModProjectile, IDrawable
    {
        public int ChannelTime;
        public override string Texture => base.Texture.Replace("HeldProj", string.Empty);
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 999;
            if (Projectile.extraUpdates > 0 || Projectile.extraUpdates < 0)
                Projectile.extraUpdates = 0;
        }

        int bulletType;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ItemType<PrismaticSniper>())
            {
                bulletType = ContentSamples.ItemsByType[itemSource.AmmoItemIdUsed].shoot;
            }
            else
            {
                Projectile.Kill();
            }
        }

        ref float MuzzleFlashAlpha => ref Projectile.ai[1];
        Vector2 MuzzlePosition => Projectile.Center + directionToMouse * 110f;

        private void ShootBullets(int bulletType, int amount, Vector2 from, Vector2 velocity)
        {
            for (int i = 0; i < amount; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    from,
                    velocity.RotatedByRandom(MathHelper.PiOver4 * 0.05f) * Main.rand.NextFloat(0.75f, 1.75f),
                    ModContent.ProjectileType<Projectiles.Ranged.PrismaticBullet>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner);

                SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
                Player.GetModPlayer<GerdPlayer>().ShakeScreen(4, 0.85f);
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

            if (!Player.controlUseItem || Player.HeldItem.type != ItemType<PrismaticSniper>())
            {
                Projectile.Kill();
                return;
            }

            if (Main.myPlayer == Player.whoAmI)
            {
                Vector2 shoulderPosition = Player.ShoulderPosition();
                directionToMouse = Player.ShoulderDirectionToMouse(ref shoulderPosition, 4f);
                Projectile.Center = shoulderPosition;

                float attackBuffs = (Player.GetAttackSpeed(DamageClass.Ranged) + Player.GetAttackSpeed(DamageClass.Generic)) * 0.5f;

                if (Player.controlUseItem)
                {
                    shotspeed = (int)(Player.HeldItem.useTime / attackBuffs);
                    if (shotspeed < 1)
                        shotspeed = 1;

                    if (++ChannelTime % shotspeed == 0) // Make it double because of Projectile.extraUpdates
                        shotBullets = false;
                }

                if (!shotBullets)
                {
                    shotBullets = true;

                    int bulletCount = 1;
                    ShootBullets(bulletType, bulletCount, MuzzlePosition, directionToMouse * 12f);

                    recoil += new Vector2(8f, Main.rand.NextFloat(0.25f, 0.6f));
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
            Vector2 normOrigin = new Vector2(40f, 20f) + Vector2.UnitX * recoil.X;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB),
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
                Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), MuzzleFlashAlpha) * MuzzleFlashAlpha,
                muzzleRot,
                muzzleOrigin,
                MuzzleSize(MuzzleFlashAlpha),
                SpriteEffects.None,
                0
                );
        }
    }
}