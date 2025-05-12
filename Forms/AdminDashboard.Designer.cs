using System.Windows.Forms;

namespace AuthApp.Forms
{
  partial class AdminDashboard
  {
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.DataGridView dgvUsers;
    private System.Windows.Forms.TextBox txtNewLogin;
    private System.Windows.Forms.TextBox txtNewPassword;
    private System.Windows.Forms.ComboBox cmbRole;
    private System.Windows.Forms.Button btnAddUser;
    private System.Windows.Forms.Button btnUnlockUser;

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
      this.dgvUsers = new System.Windows.Forms.DataGridView();
      this.txtNewLogin = new System.Windows.Forms.TextBox();
      this.txtNewPassword = new System.Windows.Forms.TextBox();
      this.cmbRole = new System.Windows.Forms.ComboBox();
      this.btnAddUser = new System.Windows.Forms.Button();
      this.btnUnlockUser = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvUsers
      // 
      this.dgvUsers.Location = new System.Drawing.Point(20, 20);
      this.dgvUsers.Name = "dgvUsers";
      this.dgvUsers.Size = new System.Drawing.Size(500, 200);
      this.dgvUsers.ReadOnly = true;
      this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      // 
      // txtNewLogin
      // 
      this.txtNewLogin.Location = new System.Drawing.Point(20, 240);
      this.txtNewLogin.Name = "txtNewLogin";
      this.txtNewLogin.Size = new System.Drawing.Size(120, 23);
      // 
      // txtNewPassword
      // 
      this.txtNewPassword.Location = new System.Drawing.Point(160, 240);
      this.txtNewPassword.Name = "txtNewPassword";
      this.txtNewPassword.PasswordChar = '*';
      this.txtNewPassword.Size = new System.Drawing.Size(120, 23);
      // 
      // cmbRole
      // 
      this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbRole.Items.AddRange(new object[] { "A", "U" });
      this.cmbRole.Location = new System.Drawing.Point(300, 240);
      this.cmbRole.Name = "cmbRole";
      this.cmbRole.Size = new System.Drawing.Size(60, 23);
      // 
      // btnAddUser
      // 
      this.btnAddUser.Location = new System.Drawing.Point(380, 240);
      this.btnAddUser.Name = "btnAddUser";
      this.btnAddUser.Size = new System.Drawing.Size(75, 23);
      this.btnAddUser.Text = "Добавить";
      this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
      // 
      // btnUnlockUser
      // 
      this.btnUnlockUser.Location = new System.Drawing.Point(460, 240);
      this.btnUnlockUser.Name = "btnUnlockUser";
      this.btnUnlockUser.Size = new System.Drawing.Size(75, 23);
      this.btnUnlockUser.Text = "Разблокировать";
      this.btnUnlockUser.Click += new System.EventHandler(this.btnUnlockUser_Click);
      // 
      // AdminDashboard
      // 
      this.ClientSize = new System.Drawing.Size(540, 280);
      this.Controls.Add(this.dgvUsers);
      this.Controls.Add(this.txtNewLogin);
      this.Controls.Add(this.txtNewPassword);
      this.Controls.Add(this.cmbRole);
      this.Controls.Add(this.btnAddUser);
      this.Controls.Add(this.btnUnlockUser);
      this.Name = "AdminDashboard";
      this.Text = "Администрирование";
      ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}