using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Others
{
	public class AncientHarpoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 64;
			Item.rare = 3;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 165);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 60;
			Item.crit = 3;
			Item.knockBack = 18f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.AncientHarpoon>();
			Item.shootSpeed = 24f;
		}
	}
}