using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuaniuSearchBar
{

    public partial class LeftPopupWnd : Form
    {
        const int ITEM_COUNT = 3;

        PictureBox pbIcon;
        PictureBox[] items;
      

        private struct PictureBoxTag
        {
            public string name;
            public Image img;
            public bool hoverFlag ;
        }
        PictureBoxTag[] pbTags;

        public LeftPopupWnd(PictureBox pbIcon)
        {
            InitializeComponent();

          
            this.MaximumSize = this.MinimumSize = this.Size;

            this.pbIcon = pbIcon;
            items = new PictureBox[] { pictureBox1, pictureBox2 ,pictureBox3};
            pbTags = new PictureBoxTag[ITEM_COUNT];
            pbTags[0].name = "百度";
            pbTags[0].img = Properties.Resources.bd_logo_18x18;
            pbTags[1].name = "搜狗";
            pbTags[1].img = Properties.Resources.sg_logo_18x18;
            pbTags[2].name = "360搜索";
            pbTags[2].img = Properties.Resources._360ss_logo_18x18;

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Click += pictureBox_Click;
                items[i].MouseEnter += LeftPopupWnd_MouseEnter;
                items[i].MouseLeave += LeftPopupWnd_MouseLeave;
                items[i].Paint += pictureBox_Paint;
                //pbTags[i] = new PictureBoxTag();
                pbTags[i].hoverFlag = false;
                items[i].Tag = pbTags[i];
            }
           

        }

     

        private void Label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.White;
        }

        private void Label_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Gray;
        }
        private void LeftPopupWnd_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            var tag = ((PictureBoxTag)pb.Tag);
            tag.hoverFlag = false;
            pb.Tag = tag;
            pb.Invalidate();
        }

        private void LeftPopupWnd_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            var tag= ((PictureBoxTag)pb.Tag) ;
            tag.hoverFlag = true;
            pb.Tag = tag;
            pb.Invalidate();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            pbIcon.Image = ((PictureBoxTag)(sender as PictureBox).Tag).img;
            pbIcon.Tag = ((PictureBoxTag)(sender as PictureBox).Tag).name;
            this.Close();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            var g=e.Graphics;
            var pbTag = (PictureBoxTag)pb.Tag;
            if (pbTag.hoverFlag)
            {
                g.FillRectangle(Brushes.WhiteSmoke, 0, 0, pb.Width, pb.Height);
            }
            if (pbTag.img!=null)
            {
                g.DrawImage(pbTag.img, new Point(5, 5));
                g.DrawString(pbTag.name, new Font("宋体", 11), Brushes.Black, new PointF(25, 3));

            }
            
        }
    }
}
