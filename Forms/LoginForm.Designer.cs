namespace AuthApp.Forms
{
  partial class LoginForm
  {
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TextBox txtLogin;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnLogin;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtLogin = new System.Windows.Forms.TextBox();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.btnLogin = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // txtLogin
      // 
      this.txtLogin.Location = new System.Drawing.Point(30, 30);
      this.txtLogin.Name = "txtLogin";
      this.txtLogin.Size = new System.Drawing.Size(200, 20);
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(30, 70);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new System.Drawing.Size(200, 20);
      // 
      // btnLogin
      // 
      this.btnLogin.Location = new System.Drawing.Point(30, 110);
      this.btnLogin.Name = "btnLogin";
      this.btnLogin.Size = new System.Drawing.Size(75, 23);
      this.btnLogin.Text = "Войти";
      this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
      // 
      // LoginForm
      // 
      this.ClientSize = new System.Drawing.Size(260, 160);
      this.Controls.Add(this.txtLogin);
      this.Controls.Add(this.txtPassword);
      this.Controls.Add(this.btnLogin);
      this.Name = "LoginForm";
      this.Text = "Авторизация";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}