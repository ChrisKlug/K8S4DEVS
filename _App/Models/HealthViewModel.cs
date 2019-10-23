using System;

namespace WebApplication.Models
{
    public class HealthViewModel
    {
        public bool IsHealthy { get; set; }
        public (DateTime Time, bool Success)[] Pings { get; set; }
    }
}