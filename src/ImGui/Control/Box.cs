﻿using System.Diagnostics;
using ImGui.Common.Primitive;

namespace ImGui
{
    public partial class GUI
    {
        /// <summary>
        /// Box
        /// </summary>
        internal static void Box(Rect rect, string text)
        {
            DoControl(rect, text);
        }

        public static void DoControl(Rect rect, string text)
        {
            Form form = Form.current;
            GUIContext g = form.uiContext;
            Window window = g.WindowManager.CurrentWindow;
            DrawList d = window.DrawList;
            d.DrawBoxModel(rect, text, GUISkin.Instance[GUIControlName.Box]);
        }
    }
}