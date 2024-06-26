﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClient;

public partial class MainWindow : Window
{
    HubConnection connection;
    HubConnection counterConnection;
    public MainWindow()
    {
        InitializeComponent();

        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7080/chathub")
            .WithAutomaticReconnect()
            .Build();
        counterConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7080/counterhub")
            .WithAutomaticReconnect()
            .Build();

        connection.Reconnecting += (sender) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = "Attempting to reconnect...";
                messages.Items.Add(newMessage);
            });

            return Task.CompletedTask;
        };
        
        connection.Reconnected += (sender) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = "Reconnected to the server";
                messages.Items.Clear();
                messages.Items.Add(newMessage);
            });

            return Task.CompletedTask;
        };
        
        connection.Closed += (sender) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = "Connection closed";
                messages.Items.Add(newMessage);
                openConnection.IsEnabled = true;
                sendMessage.IsEnabled = false;
            });

            return Task.CompletedTask;
        };


    }

    private async void openConnection_Click(object sender, RoutedEventArgs e)
    {
        connection.On<string, string>("RecieveMessage", (user, message) => 
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = $"{user}: {message}";
                messages.Items.Add(newMessage);
            });
        });

        try
        {
            await connection.StartAsync();
            messages.Items.Add("Connection Started");
            openConnection.IsEnabled = false;
            sendMessage.IsEnabled = true;
        }
        catch (Exception ex)
        {
            messages.Items.Add(ex.Message);
        }
    }

    private async void sendMessage_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await connection.InvokeAsync("SendMessage", "WPF Client", messageInput.Text);
        }
        catch (Exception ex)
        {
            messages.Items.Add(ex.Message);
        }
    }

    private async void openCounter_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await counterConnection.StartAsync();
            openCounter.IsEnabled = false;
        }
        catch (Exception ex)
        {
            messages.Items.Add(ex.Message);
        }
    }

    private async void incrementCounter_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await counterConnection.InvokeAsync("AddToTotal", "WPF Counter", 1);
        }
        catch (Exception ex)
        {
            messages.Items.Add(ex.Message);
        }
    }
}
