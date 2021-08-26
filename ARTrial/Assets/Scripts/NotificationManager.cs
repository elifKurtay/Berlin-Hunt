using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //remove notification that had already been displayed
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        //creating notification channel
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notification Channel",
            Importance = Importance.Default,
            Description = "Alert notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        //creating the notification
        var notification = new AndroidNotification();
        notification.Title = "Discovery Time!";
        notification.Text = "There is a sightseeing location near you!";
        notification.FireTime = System.DateTime.Now.AddSeconds(15);

        //send the noti
        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        if(AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
