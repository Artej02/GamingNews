import React from 'react';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import { Routes, Route as RRoute } from 'react-router';
import Header from './components/Common/Header';
import Home from './components/Home/Home';
import Products from './components/Products/Products';
import Location from './components/Location/Location';
import Contact from './components/Contact/Contact';

const App = () => {
  return (
    <Router>
      <div className="relative">
        <Header className="fixed top-0 left-0 w-full z-50" /> {/* Apply Tailwind classes */}
        <div className="pt-6"> {/* Create space below fixed header */}
          <Routes>
            <RRoute path="/" element={<Home />} />
            <RRoute path="/products" element={<Products />} />
            <RRoute path="/location" element={<Location />} />
            <RRoute path="/contact" element={<Contact />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
};

export default App;
