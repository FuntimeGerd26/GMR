using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
    public class ArkEnchantment : ModItem
    {
        public bool flip;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"Increases movement speed by 35%\nIncreases knockback by 25%\nIncreases armor penetration and attack speed of all weapons by 15%\nIncreases max minions and sentries by 3" +
                $"\nGrants the 'Blood Overflow' buff" +
                $"\nWhen under 75% of health: increased armor penetration, crit chance, damage by 14% and decreased mana cost by 10%" +
                $"\nSummons 8 orbiting BL Books and Psyco Axes around the player and increases damage taken by 10%" +
                $"\nMelee weapons shoot an Alloyblood Dagger that inflicts 'Devilish' to enemies, hitting enemies with this dagger has a chance to drop Alloyblood Cans above you which heal 15% life" +
                "\n'Conclusion, One'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.DamageType = DamageClass.Generic;
            Item.damage = 200;
            Item.crit = 14;
            Item.knockBack = 18f;
            Item.rare = 8;
            Item.value = Item.sellPrice(silver: 380);
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 2);
            player.moveSpeed += 0.35f;
            player.maxMinions += 3;
            player.maxTurrets += 3;
            player.GetKnockback(DamageClass.Generic) += 0.25f;
            player.GetArmorPenetration(DamageClass.Generic) += 15f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
            if (player.GPlayer().EnchantToggles["AlloybloodDagger"])
            {
                player.GPlayer().AlloybloodEnch = Item;
            }


            if (player.statLife < (int)(player.statLifeMax * 0.25f))
            {
                player.AddBuff(ModContent.BuffType<Buffs.Buff.ArkBuffBoost>(), 2);
            }
            else
                player.AddBuff(ModContent.BuffType<Buffs.Buff.ArkBuff>(), 2);

            if (player.GPlayer().EnchantToggles["AlloybloodOrbitingProjectiles"])
            {
                player.GPlayer().BLBook = true;
                flip = !flip;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>()] < 1)
                {
                    const int max = 6;
                    Vector2 velocity = new Vector2(0f, -2f);
                    for (int i = 0; i < max; i++)
                    {
                        Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
                        Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>(), Item.damage, 14f,
                            player.whoAmI, 0, (player.Center - 100 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
                    }
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>()] < 1)
                {
                    const int max2 = 2;
                    Vector2 velocity = new Vector2(0f, -6f);
                    for (int y = 0; y < max2; y++)
                    {
                        Vector2 perturbedSpeed2 = velocity.RotatedBy(2 * Math.PI / max2 * y);
                        Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed2, ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>(), Item.damage + 10, 4f,
                            player.whoAmI, 0, (player.Center - 200 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
                    }
                }
            }
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "ArkHelmet");
            recipe.AddIngredient(null, "ArkChestplate");
            recipe.AddIngredient(null, "ArkBoots");
            recipe.AddIngredient(null, "ArkBlade");
            recipe.AddIngredient(null, "NeonGunblade");
            recipe.AddIngredient(null, "Equalizer");
            recipe.AddIngredient(null, "AlloybloodEnchantment");
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}