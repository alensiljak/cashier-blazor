using Cashier.Lib;
using Cashier.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.UnitTests
{
    public class SchedulerTests
    {
        private Scheduler _scheduler;

        public SchedulerTests() { 
            _scheduler = new Scheduler();
        }

        [Fact]
        public void TestProjection()
        {
            var startDate = DateOnly.FromDateTime(DateTime.Parse("2024-05-01"));
            var scX = new ScheduledXact
            {
                NextDate = DateOnly.FromDateTime(DateTime.Parse("2024-05-01"))
            };
            var endDate = DateOnly.FromDateTime(DateTime.Parse("2024-08-30"));

            _scheduler.ProjectSchedule(scX, startDate, endDate);

            // Assert
            Assert.True(false);
        }
    }
}
