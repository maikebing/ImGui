﻿//#define DrawPaddingBox
//#define DrawContentBox

using ImGui.Common;
using ImGui.Common.Primitive;
using ImGui.OSAbstraction.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImGui
{
    internal static class GUIPrimitive
    {
        /// <summary>
        /// Render a rectangle shaped with optional rounding and borders
        /// </summary>
        /// <param name="drawList"></param>
        /// <param name="p_min"></param>
        /// <param name="p_max"></param>
        /// <param name="fill_col"></param>
        /// <param name="border"></param>
        /// <param name="rounding"></param>
        public static void RenderFrame(this DrawList drawList, Point p_min, Point p_max, Color fill_col,
            bool border = false, float rounding = 15)
        {
            drawList.AddRectFilled(p_min, p_max, fill_col, rounding);
            if (border)
            {
                drawList.AddRect(p_min + new Vector(1, 1), p_max + new Vector(1, 1), Color.Black, rounding);
                drawList.AddRect(p_min, p_max, new Color(0.70f, 0.70f, 0.70f, 0.65f), rounding);
            }
        }

        /// <summary>
        /// Draw a box model
        /// </summary>
        /// <param name="drawList"></param>
        /// <param name="rect">the rect (of the border-box) to draw this box model </param>
        /// <param name="text">text of the box model</param>
        /// <param name="style">style of the box model</param>
        /// <param name="state"></param>
        public static void DrawBoxModel(this DrawList drawList, Rect rect, string text, GUIStyle style, GUIState state = GUIState.Normal)
        {
            //Widths of border
            var bt = style.Get<double>(GUIStyleName.BorderTop, state);
            var br = style.Get<double>(GUIStyleName.BorderRight, state);
            var bb = style.Get<double>(GUIStyleName.BorderBottom, state);
            var bl = style.Get<double>(GUIStyleName.BorderLeft, state);

            //Widths of padding
            var pt = style.Get<double>(GUIStyleName.PaddingTop, state);
            var pr = style.Get<double>(GUIStyleName.PaddingRight, state);
            var pb = style.Get<double>(GUIStyleName.PaddingBottom, state);
            var pl = style.Get<double>(GUIStyleName.PaddingLeft, state);

            //4 corner of the border-box
            var btl = new Point(rect.Left, rect.Top);
            var btr = new Point(rect.Right, rect.Top);
            var bbr = new Point(rect.Right, rect.Bottom);
            var bbl = new Point(rect.Left, rect.Bottom);
            var borderBoxRect = new Rect(btl, bbr);

            //4 corner of the padding-box
            var ptl = new Point(btl.X + bl, btl.Y + bt);
            var ptr = new Point(btr.X - br, btr.Y + bt);
            var pbr = new Point(bbr.X - br, bbr.Y - bb);
            var pbl = new Point(bbl.X + bl, bbl.Y - bb);
            //if (ptl.X > ptr.X) return;//TODO what if (ptl.X > ptr.X) happens?
            var paddingBoxRect = new Rect(ptl, pbr);

            //4 corner of the content-box
            var ctl = new Point(ptl.X + pl, ptl.Y + pt);
            var ctr = new Point(ptr.X - pr, ptr.Y + pr);
            var cbr = new Point(pbr.X - pr, pbr.Y - pb);
            var cbl = new Point(pbl.X + pl, pbl.Y - pb);
            var contentBoxRect = new Rect(ctl, cbr);

            // draw background in padding-box
            drawList.AddRectFilled(paddingBoxRect.TopLeft, paddingBoxRect.BottomRight, style.Get<Color>(GUIStyleName.BackgroundColor, state));

            //Content
            //Content-box
            if (text != null && ctl.X < ctr.X)//content should not be visible when ctl.X > ctr.X
            {
                //TODO handle text alignment
                drawList.DrawText(contentBoxRect, text, style, state);
            }

            //Border
            //  Top
            if (!MathEx.AmostZero(bt))
            {
                var borderTopColor = style.Get<Color>(GUIStyleName.BorderTopColor, state);
                if (!MathEx.AmostZero(borderTopColor.A))
                {
                    drawList.PathLineTo(ptl);
                    drawList.PathLineTo(btl);
                    drawList.PathLineTo(btr);
                    drawList.PathLineTo(ptr);
                    drawList.PathFill(borderTopColor);
                }
            }
            //  Right
            if (!MathEx.AmostZero(br))
            {
                var borderRightColor = style.Get<Color>(GUIStyleName.BorderRightColor, state);
                if(!MathEx.AmostZero(borderRightColor.A))
                {
                    drawList.PathLineTo(ptr);
                    drawList.PathLineTo(btr);
                    drawList.PathLineTo(bbr);
                    drawList.PathLineTo(pbr);
                    drawList.PathFill(borderRightColor);
                }
            }
            //  Bottom
            if (!MathEx.AmostZero(bb))
            {
                var borderBottomColor = style.Get<Color>(GUIStyleName.BorderBottomColor, state);
                if (!MathEx.AmostZero(borderBottomColor.A))
                {
                    drawList.PathLineTo(pbr);
                    drawList.PathLineTo(bbr);
                    drawList.PathLineTo(bbl);
                    drawList.PathLineTo(pbl);
                    drawList.PathFill(borderBottomColor);
                }
            }
            //  Left
            if (!MathEx.AmostZero(bl))
            {
                var borderLeftColor = style.Get<Color>(GUIStyleName.BorderLeftColor, state);
                if (!MathEx.AmostZero(borderLeftColor.A))
                {
                    drawList.PathLineTo(pbl);
                    drawList.PathLineTo(bbl);
                    drawList.PathLineTo(btl);
                    drawList.PathLineTo(ptl);
                    drawList.PathFill(borderLeftColor);
                }
            }

            //Outline
            var outlineWidth = style.Get<double>(GUIStyleName.OutlineWidth, state);
            if (!MathEx.AmostZero(outlineWidth))
            {
                var outlineColor = style.Get<Color>(GUIStyleName.OutlineColor, state);
                if(!MathEx.AmostZero(outlineColor.A))
                {
                    drawList.PathRect(btl, bbr);
                    drawList.PathStroke(outlineColor, true, outlineWidth);
                }
            }

#if DrawPaddingBox
            drawList.PathRect(ptl, pbr);
            drawList.PathStroke(Color.ColorRgb(0, 100, 100), true, 1);
#endif

#if DrawContentBox
            drawList.PathRect(ctl, cbr);
            drawList.PathStroke(Color.ColorRgb(100, 0, 100), true, 1);
#endif
        }

        /// <summary>
        /// Draw a box model
        /// </summary>
        /// <param name="drawList"></param>
        /// <param name="rect">the rect (of the border-box) to draw this box model </param>
        /// <param name="texture">texture of the box model</param>
        /// <param name="style">style of the box model</param>
        /// <param name="state"></param>
        public static void DrawBoxModel(this DrawList drawList, Rect rect, ITexture texture, GUIStyle style, GUIState state = GUIState.Normal)
        {
            //Widths of border
            var bt = style.Get<double>(GUIStyleName.BorderTop, state);
            var br = style.Get<double>(GUIStyleName.BorderRight, state);
            var bb = style.Get<double>(GUIStyleName.BorderBottom, state);
            var bl = style.Get<double>(GUIStyleName.BorderLeft, state);

            //Widths of padding
            var pt = style.Get<double>(GUIStyleName.PaddingTop, state);
            var pr = style.Get<double>(GUIStyleName.PaddingRight, state);
            var pb = style.Get<double>(GUIStyleName.PaddingBottom, state);
            var pl = style.Get<double>(GUIStyleName.PaddingLeft, state);

            //4 corner of the border-box
            var btl = new Point(rect.Left, rect.Top);
            var btr = new Point(rect.Right, rect.Top);
            var bbr = new Point(rect.Right, rect.Bottom);
            var bbl = new Point(rect.Left, rect.Bottom);
            var borderBoxRect = new Rect(btl, bbr);

            //4 corner of the padding-box
            var ptl = new Point(btl.X + bl, btl.Y + bt);
            var ptr = new Point(btr.X - br, btr.Y + bt);
            var pbr = new Point(bbr.X - br, bbr.Y - bb);
            var pbl = new Point(bbl.X + bl, bbl.Y - bb);
            var paddingBoxRect = new Rect(ptl, pbr);

            //4 corner of the content-box
            var ctl = new Point(ptl.X + pl, ptl.Y + pt);
            var ctr = new Point(ptr.X - pr, ptr.Y + pr);
            var cbr = new Point(pbr.X - pr, pbr.Y - pb);
            var cbl = new Point(pbl.X + pl, pbl.Y - pb);
            var contentBoxRect = new Rect(ctl, cbr);

            // draw background in padding-box
            drawList.AddRectFilled(paddingBoxRect.TopLeft, paddingBoxRect.BottomRight, style.Get<Color>(GUIStyleName.BackgroundColor, state));

            //Content
            //Content-box
            if (texture != null && ctl.X < ctr.X)//content should not be visible when ctl.X > ctr.X
            {
                drawList.DrawImage(contentBoxRect, texture, style);
            }

            //Border
            //  Top
            if (!MathEx.AmostZero(bt))
            {
                var borderTopColor = style.Get<Color>(GUIStyleName.BorderTopColor, state);
                if (!MathEx.AmostZero(borderTopColor.A))
                {
                    drawList.PathLineTo(ptl);
                    drawList.PathLineTo(btl);
                    drawList.PathLineTo(btr);
                    drawList.PathLineTo(ptr);
                    drawList.PathFill(borderTopColor);
                }
            }
            //  Right
            if (!MathEx.AmostZero(br))
            {
                var borderRightColor = style.Get<Color>(GUIStyleName.BorderRightColor, state);
                if (!MathEx.AmostZero(borderRightColor.A))
                {
                    drawList.PathLineTo(ptr);
                    drawList.PathLineTo(btr);
                    drawList.PathLineTo(bbr);
                    drawList.PathLineTo(pbr);
                    drawList.PathFill(borderRightColor);
                }
            }
            //  Bottom
            if (!MathEx.AmostZero(bb))
            {
                var borderBottomColor = style.Get<Color>(GUIStyleName.BorderBottomColor, state);
                if (!MathEx.AmostZero(borderBottomColor.A))
                {
                    drawList.PathLineTo(pbr);
                    drawList.PathLineTo(bbr);
                    drawList.PathLineTo(bbl);
                    drawList.PathLineTo(pbl);
                    drawList.PathFill(borderBottomColor);
                }
            }
            //  Left
            if (!MathEx.AmostZero(bl))
            {
                var borderLeftColor = style.Get<Color>(GUIStyleName.BorderLeftColor, state);
                if (!MathEx.AmostZero(borderLeftColor.A))
                {
                    drawList.PathLineTo(pbl);
                    drawList.PathLineTo(bbl);
                    drawList.PathLineTo(btl);
                    drawList.PathLineTo(ptl);
                    drawList.PathFill(borderLeftColor);
                }
            }

            //Outline
            var outlineWidth = style.Get<double>(GUIStyleName.OutlineWidth, state);
            if (!MathEx.AmostZero(outlineWidth))
            {
                var outlineColor = style.Get<Color>(GUIStyleName.OutlineColor, state);
                if (!MathEx.AmostZero(outlineColor.A))
                {
                    drawList.PathRect(btl, bbr);
                    drawList.PathStroke(outlineColor, true, outlineWidth);
                }
            }

#if DrawPaddingBox
            drawList.PathRect(ptl, pbr);
            drawList.PathStroke(Color.ColorRgb(0, 100, 100), true, 1);
#endif

#if DrawContentBox
            drawList.PathRect(ctl, cbr);
            drawList.PathStroke(Color.ColorRgb(100, 0, 100), true, 1);
#endif
        }

        /// <summary>
        /// Draw a text content
        /// </summary>
        /// <param name="drawList"></param>
        /// <param name="rect">the rect (of the text layouting box) to draw this text content</param>
        /// <param name="text">the text</param>
        /// <param name="style">style of the box model</param>
        /// <param name="state">state of the style</param>
        public static void DrawText(this DrawList drawList, Rect rect, string text, GUIStyle style, GUIState state)
        {
            var textMesh = drawList.TextMesh;
            drawList.AddTextDrawCommand();

            var oldIndexBufferCount = textMesh.IndexBuffer.Count;

            string fontFamily = style.FontFamily;
            double fontSize = style.FontSize;
            Color fontColor = style.FontColor;

            // get offset and scale from text layout
            var scale = OSImplentation.TypographyTextContext.GetScale(fontFamily, fontSize);
            var textContext = TextMeshUtil.GetTextContext(text, rect.Size, style, state) as OSImplentation.TypographyTextContext;
            var glyphOffsets = textContext.GlyphOffsets;

            int index = 0;

            // get glyph data from typeface
            FontStyle fontStyle = style.FontStyle;
            FontWeight fontWeight = style.FontWeight;
            foreach (var character in text)
            {
                var glyphData = GlyphCache.Default.GetGlyph(character, fontFamily, fontStyle, fontWeight);
                if (glyphData == null)
                {
                    Typography.OpenFont.Glyph glyph = OSImplentation.TypographyTextContext.LookUpGlyph(fontFamily, character);
                    var polygons = new List<List<Point>>();
                    var bezierSegments = new List<(Point, Point, Point)>();
                    Typography.OpenFont.GlyphLoader.Read(glyph, out polygons, out bezierSegments);
                    GlyphCache.Default.AddGlyph(character, fontFamily, fontStyle, fontWeight, polygons, bezierSegments);
                    glyphData = GlyphCache.Default.GetGlyph(character, fontFamily, fontStyle, fontWeight);
                    Debug.Assert(glyphData != null);
                }

                // append to drawlist
                Vector glyphOffset = glyphOffsets[index];
                index++;
                var positionOffset = (Vector)rect.TopLeft;
                drawList.TextMesh.Append(positionOffset, glyphData, glyphOffset, scale, fontColor, false);
            }

            var newIndexBufferCount = textMesh.IndexBuffer.Count;

            // Update added command
            var command = textMesh.Commands[textMesh.Commands.Count - 1];
            command.ElemCount = newIndexBufferCount - oldIndexBufferCount;
            textMesh.Commands[textMesh.Commands.Count - 1] = command;

            // TODO merge command with previous one if they share the same clip rect.
        }

        /// <summary>
        /// Draw an image content
        /// </summary>
        /// <param name="drawList"></param>
        /// <param name="rect">the rect to draw this image content</param>
        /// <param name="texture">the texture</param>
        /// <param name="style">style of the image content (not used)</param>
        public static void DrawImage(this DrawList drawList, Rect rect, ITexture texture, GUIStyle style)
        {
            var (top, right, bottom, left) = style.BorderImageSlice;

            if (MathEx.AmostEqual(top, 0)
                && MathEx.AmostEqual(left, 0)
                && MathEx.AmostEqual(right, 0)
                && MathEx.AmostEqual(bottom, 0))
            {
                drawList.AddImage(texture, rect.TopLeft, rect.BottomRight, Point.Zero, new Point(1,1), Color.White);
            }
            else
            {
                Point uv0 = new Point(left / texture.Width, top / texture.Height);
                Point uv1 = new Point(1 - right / texture.Width, 1 - bottom / texture.Height);

                //     | L |   | R |
                // ----a---b---c---+
                //   T | 1 | 2 | 3 |
                // ----d---e---f---g
                //     | 4 | 5 | 6 |
                // ----h---i---j---k
                //   B | 7 | 8 | 9 |
                // ----+---l---m---n

                var a = rect.TopLeft;
                var b = a + new Vector(left, 0);
                var c = rect.TopRight + new Vector(-right, 0);

                var d = a + new Vector(0, top);
                var e = b + new Vector(0, top);
                var f = c + new Vector(0, top);
                var g = f + new Vector(right, 0);

                var h = rect.BottomLeft + new Vector(0, -bottom);
                var i = h + new Vector(left, 0);
                var j = rect.BottomRight + new Vector(-right, -bottom);
                var k = j + new Vector(right, 0);

                var l = i + new Vector(0, bottom);
                var m = rect.BottomRight + new Vector(-right, 0);
                var n = rect.BottomRight;

                var uv_a = new Point(0, 0);
                var uv_b = new Point(uv0.X, 0);
                var uv_c = new Point(uv1.X, 0);

                var uv_d = new Point(0, uv0.Y);
                var uv_e = new Point(uv0.X, uv0.Y);
                var uv_f = new Point(uv1.X, uv0.Y);
                var uv_g = new Point(1, uv0.Y);

                var uv_h = new Point(0, uv1.Y);
                var uv_i = new Point(uv0.X, uv1.Y);
                var uv_j = new Point(uv1.X, uv1.Y);
                var uv_k = new Point(1, uv1.Y);

                var uv_l = new Point(uv0.X, 1);
                var uv_m = new Point(uv1.X, 1);
                var uv_n = new Point(1, 1);

                //TODO merge these draw-calls (each call of AddImage will introduce a draw call)
                drawList.AddImage(texture, a, e, uv_a, uv_e, Color.White);//1
                drawList.AddImage(texture, b, f, uv_b, uv_f, Color.White);//2
                drawList.AddImage(texture, c, g, uv_c, uv_g, Color.White);//3
                drawList.AddImage(texture, d, i, uv_d, uv_i, Color.White);//4
                drawList.AddImage(texture, e, j, uv_e, uv_j, Color.White);//5
                drawList.AddImage(texture, f, k, uv_f, uv_k, Color.White);//6
                drawList.AddImage(texture, h, l, uv_h, uv_l, Color.White);//7
                drawList.AddImage(texture, i, m, uv_i, uv_m, Color.White);//8
                drawList.AddImage(texture, j, n, uv_j, uv_n, Color.White);//9
            }
        }

    }
}
