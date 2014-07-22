using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace WOT.Stats
{
    public static class FormHelpers
    {
        public static void ResizeLables(System.Windows.Forms.Control.ControlCollection controls)
        {
            int max = 0;

            List<LabelControl> labels = new List<LabelControl>();

            foreach (Control item in controls)
            {

                if (item.GetType() == typeof(LabelControl))
                {
                    LabelControl lb = ((LabelControl)item);
                    labels.Add(lb);

                    lb.AutoSizeMode = LabelAutoSizeMode.Horizontal;
                    if (lb.Width > max)
                        max = lb.Width;
                    lb.AutoSizeMode = LabelAutoSizeMode.None;
                    lb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                }
            }

            foreach (LabelControl item in labels)
            {
                item.Width = max + 5;
            }
        }
    }

}
