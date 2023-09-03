import React, { useEffect, useRef, useState } from 'react';
// import logo from 'C:/Users/Admin/Desktop/glass/goblen/src/images/logo.png';

const Footer = () => {
  const [isVisible, setIsVisible] = useState(true);
  const footerRef = useRef(null);

  const handleIntersection = (entries) => {
    const [entry] = entries;
    setIsVisible(!entry.isIntersecting);
  };

  useEffect(() => {
    const observer = new IntersectionObserver(handleIntersection, {
      root: null,
      rootMargin: '0px',
      threshold: 0,
    });

    if (footerRef.current) {
      observer.observe(footerRef.current);
    }

    return () => {
      if (footerRef.current) {
        observer.unobserve(footerRef.current);
      }
    };
  }, []);

  return (
    <>
      {isVisible && (
        <footer
          ref={footerRef}
          className={`bg-gray-900 text-white py-2 z-10 w-full shadow-mt fade-in`}
        >
          <div className="container mx-auto px-6 flex flex-col md:flex-row justify-center items-center">
            <div className="flex items-center mb-2 md:mb-0">
              {/* <img src={logo} alt="Logo" className="h-8 w-8 mr-2" /> */}
              <span className="text-sm">Copyright by Gaming News, 2023</span>
            </div>
          </div>
        </footer>
      )}
    </>
  );
};

export default Footer;
