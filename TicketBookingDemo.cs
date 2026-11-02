
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Exercises.Exercise1
{
    public class TicketBookingDemo
    {
        public static async Task RunAsync()
        {
            const int totalCustomers = 100;

            var bookingSystem = new TicketBookingSystem();

            Console.WriteLine("===== MOVIE TICKET BOOKING SYSTEM =====");
            Console.WriteLine("Total seats: 50");
            Console.WriteLine("Total customers: 100");
            Console.WriteLine("Starting concurrent booking...\n");

            var tasks = Enumerable.Range(1, totalCustomers)
                .Select(customerId => Task.Run(() =>
                {
                    // Mỗi ghế được hai khách hàng cùng yêu cầu.
                    int seatNumber = ((customerId - 1) % 50) + 1;

                    bookingSystem.BookSeat(
                        seatNumber,
                        $"Customer_{customerId}");
                }))
                .ToArray();

            await Task.WhenAll(tasks);

            Console.WriteLine("\n===== BOOKING SUMMARY =====");

            Console.WriteLine(
                $"Successful bookings: {bookingSystem.SuccessfulBookings}");

            Console.WriteLine(
                $"Failed bookings: {bookingSystem.FailedBookings}");

            Console.WriteLine(
                $"Total requests: {bookingSystem.TotalRequests}");

            Console.WriteLine("\n===== VERIFICATION =====");

            Console.WriteLine(
                $"All 50 seats booked: " +
                $"{Enumerable.Range(1, 50).All(bookingSystem.IsSeatBooked)}");

            Console.WriteLine(
                $"Correct total requests: " +
                $"{bookingSystem.TotalRequests == totalCustomers}");
        }
    }
}
