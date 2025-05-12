using System;
using System.Windows.Forms;
using AuthApp.Data;
using AuthApp.Forms;

namespace AuthApp.Forms
{
  public partial class LoginForm : Form
  {
    public LoginForm()
    {
      InitializeComponent();
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
      string login = txtLogin.Text.Trim();
      string password = txtPassword.Text;

      if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
      {
        MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      var (result, lastLogin) = DataAccess.Authenticate(login, password);

      if (result != 0)
      {
        // Ошибки аутентификации
        switch (result)
        {
          case 1:
            MessageBox.Show("Вы ввели неверный логин или пароль.", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            break;
          case 2:
            MessageBox.Show("Вы заблокированы. Обратитесь к администратору.", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
            break;
          case 4:
            MessageBox.Show("Ваша учетная запись заблокирована из-за неактивности. Обратитесь к администратору.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            break;
          default:
            MessageBox.Show("Произошла неизвестная ошибка.", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            break;
        }
        return;
      }
      // Успешная аутентификация
      var user = DataAccess.GetUserByLogin(login);
      if (user == null)
      {
        MessageBox.Show("Ошибка при получении данных пользователя.", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      // Если первый вход (LastLogin == null) — меняем пароль
      if (lastLogin == null)
      {
        MessageBox.Show("Первый вход — требуется смена пароля.", "Информация",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        var changeForm = new ChangePasswordForm(user.Login);
        changeForm.ShowDialog();
        // Если администратор — открываем панель админа
        if (user.Role == 'A')
        {
          MessageBox.Show("Вы успешно авторизовались как Администратор.", "Информация",
              MessageBoxButtons.OK, MessageBoxIcon.Information);
          var adminForm = new AdminDashboard();
          adminForm.Show();
          this.Hide();
          return;
        }
        this.Close();
        return;
      }

      // Если администратор — открываем панель админа
      if (user.Role == 'A')
      {
        MessageBox.Show("Вы успешно авторизовались как Администратор.", "Информация",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        var adminForm = new AdminDashboard();
        adminForm.Show();
        this.Hide();
        return;
      }

      // Для обычного пользователя без специальных условий — просто уведомление
      MessageBox.Show("Вы успешно авторизовались.", "Информация",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
      // Тут можно открыть основную форму приложения для пользователя, если есть
    }
  }
}