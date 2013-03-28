<%@ Application Language="C#" %>
<%@ Import Namespace="Microsoft.Win32.TaskScheduler" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        return;
        // Code that runs on application startup

        log4net.Config.XmlConfigurator.Configure();

        // Get the service on the local machine
        using (TaskService ts = new TaskService())
        {
            // Create a new task definition and assign properties
            TaskDefinition td = ts.NewTask();
            td.RegistrationInfo.Description = "ShareAthon Service";

            // Create a trigger that will fire the task at this time every other day
            td.Triggers.Add(new DailyTrigger { DaysInterval = 1 });

            // Create an action that will launch Notepad whenever the trigger fires
            // set the path of the service exe
            td.Actions.Add(new ExecAction("ShareAthonService.exe", "", null));

            // Register the task in the root folder
            ts.RootFolder.RegisterTaskDefinition(@"ShareAthon", td);

            // Remove the task we just created
            //ts.RootFolder.DeleteTask("Test");
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends.
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer
        // or SQLServer, the event is not raised.

    }
</script>