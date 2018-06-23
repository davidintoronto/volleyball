using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace VballManager
{
    public partial class v2pc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            String xsltPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\xslt.xml";
            String cpString = null;
            String deleteString = null;
                if (File.Exists(xsltPath) && this.PrefixTb.Text != "" && this.StartNumTb.Text != "")
                {
                    String xslt = File.OpenText(xsltPath).ReadToEnd();
                    int startNumber = int.Parse(this.StartNumTb.Text);
                    for (int i = 0; i < 10; i++)
                    {
                        String contentId = this.PrefixTb.Text + startNumber + i;
                        String replace = "V2PC" + i;
                        xslt = xslt.Replace(replace, contentId);
                        cpString = cpString + "\r\ncreateDirectoryAndRename(\"TVE1000\", \"" + contentId + "\")";
                        deleteString = deleteString + "\r\nrm -rf " + contentId; 
                    }
                    this.ResultXsltTb.Text = xslt;
                    this.ResultCpCmdTb.Text = cpString + deleteString;

                }      

        }
    }
}