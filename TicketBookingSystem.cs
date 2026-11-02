
using System;
using System.Threading;

namespace Server.Exercises.Exercise1
{
    public class TicketBookingSystem
    {
        private const int TotalSeats = 50;

        private readonly bool[] seats = new bool[TotalSeats + 1];
        private readonly object seatLock = new object();

        private int successfulBookings;
        private int failedBookings;

        public int SuccessfulBookings =>
            Volatile.Read(ref successfulBookings);

        public int FailedBookings =>
            Volatile.Read(ref failedBookings);

        public int TotalRequests =>
            SuccessfulBookings + FailedBookings;

        public bool BookSeat(int seatNumber, string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException(
                    "Customer name cannot be empty.",
                    nameof(customerName));

            if (seatNumber < 1 || seatNumber > TotalSeats)
            {
                Interlocked.Increment(ref failedBookings);

                Console.WriteLine(
                    $"[FAILED] {customerName}: Invalid seat {seatNumber}");

                return false;
            }

            lock (seatLock)
            {
                if (seats[seatNumber])
                {
                    Interlocked.Increment(ref failedBookings);

                    Console.WriteLine(
                        $"[FAILED] {customerName}: Seat {seatNumber} is occupied.");

                    return false;
                }

                seats[seatNumber] = true;

                Interlocked.Increment(ref successfulBookings);

                Console.WriteLine(
                    $"[SUCCESS] {customerName} booked seat {seatNumber}.");

                return true;
            }
        }

        public bool IsSeatBooked(int seatNumber)
        {
            if (seatNumber < 1 || seatNumber > TotalSeats)
                throw new ArgumentOutOfRangeException(nameof(seatNumber));

            lock (seatLock)
            {
                return seats[seatNumber];
            }
        }
    }
}
