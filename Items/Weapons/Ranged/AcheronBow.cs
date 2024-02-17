using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AcheronBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acheron Bow");
			Tooltip.SetDefault($"'The parts are more useful like this than being a shield'\n Shoots a spread of 3 arrows, and an explosive bullet");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 50;
			Item.rare = 4;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 210);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 58;
			Item.crit = 10;
			Item.knockBack = 2.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity *= 4;
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(25);
			position += Vector2.Normalize(velocity);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
				Projectile.NewProjectile(Item.GetSource_FromThis(), position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.JackExplosiveBullet>(), damage * 2, knockback, player.whoAmI);
			SoundEngine.PlaySound(SoundID.Item5, player.position);
		}
	}
}