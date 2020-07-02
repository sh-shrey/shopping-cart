using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace shopping_cart
{
    public partial class checkout : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\visual basics practical\shopping cart\shopping cart\App_Data\product.mdf;Integrated Security=True");
        public int grandtotal()
        {
            DataTable d = new DataTable();
            d = (DataTable)Session["buyitems"];
            int nrow = d.Rows.Count;
            int i = 0;
            int gtotal = 0;
            while (i < nrow)
            {
                gtotal = gtotal + Convert.ToInt32(d.Rows[i]["price"].ToString());

                i = i + 1;
            }
            return gtotal;
        }
        public void findorderid()
        {
            String pass = "abcdefghijklmnopqrstuvwxyz123456789";
            Random r = new Random();
            char[] mypass = new char[5];
            for (int i = 0; i < 5; i++)
            {
                mypass[i] = pass[(int)(35 * r.NextDouble())];

            }
            String orderdate;
            orderdate =  DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + new string(mypass);

            Label3.Text = orderdate.ToString();


        }

        public void saveaddress()
        {
            String updatepass = "insert into orderaddress(orderid,address,mobilenumber) values('" + Label2.Text + "','" + TextBox1.Text + "','" + TextBox2.Text + "')";
            String mycon1 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\visual basics practical\shopping cart\shopping cart\App_Data\try.mdf;Integrated Security=True";
            SqlConnection s = new SqlConnection(mycon1);
            s.Open();
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = updatepass;
            cmd1.Connection = s;
            cmd1.ExecuteNonQuery();
            s.Close();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable d = new DataTable();
                DataRow dr;
                d.Columns.Add("S.no");
            d.Columns.Add("pro_id");
            d.Columns.Add("product_name");
            d.Columns.Add("quantity");
            d.Columns.Add("price");
            d.Columns.Add("totalprice");
            d.Columns.Add("image");
            d.Columns.Add("description");

            if (Request.QueryString["id"] != null)
            {
                if (Session["Buyitems"] == null)
                {

                    dr = d.NewRow();

                    
                    String myquery = "select * from productdetails where pro_id=" + Request.QueryString["id"];
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = myquery;
                    cmd.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    dr["S.no"] = 1;
                    dr["pro_id"] = ds.Tables[0].Rows[0]["pro_id"].ToString();
                    dr["product_name"] = ds.Tables[0].Rows[0]["product_name"].ToString();
                    dr["image"] = ds.Tables[0].Rows[0]["image"].ToString();
                    dr["quantity"] = Request.QueryString["quantity"];
                    dr["price"] = ds.Tables[0].Rows[0]["price"].ToString();
                    dr["description"] = ds.Tables[0].Rows[0]["description"].ToString();
                    int price = Convert.ToInt16(ds.Tables[0].Rows[0]["price"].ToString());
                    int quantity = Convert.ToInt16(Request.QueryString["quantity"].ToString());
                    int totalprice = price * quantity;
                    dr["price"] = totalprice;

                    d.Rows.Add(dr);
                    GridView1.DataSource = d;
                    GridView1.DataBind();

                    Session["buyitems"] = d;
                    GridView1.FooterRow.Cells[5].Text = "Total Amount";
                    GridView1.FooterRow.Cells[6].Text = grandtotal().ToString();
                    Response.Redirect("AddtoCart.aspx");

                }
                else
                {

                    d = (DataTable)Session["buyitems"];
                    int sr;
                    sr = d.Rows.Count;

                    dr = d.NewRow();
                    String myquery = "select * from productdetails where pro_id=" + Request.QueryString["id"];
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = myquery;
                    cmd.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    dr["S.no"] = sr + 1;
                    dr["pro_id"] = ds.Tables[0].Rows[0]["pro_id"].ToString();
                    dr["product_name"] = ds.Tables[0].Rows[0]["product_name"].ToString();
                    dr["image"] = ds.Tables[0].Rows[0]["image"].ToString();
                    dr["quantity"] = Request.QueryString["quantity"];
                    dr["price"] = ds.Tables[0].Rows[0]["price"].ToString();
                    int price = Convert.ToInt16(ds.Tables[0].Rows[0]["price"].ToString());
                    int quantity = Convert.ToInt16(Request.QueryString["quantity"].ToString());
                    int totalprice = price * quantity;
                    dr["totalprice"] = totalprice;
                    d.Rows.Add(dr);
                    GridView1.DataSource = d;
                    GridView1.DataBind();

                    Session["buyitems"] = d;
                    GridView1.FooterRow.Cells[5].Text = "Total Amount";
                    GridView1.FooterRow.Cells[6].Text = grandtotal().ToString();
                    Response.Redirect("AddtoCart.aspx");

                }
            }
            else
            {
                d = (DataTable)Session["buyitems"];
                GridView1.DataSource = d;
                GridView1.DataBind();
                if (GridView1.Rows.Count > 0)
                {
                    GridView1.FooterRow.Cells[5].Text = "Total Amount";
                    GridView1.FooterRow.Cells[6].Text = grandtotal().ToString();

                }


            }
            Label2.Text = GridView1.Rows.Count.ToString();

        }
    }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            DataTable d = new DataTable();
            d = (DataTable)Session["buyitems"];
            for (int i = 0; i <= d.Rows.Count - 1; i++)
            {
                String mycon1 = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = E:\visual basics practical\shopping cart\shopping cart\App_Data\try.mdf; Integrated Security = True";
                String updatepass = "insert into orderdetails(orderid,sno,pro_id,product_name,price,quantity,dateoforder) values('" + Label2.Text + "'," + d.Rows[i]["sno"] + "," + d.Rows[i]["pro_id"] + ",'" + d.Rows[i]["product_name"] + "'," + d.Rows[i]["price"] + "," + d.Rows[i]["quantity"] + ",'" + Label3.Text + "')";
                SqlConnection s = new SqlConnection(mycon1);
                s.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = updatepass;
                cmd1.Connection = s;
                cmd1.ExecuteNonQuery();
                s.Close();
                saveaddress();

            }
            Response.Redirect("placedorder.aspx");

        }
    }

       
    }
