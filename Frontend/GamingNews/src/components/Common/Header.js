import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars } from '@fortawesome/free-solid-svg-icons';
// import logo from 'C:/Users/Admin/Desktop/glass/goblen/src/images/logo.png';

const Header = () => {
  const [menuOpen, setMenuOpen] = useState(false);

  const toggleMenu = () => {
    setMenuOpen(!menuOpen);
  };

  return (
    <header className="bg-gray-900 text-white py-2 z-10 fixed top-0 w-full border-slate-100 shadow-md"> 
      <div className="container mx-auto px-6 flex justify-between items-center">
        <div className="flex items-center">
        <Link to="/">
            {/* <img src={logo} alt="Logo" className="h-8 w-8 mr-2" /> */}
          </Link>
        </div>
        <nav className={`hidden md:flex space-x-16 text-lg ${menuOpen ? 'opacity-0' : 'opacity-100'}`}>
          <ul className="flex space-x-16 text-lg">
            <li><Link to="/" className="hover:text-gray-500 transition-colors duration-300">Home</Link></li>
            <li><Link to="/products" className="hover:text-red-600 transition-colors duration-300">Products</Link></li>
            <li><Link to="/location" className="hover:text-yellow-500 transition-colors duration-300">Location</Link></li>
            <li><Link to="/contact" className="hover:text-blue-500 transition-colors duration-300">Contact Us</Link></li>
          </ul>
        </nav>
        <div className="md:hidden">
          <FontAwesomeIcon
            icon={faBars}
            className="text-white text-2xl cursor-pointer"
            onClick={toggleMenu}
          />
        </div>
      </div>
      {menuOpen && (
        <div className="md:hidden bg-gray-900 w-full p-4">
          <ul className="space-y-4">
            <li><Link to="/" className="block text-white hover:text-gray-500 transition-colors duration-300">Home</Link></li>
            <li><Link to="/products" className="block text-white hover:text-red-600 transition-colors duration-300">Products</Link></li>
            <li><Link to="/location" className="block text-white hover:text-yellow-500 transition-colors duration-300">location Us</Link></li>
            <li><Link to="/contact" className="block text-white hover:text-blue-500 transition-colors duration-300">Contact Us</Link></li>
          </ul>
        </div>
      )}
      <div className="w-full h-0.25 bg-white"></div>
    </header>
  );
};

export default Header;