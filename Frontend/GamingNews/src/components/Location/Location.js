import React from 'react';
import Footer from '../Common/Footer';
import './Location.css';
const Location = () => {
  return (
    <div id='start' className="bg-gray-900 flex flex-col min-h-screen mt-[-25px]">
      {/* Location */}
      <div id="location" className="flex flex-col justify-center items-center flex-grow">
        <div className="mx-auto max-w-2xl py-10 sm:py-16 lg:py-20">
          <div className="text-center">
            <h1 className="text-2xl sm:text-4xl font-bold tracking-tight text-white">
              Our Location
            </h1>
            <div className="mt-4 sm:mt-6 border-4 border-red-600 rounded-lg overflow-hidden mb-[-30px]">
              <iframe
                title="Location Map"
                src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2934.1853946735596!2d21.126324475512597!3d42.65742651626381!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x13549fc047f6615d%3A0xb6424b5df887a208!2sGoblen%20Glass!5e0!3m2!1sen!2s!4v1692334652493!5m2!1sen!2s"
                width="800"
                height="500" // Desktop height
                style={{ border: '0' }}
                allowFullScreen=""
                loading="lazy"
                referrerPolicy="no-referrer-when-downgrade"
                className="location-iframe" // Add a class name
              ></iframe>
               
            </div>
          </div>
         
        </div>
      </div>
    <div><Footer /></div>
    </div>
  );
};

export default Location;
