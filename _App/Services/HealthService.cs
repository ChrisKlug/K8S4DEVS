using System;
using System.Collections.Generic;

namespace WebApplication.Services
{

    public class HealthService : IHealthService
    {
        private Queue<(DateTime Time, bool Success)> _pingQueue = new Queue<(DateTime time, bool Success)>();

        public void SetUnhealthy()
        {
            this.IsHealthy = false;
        }

        public void AddPing(bool success)
        {
            _pingQueue.Enqueue((Time: DateTime.Now, Success: success));
            if (_pingQueue.Count > 5) {
                _pingQueue.Dequeue();
            }
            
        }

        public bool IsHealthy
        {
            get;
            private set;
        } = true;

        public (DateTime Time, bool Success)[] LastPings => _pingQueue.ToArray();
    }
}