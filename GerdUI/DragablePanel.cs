using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace GMR.GerdUI
{
    public class DragablePanel : UIPanel
    {
        // Stores the offset from the top left of the UIPanel while dragging
        private Vector2 offset;
        // A flag that checks if the panel is currently being dragged
        private bool dragging;
        UIElement[] UIChildren;

        public DragablePanel() { }
        public DragablePanel(params UIElement[] uiChildren)
        {
            UIChildren = uiChildren;
        }

        private void DragStart(Vector2 mousePosition)
        {
            // The offset variable helps to remember the position of the panel relative to the mouse position
            // So no matter where you start dragging the panel, it will move smoothly
            offset = new Vector2(mousePosition.X - Left.Pixels, mousePosition.Y - Top.Pixels);
            dragging = true;
        }

        private void DragEnd(Vector2 mousePosition)
        {
            Vector2 endMousePosition = mousePosition;
            dragging = false;

            Left.Set(endMousePosition.X - offset.X, 0f);
            Top.Set(endMousePosition.Y - offset.Y, 0f);

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (!dragging && ContainsPoint(Main.MouseScreen) && Main.mouseLeft && PlayerInput.MouseInfoOld.LeftButton == ButtonState.Released)
            {
                bool upperMost = true;
                if (UIChildren != null)
                {
                    IEnumerable<UIElement> children = Elements.Concat(UIChildren);

                    foreach (UIElement element in children)
                    {
                        if (element.ContainsPoint(Main.MouseScreen) && element as UIPanel == null)
                        {
                            upperMost = false;
                            break;
                        }
                    }
                }

                if (upperMost)
                    DragStart(Main.MouseScreen);
            }
            else if (dragging && !Main.mouseLeft)
            {
                DragEnd(Main.MouseScreen);
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            // Here we check if the DragableUIPanel is outside the Parent UIElement rectangle
            // (In our example, the parent would be ExampleCoinsUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
            // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                // Recalculate forces the UI system to do the positioning math again.
                Recalculate();
            }
        }
    }
}
