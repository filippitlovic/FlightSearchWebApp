import React from 'react';

const FlightForm = ({ 
  origin, setOrigin, 
  destination, setDestination, 
  departureDate, setDepartureDate, 
  returnDate, setReturnDate, 
  adults, setAdults, 
  currency, setCurrency, 
  isOneWay, setIsOneWay, 
  searchFlights 
}) => {
  return (
    <div className="flight-search-form">
      <label className="checkbox-container">
        <input
          type="checkbox"
          checked={isOneWay}
          onChange={() => setIsOneWay(!isOneWay)} 
        />
        One Way Flight
      </label>
      <div className="input-group">
        <label>Origin Airport (IATA):</label>
        <input
          type="text"
          value={origin}
          onChange={(e) => setOrigin(e.target.value)}
          placeholder="e.g. ZAG"
        />
      </div>
      <div className="input-group">
        <label>Destination Airport (IATA):</label>
        <input
          type="text"
          value={destination}
          onChange={(e) => setDestination(e.target.value)}
          placeholder="e.g. LHR"
        />
      </div>
      <div className="input-group">
        <label>Departure Date:</label>
        <input
          type="date"
          value={departureDate}
          onChange={(e) => setDepartureDate(e.target.value)}
        />
      </div>
      

      <div className="input-group">
        <label>Return Date:</label>
        <input
          type="date"
          value={returnDate}
          onChange={(e) => setReturnDate(e.target.value)}
          disabled={isOneWay} // if isOneWay checkbox is clicked, block returnDate
        />
      </div>
      <div className="input-group">
        <label>Number of Adults:</label>
        <input
          type="number"
          value={adults}
          onChange={(e) => setAdults(e.target.value)}
          min="1"
        />
      </div>
      <div className="input-group">
        <label>Currency:</label>
        <select value={currency} onChange={(e) => setCurrency(e.target.value)}>
          <option value="USD">USD</option>
          <option value="EUR">EUR</option>
          <option value="HRK">HRK</option>
        </select>
      </div>
      <button onClick={searchFlights} className="search-btn">Search Flights</button>
    </div>
  );
};

export default FlightForm;
