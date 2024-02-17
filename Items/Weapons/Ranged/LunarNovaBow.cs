using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class LunarNovaBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 94;
			Item.rare = 4;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 120);
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 18;
			Item.crit = 2;
			Item.knockBack = 1f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.LunarNovaArrow>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.LunarNovaArrow>();
			}
			SoundEngine.PlaySound(SoundID.Item5, player.Center);
		}
	}
}