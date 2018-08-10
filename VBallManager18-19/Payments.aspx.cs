using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Payments : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            ShowPayments();
            AssignEditRowValues();
         }

        private bool IsSuperAdmin()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {
                String userId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
                String passcode = Request.Cookies[Constants.PRIMARY_USER][Constants.PASSCODE];
                Player player = Manager.FindPlayerById(userId);
                if (!String.IsNullOrEmpty(player.Passcode) && player.Passcode == passcode && Manager.ActionPermitted(Actions.Admin_Management, player.Role))
                {
                    return true;
                }
            }
            TextBox passcodeTb = (TextBox)Master.FindControl("PasscodeTb");
            if (Manager.SuperAdmin != passcodeTb.Text)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong passcode! Re-enter your passcode and try again')", true);
                return false;
            }
            Session[Constants.SUPER_ADMIN] = passcodeTb.Text;
            return true;
        }

 
       private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        private Pool CurrentPool
        {
            get
            {
                return (Pool)Application[Constants.POOL];

            }
            set { Application[Constants.POOL] = value; }
        }
 
  

        private void ShowPayments()
        {
            //Create title row
            TableRow row = new TableRow();
            row.Cells.Add(CreateTableCell("Date"));
            row.Cells.Add(CreateTableCell("Player"));
            row.Cells.Add(CreateTableCell("Game Day"));
            row.Cells.Add(CreateTableCell("Amount"));
            row.Cells.Add(CreateTableCell("Note"));
            row.Cells.Add(CreateTableCell("Save/Edit"));
            //this.PaymentTable.Rows.Add(row);
            //Create new row
            if (Session[Constants.PAYMENT_ID] == null)
            {
                int index = this.PayPlayerDl.SelectedIndex;
                this.PayPlayerDl.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.PayPlayerDl.DataTextField = "Name";
                this.PayPlayerDl.DataValueField = "Id";
                this.PayPlayerDl.DataBind();
                this.PayPlayerDl.SelectedIndex = index;
                //this.PaymentTable.Rows.Add(this.paymentEditRow);
                this.PayDateTb.Text = DateTime.Today.ToString();
            }
            foreach (Payment payment in Manager.Payments)
            {
                if (Session[Constants.PAYMENT_ID] != null && payment.PaymentId == (String)Session[Constants.PAYMENT_ID])
                {
                    this.PaymentTable.Rows.Remove(this.paymentEditRow);
                    this.PaymentTable.Rows.Add(this.paymentEditRow);
                    this.PaymentSaveBtn.Click += new EventHandler(SavePayment_Click);
                }
                else if (Manager.FindPlayerById(payment.PlayerId)!=null)
                {
                    row = new TableRow();
                    row.Cells.Add(CreateTableCell(payment.Date.ToShortDateString()));
                    row.Cells.Add(CreateTableCell(Manager.FindPlayerById(payment.PlayerId).Name));
                    row.Cells.Add(CreateTableCell(payment.DayOfWeek.ToString()));
                    row.Cells.Add(CreateTableCell(payment.Amount.ToString()));
                    row.Cells.Add(CreateTableCell(payment.Note));
                    Button editBtn = new Button();
                    editBtn.Text = "Edit";
                    editBtn.ID = "Edit:" + payment.PaymentId;
                    editBtn.Click += EditPayment_Click;
                    TableCell editBtnCell = new TableCell();
                    editBtnCell.Controls.Add(editBtn);
                    row.Cells.Add(editBtnCell);
                    this.PaymentTable.Rows.Add(row);
                }
            }
        }

         private void AssignEditRowValues()
        {
            if (Session[Constants.PAYMENT_ID] == null)
            {
                return;
            }
            Payment payment = Manager.FindPaymentById(Session[Constants.PAYMENT_ID].ToString());
            if (payment != null)
            {
                this.PayPlayerDl.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.PayPlayerDl.DataTextField = "Name";
                this.PayPlayerDl.DataValueField = "Id";
                this.PayPlayerDl.DataBind();
                this.PayDateTb.Text = payment.Date.ToShortDateString();
                this.PayPlayerDl.SelectedValue = payment.PlayerId;
                this.PayDayOfWeekDl.SelectedValue = payment.DayOfWeek.ToString();
                this.PayAmountTb.Text = payment.Amount.ToString();
                this.PayNoteTb.Text = payment.Note;
                this.PaymentSaveBtn.ID = payment.PaymentId;
            }
            else
            {
                this.PayDateTb.Text = DateTime.Today.ToString();
            }
        }

        protected void SavePayment_Click(object sender, EventArgs e)
        {
            String paymentId = (String)Session[Constants.PAYMENT_ID];
            Payment payment = payment = Manager.FindPaymentById(paymentId);
            if (payment == null)
            {
                payment = new Payment();
                payment.PaymentId = Guid.NewGuid().ToString();
                Manager.Payments.Add(payment);
            }
            payment.PlayerId = this.PayPlayerDl.SelectedValue;
            payment.Date = DateTime.Parse(this.PayDateTb.Text);
            payment.DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), this.PayDayOfWeekDl.SelectedValue);
            payment.Amount = int.Parse(this.PayAmountTb.Text);
            payment.Note = this.PayNoteTb.Text;
            DataAccess.Save(Manager);
            Session[Constants.PAYMENT_ID] = null;
            Response.Redirect("/Payments.Aspx");

        }
        protected void EditPayment_Click(object sender, EventArgs e)
        {
            Button btm = (Button)sender;
            String paymentId = btm.ID.Split(':')[1];
            Session[Constants.PAYMENT_ID] = paymentId;
            Response.Redirect(Request.RawUrl);
        }

        protected void DeletePayment_Click(object sender, EventArgs e)
        {
            Button btm = (Button)sender;
            String paymentId = btm.ID.Split('-')[1];
            Payment payment = Manager.FindPaymentById(paymentId);
            Manager.Payments.Remove(payment);
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        protected void CancelPayment_Click(object sender, EventArgs e)
        {
            Session[Constants.PAYMENT_ID] = null;
        }

        private TableCell CreateTableCell(String content)
        {
            TableCell cell = new TableCell();
            cell.Text = content;
            return cell;
        }
        private TableCell CreateTableCell(Control control)
        {
            TableCell cell = new TableCell();
            //cell.Controls.Add(control); 
            return cell;
        }
    }
}