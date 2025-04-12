using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class JackSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.JackSword>(20);
			Item.SetWeaponValues(20, 5f, -2);
			Item.width = 64;
			Item.height = 78;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 170);
			Item.rare = 3;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			Item.FixSwing(player);
			return null;
		}

		public override bool MeleePrefix()
		{
			return true;
		}
	}
}