import React, { useState } from 'react';
import axios from 'axios';
import FlightForm from './FlightForm';  
import FlightCard from './FlightCard';  
const FlightSearch = () => {
  const [origin, setOrigin] = useState('');
  const [destination, setDestination] = useState('');
  const [departureDate, setDepartureDate] = useState('');
  const [returnDate, setReturnDate] = useState('');
  const [adults, setAdults] = useState(1);
  const [currency, setCurrency] = useState('USD');
  const [flights, setFlights] = useState([]);
  const [message, setMessage] = useState('');
  const [isOneWay, setIsOneWay] = useState(false);

  
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${day}.${month}.${year}. ${hours}:${minutes}`;
  };


  const searchFlights = async () => {
    const requestData = {
      originLocationCode: origin,
      destinationLocationCode: destination,
      departureDate: departureDate,
      returnDate: isOneWay ? "" : returnDate, 
      adults: adults,
      currencyCode: currency,
    };

    
    const storedData = localStorage.getItem(JSON.stringify(requestData));

    if (storedData) {
      console.log("Fetching data from localStorage");
      const parsedData = JSON.parse(storedData);
      setFlights(parsedData);
      setMessage('');
      return;
    }

    try {
      const response = await axios.post('https://localhost:7077/api/home/search-flights', requestData, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      const data = response.data;

      if (data.message) {
        setMessage(data.message);
        setFlights([]);
      } else {
        setMessage('');
        setFlights(data.data);

        
        localStorage.setItem(JSON.stringify(requestData), JSON.stringify(data.data));
      }
    } catch (error) {
      setMessage('Error fetching flight data');
      console.error('Error fetching flights:', error);
    }
  };

  return (
    <div className="flight-search-container">
      <h2>Flight Search</h2>
      
      <FlightForm 
        origin={origin}
        setOrigin={setOrigin}
        destination={destination}
        setDestination={setDestination}
        departureDate={departureDate}
        setDepartureDate={setDepartureDate}
        returnDate={returnDate}
        setReturnDate={setReturnDate}
        adults={adults}
        setAdults={setAdults}
        currency={currency}
        setCurrency={setCurrency}
        isOneWay={isOneWay}
        setIsOneWay={setIsOneWay}
        searchFlights={searchFlights} 
      />

      
      {message && <div className="error-message">{message}</div>}

      
      <div className="flight-cards-container">
        {flights.length > 0 && flights.map((flight, index) => (
          <FlightCard 
            key={index} 
            flight={flight} 
            formatDate={formatDate} 
            index={index} 
          />
        ))}
      </div>
    </div>
  );
};

export default FlightSearch;