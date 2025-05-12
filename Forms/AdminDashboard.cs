using System;
using System.Windows.Forms;
using AuthApp.Data;

namespace AuthApp.Forms
{
  public partial class AdminDashboard : Form
  {
    public AdminDashboard()
    {
      InitializeComponent();
      LoadUsers();
    }

    private void LoadUsers()
    {
      dgvUsers.DataSource = DataAccess.GetAllUsers();
    }

    private void btnAddUser_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtNewLogin.Text) || string.IsNullOrEmpty(txtNewPassword.Text) || cmbRole.SelectedItem == null)
      {
        MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      bool added = DataAccess.AddUser(txtNewLogin.Text.Trim(), txtNewPassword.Text, cmbRole.SelectedItem.ToString()[0]);
      if (added)
      {
        MessageBox.Show("Пользователь добавлен.", "Информация",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        LoadUsers();
      }
      else
      {
        MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnUnlockUser_Click(object sender, EventArgs e)
    {
      if (dgvUsers.SelectedRows.Count == 0)
      {
        MessageBox.Show("Выберите пользователя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      string login = dgvUsers.SelectedRows[0].Cells["Login"].Value.ToString();
      DataAccess.UnlockUser(login);
      MessageBox.Show("Пользователь разблокирован.", "Информация",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
      LoadUsers();
    }
  }
}