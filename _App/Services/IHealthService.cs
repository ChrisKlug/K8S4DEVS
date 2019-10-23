using System;

namespace WebApplication.Services
{
    public interface IHealthService
    {
        void AddPing(bool success);
        void SetUnhealthy();

        bool IsHealthy { get; }
        (DateTime Time, bool Success)[] LastPings { get; }
    }
}