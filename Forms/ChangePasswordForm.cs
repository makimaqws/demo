using System;
using System.Windows.Forms;
using AuthApp.Data;

namespace AuthApp.Forms
{
  public partial class ChangePasswordForm : Form
  {
    private readonly string _login;

    public ChangePasswordForm(string login)
    {
      InitializeComponent();
      _login = login;
    }

    private void btnChange_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtCurrent.Text) || string.IsNullOrEmpty(txtNew.Text) || string.IsNullOrEmpty(txtConfirm.Text))
      {
        MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      if (txtNew.Text != txtConfirm.Text)
      {
        MessageBox.Show("Новый пароль не совпадает с подтверждением.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      bool ok = DataAccess.ChangePassword(_login, txtCurrent.Text, txtNew.Text);
      if (ok)
      {
        MessageBox.Show("Пароль успешно изменён.", "Информация",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.Close();
      }
      else
      {
        MessageBox.Show("Текущий пароль неверен.", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}