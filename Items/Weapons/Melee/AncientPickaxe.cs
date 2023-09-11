using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class AncientPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Pickaxe");
			Tooltip.SetDefault("Hold right-click to throw the pickaxe in an arc");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 52;
			Item.rare = 3;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 80);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 25;
			Item.crit = 4;
			Item.knockBack = 2.5f;
			Item.useTurn = true;
			Item.pick = 100;
			Item.shootSpeed = 12f;
			Item.noMelee = false;
			Item.noUseGraphic = false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.noMelee = true;
				Item.noUseGraphic = true;
				Item.UseSound = SoundID.Item7;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AncientPickaxe>();
				Item.useTurn = false;
			}
			else
			{
				Item.noMelee = false;
				Item.noUseGraphic = false;
				Item.UseSound = SoundID.Item1;
				Item.shoot = 0;
				Item.useTurn = true;
			}
			return true;
		}
	}
}