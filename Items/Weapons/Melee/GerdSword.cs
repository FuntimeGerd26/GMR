using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GerdSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gerd's Spirit Sword");
			Tooltip.SetDefault("Throws the sword handle like a flail towards enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 110;
			Item.height = 110;
			Item.rare = 8;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 315);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.damage = 180;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.GerdHandle>();
			Item.shootSpeed = 6f;
			Item.channel = true;
			Item.noMelee = true;
		}
	}
}