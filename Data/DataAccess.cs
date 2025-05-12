using AuthApp.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;

// Что бы создать начального пользователя:
// 1.Объявляем переменную для результата
// DECLARE @AddResult BIT;

// 2.Вызываем процедуру AddUser
// EXEC dbo.AddUser
//   @Login    = N'admin',       -- логин нового пользователя
//   @Password = N'admin123',-- пароль
//   @Role     = N'A',             -- роль: 'A' или 'U'
//   @Success  = @AddResult OUTPUT; --выходной параметр

// 3.Проверяем результат
// SELECT 
//   @AddResult AS UserAdded; --1 = успешно, 0 = пользователь существует

namespace AuthApp.Data
{
  public static class DataAccess
  {
    private static readonly string connString =
        ConfigurationManager.ConnectionStrings["AuthDB"].ConnectionString;

    public static SqlConnection GetConnection()
    {
      try
      {
        var conn = new SqlConnection(connString);
        conn.Open();
        EnsureUserTable(conn);
        EnsureStoredProcedures(conn);
        return conn;
      }
      catch (SqlException ex)
      {
        // При ошибке подключения показываем сообщение и останавливаем приложение
        MessageBox.Show($"Не удалось подключиться к базе данных: { ex.Message }", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
        return null;
      }
    }

    private static void EnsureUserTable(SqlConnection conn)
    {
      const string sql = @"
IF NOT EXISTS(SELECT * FROM sys.tables WHERE name='Пользователи')
BEGIN
    CREATE TABLE dbo.Пользователи (
        UserID INT IDENTITY(1,1) PRIMARY KEY,
        Login NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash VARBINARY(64) NOT NULL,
        Salt VARBINARY(16) NOT NULL,
        Role CHAR(1) NOT NULL CHECK(Role IN('A','U')),
        IsBlocked BIT NOT NULL DEFAULT 0,
        FailedAttempts INT NOT NULL DEFAULT 0,
        LastLogin DATETIME NULL
    );
END";
      using (var cmd = new SqlCommand(sql, conn)) cmd.ExecuteNonQuery();
    }

    private static void EnsureStoredProcedures(SqlConnection conn)
    {
      string sql = @"
-- AuthenticateUser
IF OBJECT_ID('dbo.AuthenticateUser','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.AuthenticateUser AS BEGIN SET NOCOUNT ON; END');
GO
ALTER PROCEDURE dbo.AuthenticateUser
    @Login NVARCHAR(50),
    @Password NVARCHAR(50),
    @LastLoginOut  DATETIME OUTPUT,
    @Result INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @uid       INT,
        @ph        VARBINARY(64),
        @salt      VARBINARY(16),
        @isBlocked BIT,
        @failed    INT,
        @last      DATETIME,
        @hash      VARBINARY(64);

    SELECT
        @uid       = UserID,
        @ph        = PasswordHash,
        @salt      = Salt,
        @isBlocked = IsBlocked,
        @failed    = FailedAttempts,
        @last      = LastLogin
    FROM dbo.Пользователи
    WHERE Login = @Login;

    IF @uid IS NULL BEGIN SET @Result = 1; RETURN; END
    IF @isBlocked = 1 BEGIN SET @Result = 2; RETURN; END

    SET @LastLoginOut = @last;

    SET @hash = HASHBYTES('SHA2_512', @salt + CAST(@Password AS VARBINARY(MAX)));
    IF @hash <> @ph
    BEGIN
        UPDATE dbo.Пользователи
           SET FailedAttempts = @failed + 1,
               IsBlocked      = CASE WHEN @failed + 1 >= 3 THEN 1 ELSE 0 END
         WHERE UserID = @uid;
        SET @Result = 1; RETURN;
    END

    IF @last IS NOT NULL AND DATEDIFF(DAY, @last, GETDATE()) >= 30
    BEGIN
        UPDATE dbo.Пользователи SET IsBlocked = 1 WHERE UserID = @uid;
        SET @Result = 4; RETURN;
    END

    UPDATE dbo.Пользователи
       SET FailedAttempts = 0
     WHERE UserID = @uid;

    UPDATE dbo.Пользователи
       SET LastLogin = GETDATE()
     WHERE UserID = @uid;

    SET @Result = 0;
END
GO

-- ChangeUserPassword
IF OBJECT_ID('dbo.ChangeUserPassword','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.ChangeUserPassword AS BEGIN SET NOCOUNT ON; END');
GO
ALTER PROCEDURE dbo.ChangeUserPassword
    @Login           NVARCHAR(50),
    @CurrentPassword NVARCHAR(50),
    @NewPassword     NVARCHAR(50),
    @Success         BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @uid         INT,
        @ph          VARBINARY(64),
        @salt        VARBINARY(16),
        @hashCurrent VARBINARY(64),
        @newSalt     VARBINARY(16),
        @newHash     VARBINARY(64);

    SELECT
        @uid  = UserID,
        @ph   = PasswordHash,
        @salt = Salt
    FROM dbo.Пользователи
    WHERE Login = @Login;

    IF @uid IS NULL BEGIN SET @Success = 0; RETURN; END

    SET @hashCurrent = HASHBYTES('SHA2_512', @salt + CAST(@CurrentPassword AS VARBINARY(MAX)));
    IF @hashCurrent <> @ph BEGIN SET @Success = 0; RETURN; END

    SET @newSalt = CRYPT_GEN_RANDOM(16);
    SET @newHash = HASHBYTES('SHA2_512', @newSalt + CAST(@NewPassword AS VARBINARY(MAX)));

    UPDATE dbo.Пользователи
       SET Salt         = @newSalt,
           PasswordHash = @newHash
     WHERE UserID = @uid;

    SET @Success = 1;
END
GO

-- AddUser
IF OBJECT_ID('dbo.AddUser','P') IS NULL
    EXEC('CREATE PROCEDURE dbo.AddUser AS BEGIN SET NOCOUNT ON; END');
GO
ALTER PROCEDURE dbo.AddUser
    @Login    NVARCHAR(50),
    @Password NVARCHAR(50),
    @Role     CHAR(1),
    @Success  BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Пользователи WHERE Login = @Login)
    BEGIN SET @Success = 0; RETURN; END

    DECLARE
        @salt VARBINARY(16) = CRYPT_GEN_RANDOM(16),
        @hash VARBINARY(64);

    SET @hash = HASHBYTES('SHA2_512', @salt + CAST(@Password AS VARBINARY(MAX)));

    INSERT INTO dbo.Пользователи
        (Login, Salt, PasswordHash, Role, IsBlocked, FailedAttempts)
    VALUES
        (@Login, @salt, @hash, @Role, 0, 0);

    SET @Success = 1;
END
GO
";

      var batches = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
      foreach (var batch in batches)
      {
        using (var cmd = new SqlCommand(batch, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public static (int Result, DateTime? LastLogin) Authenticate(string login, string password)
    {
      using (var conn = GetConnection())
      using (var cmd = new SqlCommand("dbo.AuthenticateUser", conn))
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Login", login);
        cmd.Parameters.AddWithValue("@Password", password);

        var pLast = new SqlParameter("@LastLoginOut", SqlDbType.DateTime)
        {
          Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(pLast);

        var pResult = new SqlParameter("@Result", SqlDbType.Int)
        {
          Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(pResult);

        cmd.ExecuteNonQuery();

        DateTime? lastLogin = pLast.Value == DBNull.Value
            ? (DateTime?)null
            : (DateTime)pLast.Value;

        return ((int)pResult.Value, lastLogin);
      }
    }

    public static bool ChangePassword(string login, string currentPassword, string newPassword)
    {
      using (var conn = GetConnection())
      {
        // Реализация смены пароля через хранимую процедуру
        using (var cmd = new SqlCommand("dbo.ChangeUserPassword", conn))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.AddWithValue("@Login", login);
          cmd.Parameters.AddWithValue("@CurrentPassword", currentPassword);
          cmd.Parameters.AddWithValue("@NewPassword", newPassword);
          var result = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
          cmd.Parameters.Add(result);
          cmd.ExecuteNonQuery();
          return (bool)result.Value;
        }
      }
    }

    public static bool AddUser(string login, string password, char role)
    {
      using (var conn = GetConnection())
      {
        using (var cmd = new SqlCommand("dbo.AddUser", conn))
        {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.AddWithValue("@Login", login);
          cmd.Parameters.AddWithValue("@Password", password);
          cmd.Parameters.AddWithValue("@Role", role);
          var result = new SqlParameter("@Success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
          cmd.Parameters.Add(result);
          cmd.ExecuteNonQuery();
          return (bool)result.Value;
        }
      }
    }

    public static void UnlockUser(string login)
    {
      using (var conn = GetConnection())
      using (var cmd = new SqlCommand(
          "UPDATE dbo.Пользователи SET IsBlocked=0, FailedAttempts=0 WHERE Login=@Login", conn))
      {
        cmd.Parameters.AddWithValue("@Login", login);
        cmd.ExecuteNonQuery();
      }
    }

    public static DataTable GetAllUsers()
    {
      using (var conn = GetConnection())
      using (var cmd = new SqlCommand("SELECT UserID, Login, Role, IsBlocked, FailedAttempts, LastLogin FROM dbo.Пользователи", conn))
      using (var da = new SqlDataAdapter(cmd))
      {
        var dt = new DataTable();
        da.Fill(dt);
        return dt;
      }
    }

    public static User GetUserByLogin(string login)
    {
      using (var conn = GetConnection())
      using (var cmd = new SqlCommand(
          "SELECT UserID, Login, Role, IsBlocked, FailedAttempts, LastLogin FROM dbo.Пользователи WHERE Login=@Login", conn))
      {
        cmd.Parameters.AddWithValue("@Login", login);
        using (var reader = cmd.ExecuteReader())
        {
          if (!reader.Read()) return null;
          return new User
          {
            UserID = reader.GetInt32(0),
            Login = reader.GetString(1),
            Role = reader.GetString(2)[0],
            IsBlocked = reader.GetBoolean(3),
            FailedAttempts = reader.GetInt32(4),
            LastLogin = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)
          };
        }
      }
    }
  }
}