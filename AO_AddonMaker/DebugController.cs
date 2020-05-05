using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AO_AddonMaker
{
    static class DebugController
    {
        static TextBoxBase textBox;
        public static void Init(TextBoxBase widget)
        {
            textBox = widget;
        }
        public static void Write(IUIElement uIElement, string msg)
        {
            if (textBox == null)
                throw new ArgumentNullException();
            textBox.AppendText(string.Format("[{0}]: {1}\n", uIElement.GetName(), msg));
        }
        public static void Write(string msg)
        {
            if (textBox == null)
                throw new ArgumentNullException();
            textBox.AppendText(string.Format("{0}\n", msg));
        }
    }
}
