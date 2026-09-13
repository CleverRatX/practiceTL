using Domain.Entities;
using Domain.Exceptions;

namespace Application.Rules
{
    public static class BookingRules
    {
        public static readonly TimeOnly ArrivalTime = new( 14, 0 );

        public static readonly TimeOnly DepartureTime = new( 12, 0 );

        public static int CountNights( DateOnly arrivalDate, DateOnly departureDate )
        {
            return departureDate.DayNumber - arrivalDate.DayNumber;
        }

        public static bool FitsGuests( RoomType roomType, int guestCount )
        {
            return guestCount >= roomType.MinPersonCount && guestCount <= roomType.MaxPersonCount;
        }

        public static bool CoversNight( Reservation reservation, DateOnly night )
        {
            return night >= reservation.ArrivalDate && night < reservation.DepartureDate;
        }

        public static int CountPeakOccupancy(
            IReadOnlyList<Reservation> reservations,
            DateOnly fromDate,
            DateOnly toDate )
        {
            int peakOccupancy = 0;

            for ( DateOnly night = fromDate; night < toDate; night = night.AddDays( 1 ) )
            {
                DateOnly currentNight = night;
                int occupiedRooms = reservations.Count( reservation => CoversNight( reservation, currentNight ) );

                if ( occupiedRooms > peakOccupancy )
                {
                    peakOccupancy = occupiedRooms;
                }
            }

            return peakOccupancy;
        }

        public static decimal CalculateTotal( RoomType roomType, int nights )
        {
            if ( nights <= 0 )
            {
                throw new DomainValidationException( "Количество ночей должно быть больше нуля." );
            }

            return roomType.DailyPrice * nights;
        }
    }
}
