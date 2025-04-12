using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Staffs
{
	public class MagmaStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magmatic Staff");
			Tooltip.SetDefault("Shoots 5 fireballs that can pass through blocks");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 92;
			Item.height = 90;
			Item.rare = 3;
			Item.useTime = 8;
			Item.useAnimation = 48;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 80);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item34;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 17;
			Item.crit = -3;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Flames;
			Item.shootSpeed = 5f;
			Item.mana = 6;
		}
	}
}