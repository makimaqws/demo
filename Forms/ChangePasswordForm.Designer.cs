namespace AuthApp.Forms
{
  partial class ChangePasswordForm
  {
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TextBox txtCurrent;
    private System.Windows.Forms.TextBox txtNew;
    private System.Windows.Forms.TextBox txtConfirm;
    private System.Windows.Forms.Button btnChange;

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
      this.txtCurrent = new System.Windows.Forms.TextBox();
      this.txtNew = new System.Windows.Forms.TextBox();
      this.txtConfirm = new System.Windows.Forms.TextBox();
      this.btnChange = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // txtCurrent
      // 
      this.txtCurrent.Location = new System.Drawing.Point(30, 30);
      this.txtCurrent.Name = "txtCurrent";
      this.txtCurrent.PasswordChar = '*';
      this.txtCurrent.Size = new System.Drawing.Size(200, 20);
      // 
      // txtNew
      // 
      this.txtNew.Location = new System.Drawing.Point(30, 70);
      this.txtNew.Name = "txtNew";
      this.txtNew.PasswordChar = '*';
      this.txtNew.Size = new System.Drawing.Size(200, 20);
      // 
      // txtConfirm
      // 
      this.txtConfirm.Location = new System.Drawing.Point(30, 110);
      this.txtConfirm.Name = "txtConfirm";
      this.txtConfirm.PasswordChar = '*';
      this.txtConfirm.Size = new System.Drawing.Size(200, 20);
      // 
      // btnChange
      // 
      this.btnChange.Location = new System.Drawing.Point(30, 150);
      this.btnChange.Name = "btnChange";
      this.btnChange.Size = new System.Drawing.Size(100, 23);
      this.btnChange.Text = "Изменить пароль";
      this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
      // 
      // ChangePasswordForm
      // 
      this.ClientSize = new System.Drawing.Size(260, 200);
      this.Controls.Add(this.txtCurrent);
      this.Controls.Add(this.txtNew);
      this.Controls.Add(this.txtConfirm);
      this.Controls.Add(this.btnChange);
      this.Name = "ChangePasswordForm";
      this.Text = "Смена пароля";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}