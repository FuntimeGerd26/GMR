using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace GMR.Items.Weapons.Magic
{
	public class InfraRedStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Staff");
			Tooltip.SetDefault("Shoots a ring of energy from your cursor that spirals outwards");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 120;
			Item.height = 120;
			Item.rare = 4;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 45;
			Item.crit = 28;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.JackStaffProj>();
			Item.shootSpeed = 8f;
			Item.mana = 6;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

			const int max = 4;
			for (int i = 0; i < max; i++)
			{
				Projectile.NewProjectile(source, target, Vector2.UnitX.RotatedBy(2 * Math.PI / max * i) * 2f, Item.shoot, damage, knockback, player.whoAmI);
				Projectile.NewProjectile(source, target, Vector2.UnitX.RotatedBy(2 * Math.PI / max * i) * 2f, ModContent.ProjectileType<Projectiles.Magic.JackStaffProjFlip>(), damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}