﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace Sunset.NewCourse
{
    public partial class GraduationPlanSelector : UserControl
    {
        public GraduationPlanSelector()
        {
            InitializeComponent();
            this.cardPanelEx1.SuspendLayout();
            int count=0;

            List<SchedulerProgramPlan> Infos = new List<SchedulerProgramPlan>();
            //SmartSchool.Evaluation.GraduationPlan.GraduationPlan.Instance.Items
            foreach (SchedulerProgramPlan info in Infos)
            {
                count++;
                DevComponents.DotNetBar.ButtonX item = new DevComponents.DotNetBar.ButtonX();
                item.Text = (info.Name.Length>10?info.Name.Substring(0,10)+"...":info.Name);
                item.Tooltip = info.Name;
                item.Tag = info;
                item.TextAlignment = eButtonTextAlignment.Left;
                item.ColorTable = eButtonColor.OrangeWithBackground;
                item.Size = new Size(cardPanelEx1.CardWidth, 23);
                item.Click += new EventHandler(item_Click);
                item.MouseHover+=new EventHandler(SetFocus);
                cardPanelEx1.Controls.Add(item);
            }
            if (count <= 13)
                this.Size = new Size(155, 27 * count + 4);
            else
            {
                if (count <= 26)
                    this.Size = new Size(314, 27 * ((count + 1) / 2) + 4);
            }
            this.cardPanelEx1.ResumeLayout();
        }

        void item_Click(object sender, EventArgs e)
        {
            if (GraduationPlanSelected != null)
                GraduationPlanSelected.Invoke(this, new GraduationPlanSelectedEventArgs((SchedulerProgramPlan)((ButtonX)sender).Tag));
        }
        public event EventHandler<GraduationPlanSelectedEventArgs> GraduationPlanSelected;

        private void SetFocus(object sender, EventArgs e)
        {
            if (!cardPanelEx1.ContainsFocus)
            {
                cardPanelEx1.Focus();
            }
        }

        private void cardPanelEx1_MouseHover(object sender, EventArgs e)
        {

        }
    }
    public class GraduationPlanSelectedEventArgs : EventArgs
    {
        private SchedulerProgramPlan _item;
        public GraduationPlanSelectedEventArgs(SchedulerProgramPlan gplan)
        {
            _item = gplan;
        }
        public SchedulerProgramPlan Item { get { return _item; } }
    }
}
