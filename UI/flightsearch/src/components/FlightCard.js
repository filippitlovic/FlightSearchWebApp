import React from 'react';

const FlightCard = ({ flight, formatDate, index }) => {
 
  const getLayovers = (itinerary) => {
    const numberOfSegments = itinerary.segments.length;
    if (numberOfSegments <= 1) {
      return 0;  
    }
    return numberOfSegments - 1;  
  };

  return (
    <div className="flight-card">
      <h3>Offer {index + 1}</h3>

      
      {flight.itineraries.length > 0 && (
        <div className="itinerary-container">
          <h4>Departure Flight</h4>
          {flight.itineraries[0].segments.map((segment, j) => (
            <div key={j} className="flight-segment">
              <p><strong>Departure:</strong> {segment.departure.iataCode} at {formatDate(segment.departure.at)}</p>
              <p><strong>Arrival:</strong> {segment.arrival.iataCode} at {formatDate(segment.arrival.at)}</p>
            </div>
          ))}
          
          <p><strong>Layovers:</strong> {getLayovers(flight.itineraries[0])}</p>
        </div>
      )}

      
      {flight.itineraries.length > 1 && (
        <div className="itinerary-container">
          <h4>Return Flight</h4>
          {flight.itineraries[1].segments.map((segment, j) => (
            <div key={j} className="flight-segment">
              <p><strong>Departure:</strong> {segment.departure.iataCode} at {formatDate(segment.departure.at)}</p>
              <p><strong>Arrival:</strong> {segment.arrival.iataCode} at {formatDate(segment.arrival.at)}</p>
            </div>
          ))}
         
          <p><strong>Layovers:</strong> {getLayovers(flight.itineraries[1])}</p>
        </div>
      )}

      
      <div className="flight-price">
        <p><strong>Total Price:</strong> {flight.price.currency} {flight.price.total}</p>
      </div>
    </div>
  );
};

export default FlightCard;
