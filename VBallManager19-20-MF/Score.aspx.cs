using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Score : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.A11.Text = Manager.GameScores.A11;
                this.A12.Text = Manager.GameScores.A12;
                this.A13.Text = Manager.GameScores.A13;
                this.A14.Text = Manager.GameScores.A14;
                this.A21.Text = Manager.GameScores.A21;
                this.A22.Text = Manager.GameScores.A22;
                this.A23.Text = Manager.GameScores.A23;
                this.A25.Text = Manager.GameScores.A25;
                this.B11.Text = Manager.GameScores.B11;
                this.B13.Text = Manager.GameScores.B13;
                this.B22.Text = Manager.GameScores.B22;
                this.B23.Text = Manager.GameScores.B23;
                this.B31.Text = Manager.GameScores.B31;
                this.B32.Text = Manager.GameScores.B32;
                this.D14.Text = Manager.GameScores.D14;
                this.D15.Text = Manager.GameScores.D15;
                this.D24.Text = Manager.GameScores.D24;
                this.D25.Text = Manager.GameScores.D25;
                this.D34.Text = Manager.GameScores.D34;
                this.D35.Text = Manager.GameScores.D35;
              }

        }
        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }
        protected void SaveA11_Click(object sender, EventArgs e)
        {
            Manager.GameScores.A11 = this.A11.Text;
            DataAccess.Save(Manager);
        }
        protected void SaveA12_Click(object sender, EventArgs e)
        {
 
            Manager.GameScores.A12 = this.A12.Text;
            DataAccess.Save(Manager);
        }
        protected void SaveA13_Click(object sender, EventArgs e)
        {
 
            Manager.GameScores.A13 = this.A13.Text;
            DataAccess.Save(Manager);
        }
        protected void SaveA14_Click(object sender, EventArgs e)
        {
 
            Manager.GameScores.A14 = this.A14.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveA21_Click(object sender, EventArgs e)
        {
            Manager.GameScores.A21 = this.A21.Text;
            DataAccess.Save(Manager);

           }
        protected void SaveA22_Click(object sender, EventArgs e)
        {
            Manager.GameScores.A22 = this.A22.Text;
            DataAccess.Save(Manager);

          }
        protected void SaveA23_Click(object sender, EventArgs e)
        {
            Manager.GameScores.A23 = this.A23.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveA25_Click(object sender, EventArgs e)
        {
            Manager.GameScores.A25 = this.A25.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveB11_Click(object sender, EventArgs e)
        {
            Manager.GameScores.B11 = this.B11.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveB13_Click(object sender, EventArgs e)
        {
            Manager.GameScores.B13 = this.B13.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveB22_Click(object sender, EventArgs e)
        {
            Manager.GameScores.B22 = this.B22.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveB23_Click(object sender, EventArgs e)
        {
            Manager.GameScores.B23 = this.B23.Text;
            DataAccess.Save(Manager);
        }
        protected void SaveB31_Click(object sender, EventArgs e)
        {
 
            Manager.GameScores.B31 = this.B31.Text;
            DataAccess.Save(Manager);
        }
        protected void SaveB32_Click(object sender, EventArgs e)
        {
 
            Manager.GameScores.B32 = this.B32.Text;
            DataAccess.Save(Manager);
        }
        protected void SaveD14_Click(object sender, EventArgs e)
        {
 
            Manager.GameScores.D14 = this.D14.Text;
            DataAccess.Save(Manager);

          }
        protected void SaveD15_Click(object sender, EventArgs e)
        {
            Manager.GameScores.D15 = this.D15.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveD24_Click(object sender, EventArgs e)
        {
            Manager.GameScores.D24 = this.D24.Text;
            DataAccess.Save(Manager);

          }
        protected void SaveD25_Click(object sender, EventArgs e)
        {
            Manager.GameScores.D25 = this.D25.Text;
            DataAccess.Save(Manager);

          }
        protected void SaveD34_Click(object sender, EventArgs e)
        {
            Manager.GameScores.D34 = this.D34.Text;
            DataAccess.Save(Manager);

         }
        protected void SaveD35_Click(object sender, EventArgs e)
        {
            Manager.GameScores.D35 = this.D35.Text;
            DataAccess.Save(Manager);
        }

        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}