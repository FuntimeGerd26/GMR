using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Spears
{
	public class MaskedPlaguePike : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 84;
			Item.height = 84;
			Item.rare = 3;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 133);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 35;
			Item.crit = 0;
			Item.knockBack = 14f;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.MaskedPlaguePike>();
			Item.shootSpeed = 8f;
			Item.channel = true;
			Item.noMelee = true;
		}

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 24);
			recipe.AddIngredient(ItemID.Silk, 18);
			recipe.AddIngredient(ItemID.Bone, 28);
			recipe.AddRecipeGroup("GMR:AnyGem", 4);
			recipe.AddIngredient(null, "UpgradeCrystal", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}