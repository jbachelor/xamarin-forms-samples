﻿using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using TodoAzure.WinPhone81;
using Windows.UI.Popups;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationProvider))]
namespace TodoAzure.WinPhone81
{
    public class AuthenticationProvider : IAuthenticate
    {
        MobileServiceUser user;

        public async Task<bool> AuthenticateAsync()
        {
            bool success = false;

            try
            {
                if (user == null)
                {
                    user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                    if (user != null)
                    {
                        var dialog = new MessageDialog(string.Format("You are now logged in - {0}", user.UserId), "Authentication");
                        await dialog.ShowAsync();
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Authentication Failed");
                await dialog.ShowAsync();
            }
            return success;
        }

        public async Task<bool> LogoutAsync()
        {
            bool success = false;
            try
            {
                if (user != null)
                {
                    await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();
                    var dialog = new MessageDialog(string.Format("You are now logged out - {0}", user.UserId), "Logout");
                    await dialog.ShowAsync();
                }

                user = null;
                success = true;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Logout failed");
                await dialog.ShowAsync();
            }
            return success;
        }
    }
}
