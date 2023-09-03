import React, { useState, useEffect } from 'react';
import { IoIosArrowUp, IoIosArrowDown } from 'react-icons/io';
import { Link } from 'react-scroll';

const Navigation = ({ target, direction }) => {
  const [isMobile, setIsMobile] = useState(false);

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768);
    };

    handleResize(); 
    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  return (
    <div className={`bg-gray-900 flex justify-center ${direction === 'up' ? 'items-end' : 'items-start'}`}>
      {isMobile ? (
        <div /> // Render an empty div when on mobile to hide arrows
      ) : (
        <Link to={target} smooth={true} duration={500}>
          <button className={`text-white px-4 py-2 rounded-full ${direction === 'up' ? 'mb-4' : 'mt-4'}`}>
            {direction === 'up' ? <IoIosArrowUp className="w-8 h-8" /> : <IoIosArrowDown className="w-8 h-8 down-svg" />}
          </button>
        </Link>
      )}
    </div>
  );
};

export default Navigation;
