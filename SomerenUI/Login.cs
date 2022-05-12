using SomerenLogic;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SomerenUI
{
    public partial class Login : Form
    {

        UserService userService = new UserService();
        SHA256 sha256Hash = SHA256.Create();

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            showPanel("Login");
        }

        private void showPanel(string panelName)
        {
            HideAllPanels();

            if (panelName == "Login")
            {
                pnlLogin.Show();

            }
            else if (panelName == "Register")
            {
                pnlRegister.Show();

            }
            else if (panelName == "Forgot")
            {
                pnlForgot.Show();
                btnResetPassword.Enabled = false;
            }
        }

        #region Login

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Please fill in both Username and Password", "Log in failed");
                    return;
                }

                // if username exsists
                int checkName = userService.CheckExistingUser(txtUserName.Text);
                if (checkName == 0)
                {
                    MessageBox.Show("Username does not exist, check the name or register as new user", "Log in failed");
                    return;
                }
                else
                {
                    User user = userService.GetByName(txtUserName.Text);
                    if (VerifyHash(sha256Hash, txtPassword.Text + user.Salt, user.Password))
                    {
                        SomerenUI somerenUI = new SomerenUI();
                        somerenUI.Show();
                        this.Hide();
                        somerenUI.Closed += (s, args) => this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wrong password", "Log in failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loging in");
            }
        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            showPanel("Register");
        }

        private void btnForgotPassword_Click_1(object sender, EventArgs e)
        {
            int checkName = userService.CheckExistingUser(txtUserName.Text);
            if (checkName == 0)
            {
                MessageBox.Show("Username does not exist", "Enter valid username");
                return;
            }
            else
            {
                User user = userService.GetByName(txtUserName.Text);
                showPanel("Forgot");
                lblDisplayUsername.Text = user.Username;
                lblShowQuestion.Text = user.Question;
            }
        }

        #endregion

        #region Regsiter

        private void btnConfirmRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRegisterPassword.Text == "" || txtRegisterUsername.Text == "" || txtConfirmPassword.Text == "" || txtSecretAnswer.Text == "" || txtSecretQuestion.Text == "" || txtLicenseKey.Text == "")
                {
                    MessageBox.Show("Please fill in all fields", "Registration  failed");
                    return;
                }
                int checkName = userService.CheckExistingUser(txtRegisterUsername.Text);
                if (checkName == 1)
                {
                    MessageBox.Show("Username already taken, Please choose a different name", "Registration failed");
                    return;
                }
                else
                {
                    if (txtRegisterPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("Please fill in matching passwords", "Registration failed");
                        return;
                    }
                    else
                    {
                        string enterdKey = LicenseKey(txtLicenseKey.Text);
                        User KeyHolder = userService.GetByName("L.Key"); // License key is hashed in db
                        if (!VerifyHash(sha256Hash, enterdKey + KeyHolder.Salt, KeyHolder.Password)) // Verify License Key
                        {
                            MessageBox.Show("Wrong license key", "Registration failed");
                            return;
                        }
                        else
                        {
                            User user = new User();
                            user.Username = txtRegisterUsername.Text;
                            user.Salt = MakeSalt();
                            user.Password = CreateSafePassword(txtRegisterPassword.Text, user.Salt);
                            user.Question = txtSecretQuestion.Text;
                            user.Answer = txtSecretAnswer.Text;
                            userService.AddUser(user);
                            MessageBox.Show($"User {user.Username} was added sucessfully", "Registration is done");
                            ClearAllTextBoxes();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while registering to the system");
            }
        }

        private void ClearAllTextBoxes()
        {
            txtRegisterPassword.Text = "";
            txtRegisterUsername.Text = "";
            txtConfirmPassword.Text = "";
            txtSecretAnswer.Text = "";
            txtSecretQuestion.Text = "";
            txtLicenseKey.Text = "";
        }

        private void btnRegisterGoBack_Click(object sender, EventArgs e)
        {
            showPanel("Login");
        }

        private string LicenseKey(string LKey)
        {
            LKey = LKey.Replace("-", "");
            LKey = LKey.Replace(" ", "");
            return LKey;
        }

        private string MakeSalt()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            char[] stringChars = new char[32];
            Random random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string salt = new String(stringChars);
            return salt;
        }

        #endregion

        #region ForgotPassword

        private void btnVerify_Click(object sender, EventArgs e)
        {
            User user = userService.GetByName(lblDisplayUsername.Text);
            if (txtForgotAnswer.Text == user.Answer)
            {
                btnResetPassword.Enabled = true;
            }
            else
            {
                MessageBox.Show("Wrong answer", "Can not change passowrd");
                return;
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewPassword.Text == txtEnterAgainPassword.Text)
                {
                    User user = userService.GetByName(lblDisplayUsername.Text);
                    user.Salt = MakeSalt();
                    user.Password = CreateSafePassword(txtNewPassword.Text, user.Salt);
                    userService.UpdatePassword(user);
                    MessageBox.Show("Password updated sucssesfully", "Changes saved");
                }
                else
                {
                    MessageBox.Show("Please enter two identical passwords", "Password reset failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while resetting the password");
            }
        }

        private void btnForgotGoBack_Click(object sender, EventArgs e)
        {
            showPanel("Login");
        }


        #endregion

        private string CreateSafePassword(string plainPassword, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return GetHash(sha256Hash, plainPassword + salt);
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        private void HideAllPanels()
        {
            pnlLogin.Hide();
            pnlForgot.Hide();
            pnlRegister.Hide();
        }

        private void ErrorProcess(Exception ex, string messege)
        {
            MessageBox.Show(messege, "Error occured");

            // write to error log file
            StreamWriter sw = File.AppendText("..\\..\\..\\Error Log.txt");
            sw.WriteLine($"Error occured at: {DateTime.Now}:");
            sw.WriteLine(messege);
            Console.WriteLine(ex);
            Console.WriteLine();
            sw.Close();
        }
    }
}
