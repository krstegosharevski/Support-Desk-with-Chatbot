﻿namespace QueueManagementSystemAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }

        
        public ICollection<App1User>? Users { get; set; }
    }
}
