﻿using Service.ModelServices;
using System.Windows;
using UI.Login;
using UI.MainViews;

namespace UI
{
    public partial class MainWindow : Window
    {
        private LoginView? loginView;
        private AskEmailView? askEmailView;
        private ResetPasswordView? resetPasswordView;
        private EmployeeMainView? employeeMainView;
        private EmployeeService employeeService = new();

        public MainWindow(string? deepLink = null)
        {
            InitializeComponent();
            Task.Run(employeeService.WarmUp);
            ShowEmployeeMainView();
            //ProcessArgs(deepLink);
        }


        public void ProcessArgs(string? deepLink)
        {
            if (deepLink != null)
            {
                ProcessDeepLink(deepLink);
            }
            else
            {
                ShowLoginView();
            }
        }

        private void ProcessDeepLink(string deepLink)
        {
            if (deepLink.StartsWith("shiftflow://reset"))
            {
                ShowResetPasswordView(deepLink);            
            }
        }

        private void ShowLoginView()
        {
            loginView ??= new();
            loginView.Login += LoginView_Login;
            loginView.ForgotPassword += LoginView_ForgotPassword;
            MainWindowContent.Content = loginView;
        }

        private void ShowAskEmailView()
        {
            askEmailView ??= new();
            MainWindowContent.Content = askEmailView;
            askEmailView.BackToLoginAskEmail += AskEmailView_BackToLogin;
        }

        private void AskEmailView_BackToLogin(object sender, RoutedEventArgs e)
        {
            ShowLoginView();
        }

        private void LoginView_Login(object sender, RoutedEventArgs e)
        {
            ShowEmployeeMainView();
        }

        private void LoginView_ForgotPassword(object sender, RoutedEventArgs e)
        {
            ShowAskEmailView();
        }

        private void ResetPasswordView_ResetPassword(object sender, RoutedEventArgs e)
        {
            ShowLoginView();
        }

        private void ResetPasswordView_BackToLogin(object sender, RoutedEventArgs e)
        {
            ShowLoginView();
        }

        private void ShowResetPasswordView(string deepLink)
        {
            resetPasswordView ??= new(deepLink);
            MainWindowContent.Content = resetPasswordView;
            resetPasswordView.ResetPasswordClick += ResetPasswordView_ResetPassword;
            resetPasswordView.BackToLoginReset += ResetPasswordView_BackToLogin;
        }

        private void ShowEmployeeMainView()
        {
            employeeMainView ??= new();
            MainWindowContent.Content = employeeMainView;
        }
    }
}