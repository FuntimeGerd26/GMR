using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class ArerniasRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 76;
			Item.height = 26;
			Item.rare = 7;
			Item.useTime = 7;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 360);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 28;
			Item.crit = 6;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ArerniasBullet>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-24, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 8;
			type = ModContent.ProjectileType<Projectiles.Ranged.ArerniasBullet>();
			SoundEngine.PlaySound(SoundID.Item11, player.Center);
		}
	}
}